using Admin.Portal.API.Core.Const;
using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
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
    public class TenantController : ControllerBase
    {
        private readonly Settings config;
        public TenantController(IOptions<Settings> _Settings)
        {
            config = _Settings.Value;
        }

        [HttpGet, Route("Tenants")]
        public async Task<IActionResult> Tenants()
        {
            IDataAccess db = ServiceInit.GetDataInstance(config);
            (bool, List<TenantModel>) result = await db.GetTenantDeatils();
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);

            //Debug
            //List<TenantModel> lstTenants =
            //[
            //    new TenantModel{ ID= "1", Name = "ABC Limited"},
            //    new TenantModel{ ID= "2", Name = "POC Limited"},
            //    new TenantModel{ ID= "3", Name = "DLF Limited"}
            //];

            //return new Context(lstTenants).ToContextResult();
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] TenantModel context)
        {
            IDataAccess db = ServiceInit.GetDataInstance(config);
            (bool, TenantModel) result = await db.CreateTenant(context);
            if (result.Item1)
                return new Context(result.Item2).ToContextResult();
            else
                return new Context(Messages.DATA_ACCESS_FAILER).ToContextResult((int)HttpStatusCode.BadRequest);
            
            //Debug
            //return new Context(context).ToContextResult();
        }
    }
}
