using Admin.Portal.API.Core.Models;
using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Request;
using Admin.Portal.API.Infrastructure.Interfaces;

namespace Admin.Portal.API.Infrastructure.Services
{
    public class SQLLite 
    {
        private protected DB Config { get; set; }
        public SQLLite(DB config)
        {
            Config = config;
        }

        public async Task<bool> Validate(LoginRequest context)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, ProfileModel)> GetLoginUserDetails(string username)
        {
            throw new NotImplementedException();
        }
    }
}
