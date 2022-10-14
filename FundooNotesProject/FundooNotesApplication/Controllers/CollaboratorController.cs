using BusinessLayer.interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FundooNotesApplication.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        ICollaboratorBL collaboratorBL;
        public CollaboratorController(ICollaboratorBL collaboratorBL)
        {
            this.collaboratorBL = collaboratorBL;
        }

        [HttpPost("AddCollaborator")]
        public IActionResult AddCollaborator(CollaboratorModel collaboratorModel)
        {
            try
            {
                int userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = collaboratorBL.AddCollaborator(collaboratorModel, userID);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Collaborator added successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Collaborator can't  be added" });
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCollaboratorsByNoteID")]
        public IActionResult GetCollaboratorsByNoteID(int NoteId)
        {
            try
            {
                int userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = collaboratorBL.GetCollaboratorsByID(NoteId, userID);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "geting Collaborator successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "not geting Collaborator successfully" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("RemoveCollaborator")]
        public IActionResult RemoveCollaborator(int CollaboratorId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                if (collaboratorBL.RemoveCollaborator(CollaboratorId, userId))
                {
                    return this.Ok(new { success = "true", message = "Collaborator removed successfully" });
                }
                else
                {
                    return this.BadRequest(new { success = "false", message = "Collaborator can't removed successfully" });
                }
            }
            catch (Exception)
            {

                throw null;
            }
        }
    }
}
