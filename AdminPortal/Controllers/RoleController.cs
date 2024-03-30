using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Core.Response;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Admin.Portal.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace Admin.Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly Settings config;
        private readonly DataContext dbContext;
        public RoleController(IOptions<Settings> _Settings, DataContext _dbContext)
        {
            config = _Settings.Value;
            dbContext = _dbContext;
        }

        [HttpGet, Route("Roles")]
        public async Task<IActionResult> Roles(int? tenantId)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, List<RoleModel>) result = await db.GetRoles(tenantId);
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] RoleRequest context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            RoleModel newRole = JsonConvert.DeserializeObject<RoleModel>(JsonConvert.SerializeObject(context));
            (bool, RoleModel) result = await db.CreateRole(newRole);
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPut, Route("Update")]
        public async Task<IActionResult> Update([FromBody] RoleModel context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, RoleModel) result = await db.UpdateRole(JsonConvert.DeserializeObject<RoleModel>(JsonConvert.SerializeObject(context)));
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete,Route("Delete")]
        public async Task<IActionResult> Delete(int? Id)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            bool result = await db.DeleteRole(Id);
            if(result)
                return new Context(Messages.ROLE_DELETE_SUCCESS).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);

        }

        [HttpPut, Route("LinkClaim")]
        public async Task<IActionResult> LinkClaim([FromBody] RoleClaimLinkRequest context)
        {
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
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, RoleModel) result = await db.UnLinkClaimFromRole(context);
            if (result.Item1)
                return new Context((result.Item2)).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpGet, Route("Claims")]
        public async Task<IActionResult> GetAllClaims()
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            List<ClaimModel> result = await db.GetClaims();
            return new Context(JsonConvert.DeserializeObject<List<ClaimResponse>>(JsonConvert.SerializeObject(result))).ToContextResult();
        }
    }


}

