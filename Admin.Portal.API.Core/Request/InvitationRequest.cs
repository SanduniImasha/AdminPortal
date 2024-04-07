namespace Admin.Portal.API.Core.Request
{
    public class InvitationRequest
    {
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public int TenantId { get; set; }
    }
}

