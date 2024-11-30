using DevForge_Connect.Entities.Identity;
using DevForge_Connect.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevForge_Connect.Models
{
    public class TeamDetailsVm
    {
        public int Id { get; set; }
        [DisplayName("Team Name")]
        [Required] public string? Name { get; set; } = null;

        //Navigation Properties
        public List<UserTeam> UserTeams { get; set; } = [];
        public List<ApplicationUser> Users { get; set; } = [];

        public List<ApplicationUser> fullUserList { get; set; } = new List<ApplicationUser>();
        public List<TeamInvite> pendingInvites { get; set; } = new List<TeamInvite>();
    }
}
