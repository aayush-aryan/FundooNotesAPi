using BusinessLayer.interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL UserBL;
        FundooDBContext fundooDBContext;
        ResponseModel responseModel;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        ILogger<UserEntity> logger;

        public UserController(IUserBL userBL, FundooDBContext fundooDBContext, ResponseModel responseModel, IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<UserEntity> logger)
        {
            this.fundooDBContext = fundooDBContext;
            this.UserBL = userBL;
            this.responseModel = responseModel;
            this.memoryCache = memoryCache;
            this.distributedCache=distributedCache;
            this.logger = logger;
        }

        [HttpPost("Register")]
        public IActionResult Registration(Registration registration)
        {
            try
            {
                var result = UserBL.UserRegistration(registration);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "UserRegistration Successsfull", data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "UserRegistration UnSuccesssfull" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginModel loginModel)
        {
            logger.LogInformation("hhvjhj");
            try
            {
                var userdata = UserBL.LoginUser(loginModel);
                if (userdata != null)
                {
                    return this.Ok(new { ResponseCode = "200", massage = "Success", SecurityToken = $"{userdata}" });
                }
                return this.NotFound(new { ResponseCode = "404", message = $"Email And PassWord Is Invalid" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string emailId)
        {
            try
            {
                var result = UserBL.ForgetPassword(emailId);
                if (result != null)
                {
                    return this.Ok(new { Sucess = true, message = "email sends successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "email doesnot send successfully" });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ResetPassword")]
        // [Authorize]
        // [AutoValidateAntiforgeryToken]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {

                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                responseModel = UserBL.ResetPassword(model, email);
                if (responseModel != null)
                {
                    return this.Ok(new { ResponseCode = responseModel.ResponseCode, message = responseModel.ResponseMassage });
                }
                else
                {
                    return this.BadRequest(new { ResponseCode = responseModel.ResponseCode, message = responseModel.ResponseMassage });
                }
            }
            catch (Exception e)
            {
                // throw e;
                return null;
            }
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllUserUsingRedisCache()
        {
            var cacheKey = "UserEntityList";
            string serializedNoteEntityList;
            var userEntityList = new List<UserEntity>();
            var redisNoteEntityList = await distributedCache.GetAsync(cacheKey);
            if (redisNoteEntityList != null)
            {
                serializedNoteEntityList = Encoding.UTF8.GetString(redisNoteEntityList);
                userEntityList = JsonConvert.DeserializeObject<List<UserEntity>>(serializedNoteEntityList);
            }
            else
            {
                userEntityList = await fundooDBContext.UserTable.ToListAsync();
                serializedNoteEntityList = JsonConvert.SerializeObject(userEntityList);
                redisNoteEntityList = Encoding.UTF8.GetBytes(serializedNoteEntityList);
                var options = new DistributedCacheEntryOptions()
           .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
           .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNoteEntityList, options);
            }
            return Ok(userEntityList);
        }


    }
}
