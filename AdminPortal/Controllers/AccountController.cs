using Admin.Portal.API.Core.Const;
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
