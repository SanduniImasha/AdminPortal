namespace Admin.Portal.API.Core.Response
{
    public class InvitationResponse
    {
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public int TenantId { get; set; }
    }
}
