using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using DevForge_Connect.Entities.Identity;

namespace DevForge_Connect.Entities
{
    public class ProjectBid
    {
        public int Id { get; set; }

        [Column(TypeName = "money")]
        public decimal OfferAmount { get; set; }
        public DateTime FinishDate { get; set; }

        [Required]public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string ProposalDescription { get; set; }

        public int? StatusId { get; set; }
        public Status? Status { get; set; }

        public int? ProjectId { get; set; }

        public ProjectSubmission? Project { get; set; } = null;

        public int? TeamId { get; set; }
        public Team? Team { get; set; }
    }
}
