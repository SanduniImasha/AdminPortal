using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Core.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Admin.Portal.API.Interfaces;
using Admin.Portal.API.Core.Request;

namespace Admin.Portal.API.Services
{
    public class JsonTextDB 
    {
        private protected DB Config { get; set; }
        public JsonTextDB(DB config)
        {
            Config = config;
        }

        public async Task<bool> Validate(LoginRequest context)
        {
            string readText = File.ReadAllText(Config.File, Encoding.UTF8);

            if (readText.Length > 2)
            {
                JObject obj = JObject.Parse(readText);
                if (obj.Count > 0)
                {
                    JsonDBStructure db = obj.ToObject<JsonDBStructure>();

                    if (db.Admin.Where(u => u.Username.ToString().Equals(context.Username, StringComparison.InvariantCultureIgnoreCase) && u.Password.ToString().Equals(context.Password)).Count() == 1)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        public async Task<(bool, UserModel)> GetLoginUserDetails(string username)
        {
            string readText = File.ReadAllText(Config.File, Encoding.UTF8);

            if (readText.Length > 2)
            {
                JObject obj = JObject.Parse(readText);
                if (obj.Count > 0)
                {
                    JsonDBStructure db = obj.ToObject<JsonDBStructure>();
                    UserModel pro = new();
                    pro = JsonConvert.DeserializeObject<UserModel>(JsonConvert.SerializeObject(db.Admin.Where(u => u.Username.ToString().Equals(username, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()));
                    return (true, pro);
                }
            }

            return (false, new UserModel());
        }

        public async Task<(bool, List<TenantModel>)> GetTenantDeatils()
        {
            string readText = File.ReadAllText(Config.File, Encoding.UTF8);

            if (readText.Length > 2)
            {
                List<TenantModel> lstTenants = [];

                JObject obj = JObject.Parse(readText);
                if (obj.Count > 0)
                {
                    JsonDBStructure db = obj.ToObject<JsonDBStructure>();
                    lstTenants = db.Tenetants;
                }

                return (true, lstTenants);
            }

            return (false, new List<TenantModel>());
        }

        public async Task<(bool, List<UserModel>)> GetUserDeatils(string? tenantID)
        {
            string readText = File.ReadAllText(Config.File, Encoding.UTF8);

            if (readText.Length > 2)
            {
                List<UserModel> lstUsers = [];

                JObject obj = JObject.Parse(readText);
                if (obj.Count > 0)
                {
                    JsonDBStructure db = obj.ToObject<JsonDBStructure>();
                    if (!String.IsNullOrEmpty(tenantID) && tenantID.Length > 0)
                        lstUsers = JsonConvert.DeserializeObject<List<UserModel>>(JsonConvert.SerializeObject(db.Users.Where(u => u.Tenants.Any(t => t.Equals(tenantID)))));
                    else
                        lstUsers = db.Users;
                }

                return (true, lstUsers);
            }

            return (false, new List<UserModel>());
        }

        public async Task<(bool, TenantModel)> CreateTenant(TenantModel context)
        {
            string readText = File.ReadAllText(Config.File, Encoding.UTF8);

            if (readText.Length > 2)
            {
                JObject obj = JObject.Parse(readText);
                if (obj.Count > 0)
                {
                    JsonDBStructure db = obj.ToObject<JsonDBStructure>();
                    context.ID = "T" + Guid.NewGuid().ToString().Replace("-", "");
                    db.Tenetants.Add(context);
                    File.WriteAllText(Config.File, JsonConvert.SerializeObject(db));
                }

                return (true, context);
            }

            return (false, new TenantModel());
        }

        public async Task<(bool, UserModel)> CreateUser(UserModel context)
        {
            string readText = File.ReadAllText(Config.File, Encoding.UTF8);

            if (readText.Length > 2)
            {
                JObject obj = JObject.Parse(readText);
                if (obj.Count > 0)
                {
                    JsonDBStructure db = obj.ToObject<JsonDBStructure>();
                    db.Users.Add(context);
                    File.WriteAllText(Config.File, JsonConvert.SerializeObject(db));
                }

                return (true, context);
            }

            return (false, new UserModel());
        }
    }
}
