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
    public class UserController : ControllerBase
    {
        private readonly Settings config;
        private readonly DataContext dbContext;
        public UserController(IOptions<Settings> _Settings, DataContext _dbContext)
        {
            config = _Settings.Value;
            dbContext = _dbContext;
        }

        [HttpGet, Route("User")]
        public async Task<IActionResult> User(string username)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_READ_USER, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, UserModel) result = await db.GetUserDetails(username);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpGet, Route("Users")] 
        public async Task<IActionResult> Users(int? tenantID)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_READ_USER, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, List<UserModel>) result = await db.GetUsers(tenantID);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<List<UserResponse>>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_CREATE_USER, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            UserModel usr = JsonConvert.DeserializeObject<UserModel>(JsonConvert.SerializeObject(context));
            (bool, UserModel) result = await db.CreateUser(usr);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPut,Route("Update")]
        public async Task<IActionResult> Update([FromBody] UserModel context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_UPDATE_USER, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, UserModel) result = await db.UpdateUser(context);
            if (result.Item1) 
                return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else 
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPut, Route("LinkTenant")]
        public async Task<IActionResult> LinkTenant([FromBody] UserTenantLinkRequest context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_UPDATE_USER, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, UserModel) result = await db.LinkTenantToUser(context);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPut, Route("UnLinkTenant")]
        public async Task<IActionResult> UnLinkTenant([FromBody] UserTenantLinkRequest context)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_UPDATE_USER, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, UserModel) result = await db.UnLinkTenantFromUser(context);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete,Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_DELETE_USER, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
                return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config,dbContext);
            bool result = await db.DeleteUser(id);
            if(result)
                return new Context(Messages.USER_DELETE_SUCCESS).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }
    }
}
