using DevForge_Connect.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevForge_Connect.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required] public string? UserId { get; set; }
        public string? Bio { get; set; }
        public string? Expirience { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? NlpTags { get; set; } = string.Empty;

        //Navigation Properties
        public ApplicationUser? User { get; set; } = null!;
    }
}
