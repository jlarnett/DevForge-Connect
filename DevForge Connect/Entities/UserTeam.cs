using DevForge_Connect.Entities.Identity;

namespace DevForge_Connect.Entities
{
    public class UserTeam
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public int? TeamId { get; set; }

        //Navigation Properties
        public ApplicationUser? User { get; set; } = null!;
        public Team? Team { get; set; } = null!;
    }
}
