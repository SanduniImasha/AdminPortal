using System.ComponentModel.DataAnnotations;

namespace Admin.Portal.API.Core.Models
{
    public class InvitationModel
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public int SenderID { get; set; }
        [Required]
        [EmailAddress]
        public int ReceiverID { get; set; }
        [Required]
        public int TenantId { get; set; }
    }
}
