namespace Admin.Portal.API.Core.Const
{
    public class Messages
    {
        public const string DATA_ACCESS_FAILER = "Could not connect to data source right now. Please try again later.";

        public const string ACCOUNT_LOGIN_FAILER = "Incorect Username or Password! Please try again.";
        public const string ACCOUNT_INSUFFICIENT_PRIVILEGES = "Insufficient Privileges";

        public const string USER_DELETE_SUCCESS = "User Deleted Successfully";
        public const string ROLE_DELETE_SUCCESS = "Role Deleted Successfully";
        public const string TENANT_DELETE_SUCCESS = "Tenant Deleted Successfully";


        public const string ERROR_TENANT_EXSIST = "Tenant name alreday exsist";
        public const string ERROR_USER_EXSIST = "User alreday exsist";
        public const string ERROR_USER_DOES_NOT_EXSIST = "User does not exsist";
        public const string ERROR_USER_TENANT_EXSIST = "User already link to tenant";
        public const string ERROR_USER_TENANT_DOES_NOT_EXSIST = "User does not link to this tenant";
        public const string ERROR_TENANT_DOES_NOT_EXSIST = "Tenant does not exsist";

        public const string ERROR_ROLE_EXSIST = "Role already exsist";
        public const string ERROR_ROLE_DOES_NOT_EXSIST = "Role does not exsist";
        public const string ERROR_USERS_ALREADY_LINKED = "Other users are already linked to this tenant, Please try again";
        public const string ERROR_ROLES_ALREADY_LINKED = "Roles are already linked to this tenant, Please try again";

    }
}
