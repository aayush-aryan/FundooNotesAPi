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
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApplication.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class NoteController : ControllerBase
    {
        FundooDBContext fundooDBContext;
        INoteBL noteBL;
        ResponseModel responseModel;
       // private object manager;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public NoteController(INoteBL noteBL, FundooDBContext fundooContext, ResponseModel responseModel, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.noteBL = noteBL;
            this.fundooDBContext = fundooContext;
            this.responseModel = responseModel;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        [HttpPost("AddNote")]
        public IActionResult AddNote(NoteModel noteModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                var result = noteBL.AddNote(Convert.ToInt32(userid.Value), noteModel);
                if (result != null)
                {
                    return this.Ok(new { Sucess = true, message = "Notes added successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes doesnot added successfully" });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetNote")]
        public IActionResult GetNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userid", StringComparison.InvariantCultureIgnoreCase));
                var result = noteBL.GetNote(noteId, Convert.ToInt32(userid.Value));
                if (result != null)
                {
                    return this.Ok(new { Sucess = true, message = "Get note successfully", result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes doesnot get " });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetAllNotes")]
        public IActionResult GetAllNotes()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                var result = noteBL.GetAllNotes(Convert.ToInt32(userid.Value));
                if (result != null)
                {
                    return this.Ok(new { Sucess = true, message = "Get All note successfully", result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes doesnot get " });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("DeleteUserNote")]
        public IActionResult DeleteUserNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                var result = noteBL.DeleteUserNote(noteId, Convert.ToInt32(userid.Value));
                if (result != 0)
                {
                    return this.Ok(new { Sucess = true, message = "Delete note successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes doesnot deleted " });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPut("UpdateUserNote")]
        public IActionResult UpdateUserNote(int noteId, NoteModel noteModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                var result = noteBL.UpdateUserNote(noteId, Convert.ToInt32(userid.Value), noteModel);
                if (result != 0)
                {
                    return this.Ok(new { Sucess = true, message = "Updated note successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes doesnot Udated " });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPut("ColorNotes")]
        public IActionResult ColorNotes(int noteID, string color)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = noteBL.UpdateColor(userId, noteID, color);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Color changed successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Color not changed  " });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPut("ArchieveNotes")]
        public IActionResult ArchieveNotes(int noteID)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                var result = noteBL.ArchieveNotes(Convert.ToInt32(userId.Value), noteID);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Archieved successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = " not Archieved successfully" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("ToPinNotes")]
        public IActionResult ToPinNotes(int noteID)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = noteBL.ToPinNotes(userId, noteID);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Notes get pinned  successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes not pinned  successfully" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("TrashNotes")]
        public IActionResult TrashNotes(int noteID)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = noteBL.ToTrashNotes(userId, noteID);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Notes gets Trashed", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes doesnot Trashed" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("AddImage")]
        public IActionResult AddImage(int noteId, IFormFile image)
        {
            try
            {
                bool note = this.noteBL.AddImage(noteId, image);
                if (note)
                {
                    return this.Ok(new { Status = true, Message = "Image uploaded Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Failed to upload image" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NoteEntityList";
            string serializedNoteEntityList;
            var noteEntityList = new List<NoteEntity>();
            var redisNoteEntityList = await distributedCache.GetAsync(cacheKey);
            if (redisNoteEntityList != null)
            {
                serializedNoteEntityList = Encoding.UTF8.GetString(redisNoteEntityList);
                noteEntityList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNoteEntityList);
            }
            else
            {
                noteEntityList = await fundooDBContext.NoteTable.ToListAsync();
                serializedNoteEntityList = JsonConvert.SerializeObject(noteEntityList);
                redisNoteEntityList = Encoding.UTF8.GetBytes(serializedNoteEntityList);
                var options = new DistributedCacheEntryOptions()
           .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
           .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNoteEntityList, options);
            }
            return Ok(noteEntityList);
        }
    }
}
