using DevForge_Connect.Entities.Identity;

namespace DevForge_Connect.Entities
{
    public class TeamInvite
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        public Team InvitingTeam { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser RecipientUser { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
