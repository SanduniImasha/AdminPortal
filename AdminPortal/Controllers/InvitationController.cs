﻿using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Admin.Portal.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Admin.Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {

        private readonly Settings _config;
        private readonly DataContext _dbContext;

        public InvitationController(IOptions<Settings> settings, DataContext dbContext)
        {
            _config = settings.Value;
            _dbContext = dbContext;
        }

        [HttpPost, Route("SendInvitation")]
        public async Task<IActionResult> SendInvitation([FromBody] InvitationRequest context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(_config, _dbContext);
            UserModel senderUser = await db.GetUserByEmail(context.SenderEmail);
            UserModel receiverUser = await db.GetUserByEmail(context.ReceiverEmail);

            if (senderUser == null || receiverUser == null)
            {
                return BadRequest("Sender or receiver not found.");
            }

            var invitation = new InvitationModel
            {
                SenderID = senderUser.ID,
                ReceiverID = receiverUser.ID,
                TenantId = context.TenantId
            };


            var (success, _) = await db.SaveInvitation(invitation);
            if (success)
            {

                receiverUser.Invitations.Add(invitation.ID);
                await db.UpdateUser(receiverUser);

                return Ok();
            }
            else
            {
                return BadRequest("Failed to send invitation.");
            }
        }
      
        [HttpPost, Route("DeclineInvitation")]
        public async Task<IActionResult> DeclineInvitation([FromBody] DeclineInvitationRequest request)
        {
            IDataAccess db = ServiceInit.GetDataInstance(_config, _dbContext);

            try
            {
                var (invitationFound, invitation) = await db.GetInvitationById(request.InvitationId);
                if (!invitationFound || invitation == null)
                {
                    return BadRequest("Invitation not found.");
                }


                _dbContext.Remove(invitation);
                await _dbContext.SaveChangesAsync();


                UserModel receiverUser = await db.GetUserById(invitation.ReceiverID);
                if (receiverUser != null)
                {
                    receiverUser.Invitations.Remove(invitation.ID);
                    await db.UpdateUser(receiverUser);
                }

                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while declining the invitation.");
            }
        }
        [HttpPost, Route("AcceptInvitation")]
        public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationRequest request)
        {
            IDataAccess db = ServiceInit.GetDataInstance(_config, _dbContext);

            try
            {
                var (invitationFound, invitation) = await db.GetInvitationById(request.InvitationId);
                if (!invitationFound || invitation == null)
                {
                    return BadRequest("Invitation not found.");
                }


                UserModel receiverUser = await db.GetUserById(invitation.ReceiverID);
                if (receiverUser != null)
                {
                    receiverUser.Tenants.Add(invitation.TenantId);
                    await db.UpdateUser(receiverUser);
                }


                _dbContext.Remove(invitation);
                await _dbContext.SaveChangesAsync();


                if (receiverUser != null)
                {
                    receiverUser.Invitations.Remove(invitation.ID);
                    await db.UpdateUser(receiverUser);
                }

                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while accepting the invitation.");
            }
        }
        [HttpGet, Route("GetInvitation")]
        public async Task<IActionResult> GetInvitation(int invitationId)
        {
            try
            {
                var invitation = await _dbContext.Invitations
                    .Where(i => i.ID == invitationId).FirstOrDefaultAsync();

                if (invitation == null)
                {
                    return BadRequest("Invitation not found.");
                }

                var sender = await _dbContext.Users.FirstOrDefaultAsync(u => u.ID == invitation.SenderID);

                var invitationDto = new
                {
                    ID = invitation.ID,
                    SenderID = invitation.SenderID,
                    SenderFirstName = sender?.FirstName,
                    SenderLastName = sender?.LastName,
                    ReceiverID = invitation.ReceiverID,
                    TenantID = invitation.TenantId
                };

                return Ok(invitationDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while getting the invitation.");
            }
        }
        [HttpGet, Route("GetInvitationsForReceiver")]
        public async Task<IActionResult> GetInvitationsForReceiver(string email)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var invitations = await _dbContext.Invitations
                    .Where(i => i.ReceiverID == user.ID)
                    .Select(invitation => new
                    {
                        ID = invitation.ID,
                        SenderID = invitation.SenderID,
                        ReceiverID = invitation.ReceiverID,
                        TenantID = invitation.TenantId,
                        SenderFirstName = _dbContext.Users.FirstOrDefault(u => u.ID == invitation.SenderID).FirstName,
                        SenderLastName = _dbContext.Users.FirstOrDefault(u => u.ID == invitation.SenderID).LastName,
                        TenantName = _dbContext.Tenants.FirstOrDefault(t => t.ID == invitation.TenantId).Name
                    })
                    .ToListAsync();

                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while getting the invitations.");
            }
        }
        [HttpGet, Route("GetSentInvitationsForSender")]
        public async Task<IActionResult> GetSentInvitationsForSender(string email, int tenantId)
        {
            try
            {
                var sender = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (sender == null)
                {
                    return NotFound("Sender not found.");
                }

                var invitations = await _dbContext.Invitations
                    .Where(i => i.SenderID == sender.ID && i.TenantId == tenantId)
                    .Select(invitation => new
                    {
                        id = invitation.ID,
                        senderID = sender.ID,
                        receiverID = invitation.ReceiverID,
                        tenantID = invitation.TenantId,
                        receiverFirstName = _dbContext.Users.FirstOrDefault(u => u.ID == invitation.ReceiverID).FirstName,
                        receiverLastName = _dbContext.Users.FirstOrDefault(u => u.ID == invitation.ReceiverID).LastName,
                        tenantName = _dbContext.Tenants.FirstOrDefault(t => t.ID == invitation.TenantId).Name
                    })
                    .ToListAsync();

                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while getting the sent invitations.");
            }
        }

    }
}
