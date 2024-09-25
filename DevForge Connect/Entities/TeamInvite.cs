using DevForge_Connect.Entities.Identity;
using Microsoft.Build.Framework;

namespace DevForge_Connect.Entities
{
    public class TeamInvite
    {
        public int Id { get; set; }

        [Required] public int TeamId { get; set; }
        public Team? InvitingTeam { get; set; }

        [Required] public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required] public int StatusId { get; set; }
        public Status? Status { get; set; }

        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
