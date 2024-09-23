using System.ComponentModel;
using DevForge_Connect.Entities.Identity;
using Microsoft.Build.Framework;

namespace DevForge_Connect.Entities
{
    public class Team
    {
        public int Id { get; set; }
        [DisplayName("Team Name")]
        [Required] public string? Name { get; set; } = null;

        //Navigation Properties
        public List<UserTeam> UserTeams { get; } = [];
        public List<ApplicationUser> Users { get; } = [];
    }
}
