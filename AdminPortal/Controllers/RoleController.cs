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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
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
        public async Task<IActionResult> Roles(int tenantId)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_READ_ROLE, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

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
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_CREATE_ROLE, Request.Headers[Config.HEADER_LOGIN_USER].ToString(), context.TenantID))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

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
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_UPDATE_ROLE, Request.Headers[Config.HEADER_LOGIN_USER].ToString(), context.TenantID))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

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
       
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_DELETE_ROLE, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            bool result = await db.DeleteRole(Id);
            if(result)
                return new Context(Messages.ROLE_DELETE_SUCCESS).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);

        }
    }


}

