using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace DevForge_Connect.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Navigation Properties
        public List<Team> Teams { get; } = [];
        public List<UserTeam> UserTeams { get; } = [];
        public List<TeamInvite> TeamInvites { get; set; }
        public List<ProjectSubmission> ProjectSubmissions { get; set; }
        public List<ProjectBid> ProjectBids { get; set; }

    }
}
