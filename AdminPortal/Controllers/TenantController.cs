using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Filters;
using Admin.Portal.API.Helpers;
using Admin.Portal.API.Interfaces;
using Admin.Portal.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [HttpGet, Route("Tenants")]
        public async Task<IActionResult> Tenants()
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, List<TenantModel>) result = await db.GetTenantDeatils();
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] TenantModel context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config, dbContext);
            (bool, TenantModel) result = await db.CreateTenant(context);
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
        }
    }
}
