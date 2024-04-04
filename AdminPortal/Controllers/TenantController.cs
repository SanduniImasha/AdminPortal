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
    public class TenantController : ControllerBase
    {
        private readonly Settings config;
        private readonly DataContext dbContext;
        public TenantController(IOptions<Settings> _Settings, DataContext _dbContext)
        {
            config = _Settings.Value;
            dbContext = _dbContext;
        }

        //ToDo: Tenant --> ID Request

        [HttpGet, Route("Tenants")]
        public async Task<IActionResult> Tenants(int? userID)
        {
            if(!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_READ_TENANT, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, List<TenantModel>) result = await db.GetTenantDeatils(userID);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<List<TenantModel>>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }
        [HttpGet, Route("Tenant")]
        public async Task<IActionResult> GetTenant(int tenantId)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_READ_TENANT, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, TenantModel) result = await db.GetOneTenant(tenantId);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<TenantModel>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] TenantRequest context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_CREATE_TENANT, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, TenantModel) result = await db.CreateTenant(new TenantModel() { Name = context.Name});
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<TenantModel>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPut, Route("Update")]
        public async Task<IActionResult> Update([FromBody] TenantModel context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_UPDATE_TENANT, Request.Headers[Config.HEADER_LOGIN_USER].ToString(), context.ID))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, TenantModel) result = await db.UpdateTenant(JsonConvert.DeserializeObject<TenantModel>(JsonConvert.SerializeObject(context)));
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<TenantModel>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete, Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.TenantnClaims, Claims.CLAIM_DELETE_TENANT, Request.Headers[Config.HEADER_LOGIN_USER].ToString(), id))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            bool result = await db.DeleteTenant(id);
            if (result)
                return new Context(Messages.TENANT_DELETE_SUCCESS).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

     // [HttpPut, Route("LinkRole")]
     //   public async Task<IActionResult> LiknRole([FromBody] TenantRoleRequest context) 
     //   {
     //     //  if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_READ_TENANT, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
     //       //    return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

     //       //IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
     //       //(bool, TenantModel) result = await db.LinkRoleToTenant(context);
     //      if (result.Item1)
     //          return new Context((result.Item2)).ToContextResult();
     //       else
     //           return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
     //   }

        //[HttpPut, Route("UnlinkRole")]
        //public async Task<IActionResult> UnLiknRole([FromBody] TenantRoleRequest context)
        //{
        //    if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Admins, Claims.CLAIM_READ_TENANT, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
        //        return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

        //    IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
        //    (bool, TenantModel) result = await db.UnLinkRoleFromTenant(context);
        //    if (result.Item1)
        //        return new Context((result.Item2)).ToContextResult();
        //    else
        //        return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        //}
      
    }
}
