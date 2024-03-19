using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
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

       /* [HttpGet, Route("User")]
        public async Task<IActionResult> Users(string? userID)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, List<UserModel>) result = await db.GetUserDeatils(tenantID);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<List<UserResponse>>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }*/

        [HttpGet, Route("Users")] 
        public async Task<IActionResult> Users(string? tenantID)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, List<UserModel>) result = await db.GetUserDeatils(tenantID);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<List<UserResponse>>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] UserModel context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, UserModel) result = await db.CreateUser(context);
            if (result.Item1)
                return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPut,Route("Update")]

        public async Task<IActionResult> Update([FromBody] UserResponse context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, UserModel) result = await db.UpdateUser(JsonConvert.DeserializeObject<UserModel>(JsonConvert.SerializeObject(context)));
            if (result.Item1) 
                return new Context(JsonConvert.DeserializeObject<UserResponse>(JsonConvert.SerializeObject(result.Item2))).ToContextResult();
            else 
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete,Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config,dbContext);
            bool result = await db.DeleteUser(id);
            if(result)
                return new Context(Messages.USER_DELETE_SUCCESS).ToContextResult((int)HttpStatusCode.BadRequest);
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }



    }
}
