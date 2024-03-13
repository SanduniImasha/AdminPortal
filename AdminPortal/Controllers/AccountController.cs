using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Infrastructure.Interfaces;
using Admin.Portal.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Admin.Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Settings config;
        public AccountController(IOptions<Settings> _Settings)
        {
            config = _Settings.Value;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest context)
        {
            //Debug Mode
            //if(context.Username.Equals("admin@gmail.com", StringComparison.InvariantCultureIgnoreCase) && context.Password.Equals("admin"))
            //    return new Context(new ProfileModel() { FirstName = "John",LastName = "Adam", Email="Admin@gmail.com", PhoneNumber = "" }).ToContextResult();
            //else
            //    return new Context(Messages.ACCOUNT_LOGIN_FAILER).ToContextResult((int)HttpStatusCode.Forbidden);

            IDataAccess db = ServiceInit.GetDataInstance(config);
            if (db.Validate(context).Result)
            {
                (bool, ProfileModel) result = await db.GetLoginUserDetails(context.Username);
                if (result.Item1)
                    return new Context(result.Item2).ToContextResult();
                else
                    return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
            }
            else
                return new Context(Messages.ACCOUNT_LOGIN_FAILER).ToContextResult((int)HttpStatusCode.Forbidden);
        }

    }
}
