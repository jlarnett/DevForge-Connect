using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace DevForge_Connect.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        //Navigation Properties
        public List<Team> Teams { get; } = [];
        public List<UserTeam> UserTeams { get; } = [];
        public List<TeamInvite> TeamInvites { get; set; }
    }
}
