using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Enum;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Core.Response;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Admin.Portal.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace Admin.Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly Settings config;
        private readonly DataContext dbContext;
        public ClaimController(IOptions<Settings> _Settings, DataContext _dbContext)
        {
            config = _Settings.Value;
            dbContext = _dbContext;
        }


        [HttpGet, Route("Claims")]
        public async Task<IActionResult> GetAllClaims()
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_READ_CLAIM, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            List<ClaimModel> result = await db.GetClaims();
            return new Context(JsonConvert.DeserializeObject<List<ClaimResponse>>(JsonConvert.SerializeObject(result))).ToContextResult();
        }

        [HttpPut, Route("LinkClaim")]
        public async Task<IActionResult> LinkClaim([FromBody] RoleClaimLinkRequest context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_UPDATE_ROLE, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, RoleModel) result = await db.LinkClaimToRole(context);
            if (result.Item1)
                return new Context((result.Item2)).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPut, Route("UnLinkClaim")]
        public async Task<IActionResult> UnLinkClaim([FromBody] RoleClaimLinkRequest context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_UPDATE_ROLE, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, RoleModel) result = await db.UnLinkClaimFromRole(context);
            if (result.Item1)
                return new Context((result.Item2)).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        
    }
}
