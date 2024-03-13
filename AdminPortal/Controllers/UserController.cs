using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Infrastructure.Interfaces;
using Admin.Portal.API.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Admin.Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Settings config;
        public UserController(IOptions<Settings> _Settings)
        {
            config = _Settings.Value;
        }

        [HttpGet, Route("Users")]
        public async Task<IActionResult> Users(string? tenantID)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config);
            (bool, List<UserModel>) result = await db.GetUserDeatils(tenantID);
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);

            //Debug
            //List<UserModel> lstUsers =
            //[
            //    new UserModel { ID = "1", FirstName = "Adam", LastName = "Y", Email = "Adam@gmail.com", Tenenats = ["1"] },
            //    new UserModel { ID = "2", FirstName = "Adrian", LastName = "Y", Email = "Adrian@gmail.com", Tenenats = ["1", "2"] },

            //    new UserModel { ID = "3", FirstName = "Alan", LastName = "Y", Email = "Alan@gmail.com", Tenenats = ["3"] },
            //];

            //return new Context(lstUsers).ToContextResult();
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] UserModel context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config);
            (bool, UserModel) result = await db.CreateUser(context);
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);

            //Debug
            //return new Context(context).ToContextResult();
        }
    }
}
