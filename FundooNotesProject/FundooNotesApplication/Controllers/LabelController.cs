

namespace FundooNotesApplication.Controllers
{  
    using BusinessLayer.interfaces;
    using CommonLayer.Model;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        ILabelBL labelBL;
        FundooDBContext fundooDBContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="labelBL"></param>
        /// <param name="fundooDBContext"></param>
        /// <param name="memoryCache"></param>
        /// <param name="distributedCache"></param>
        public LabelController(ILabelBL labelBL, FundooDBContext fundooDBContext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.labelBL = labelBL;
            this.fundooDBContext = fundooDBContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        [HttpPost("CreateLabel")]
        public IActionResult CreateLabel(LabelModel labelModel)
        {
            try
            {
                int userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = labelBL.CreateLabel(labelModel,userID);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Label added successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Label already created" });
                }
            }
            catch (Exception)
            {

                throw null;
            }

        }
        [HttpPut("EditLabel")]
        public IActionResult EditLabel(UpdateLabelModel updateLabel)
        {
            try
            {
                int userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = labelBL.EditLabel(updateLabel, userID);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Label renamed successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Label can't renamed" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpDelete("DeleteByLName")]
        public IActionResult RemoveLabel(string labelName)
        {
            try
            {
                int userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = labelBL.RemoveLabel(userID, labelName);
                if (result)
                {
                    return this.Ok(new { success = true, message = "Label removed successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Label can't be removed successfully" });
                }

            }
            catch(Exception e)
            {
                throw e;
            }
        }
        [HttpDelete("DeleteByNID")]
        public IActionResult RemoveLabelByNoteID(LabelModel labelModel)
        {
            try
            {
                int userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = labelBL.RemoveLabelByNoteID(labelModel, userID);
                if (result)
                {
                    return this.Ok(new { success = true, message = "Label removed successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Label can't be removed successfully" });
                }

            }catch(Exception e)
            {
                throw e;
            }
        }
        [HttpGet("RetriveByID")]
        public IActionResult GetLabelsByNoteID(int noteID)
        {
            try
            {
                int userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = labelBL.GetLabelsByNoteID(noteID, userID);
                if (result!=null)
                {
                    return this.Ok(new { success = true, message = "getting Label list successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Unable to retrive level list successfully" });
                }

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllUserUsingRedisCache()
        {
            var cacheKey = "LabelEntityList";
            string serializedNoteEntityList;
            var labelEntityList = new List<LabelEntity>();
            var redisNoteEntityList = await distributedCache.GetAsync(cacheKey);
            if (redisNoteEntityList != null)
            {
                serializedNoteEntityList = Encoding.UTF8.GetString(redisNoteEntityList);
                labelEntityList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedNoteEntityList);
            }
            else
            {
                labelEntityList = await fundooDBContext.LabelTable.ToListAsync();
                serializedNoteEntityList = JsonConvert.SerializeObject(labelEntityList);
                redisNoteEntityList = Encoding.UTF8.GetBytes(serializedNoteEntityList);
                var options = new DistributedCacheEntryOptions()
           .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
           .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNoteEntityList, options);
            }
            return Ok(labelEntityList);
        }


    }
}
