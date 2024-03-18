namespace Admin.Portal.API.Core.Response
{
    public class UserResponse
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Tenants { get; set; }
    }
}
