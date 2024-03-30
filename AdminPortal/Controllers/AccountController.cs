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
    public class AccountController : ControllerBase
    {
        private readonly Settings config;
        private readonly DataContext dbContext;
        public AccountController(IOptions<Settings> _Settings, DataContext _dbContext)
        {
            config = _Settings.Value;
            dbContext = _dbContext;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest context)
        {
            //if (!new AuthenticateFilter(config, dbContext).Authenticate(AccessLevel.Claims, Claims.CLAIM_ALLOW_TO_LOGIN, Request.Headers[Config.HEADER_LOGIN_USER].ToString()))
            // return new Context(Messages.ACCOUNT_INSUFFICIENT_PRIVILEGES).ToContextResult((int)HttpStatusCode.Forbidden);

            if (context.Username.ToLower().Equals(config.SuperUser.Email.ToLower()) && context.Password.Equals(config.SuperUser.Password))
                return new Context(new UserResponse() { FirstName = config.SuperUser.FirstName, LastName = config.SuperUser.LastName, ID = config.SuperUser.ID, Email = config.SuperUser.Email }).ToContextResult();

            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            if (db.Validate(context).Result)
            {
                (bool, UserModel) result = await db.GetUserDetails(context.Username);
                if (result.Item1)
                    return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            }
            
            return new Context(Messages.ACCOUNT_LOGIN_FAILER).ToContextResult((int)HttpStatusCode.Forbidden);
        }

        
    }
}
