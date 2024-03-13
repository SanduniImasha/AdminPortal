
namespace Admin.Portal.API.Core.Models.Base
{
    public class JsonDBStructure
    {
        public List<Admin> Admin { get; set; }
        public List<TenantModel> Tenetants { get; set; }
        public List<UserModel> Users { get; set; }

    }
    public class Admin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
    }
}
