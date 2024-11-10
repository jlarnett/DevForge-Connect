using DevForge_Connect.Entities.Identity;
using Microsoft.Build.Framework;

namespace DevForge_Connect.Entities
{
    public class ProjectSubmission
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime? Deadline { get; set; }
        public string Funding { get; set; } = string.Empty;

        [Required] public string? creatorId { get; set; }
        public ApplicationUser? Creator { get; set; }

        public int? StatusId { get; set; }
        public Status? Status { get; set; }

        public ICollection<ProjectBid> Bids { get; set; } = new List<ProjectBid>();
		public string? NlpTags { get; internal set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }
	}
}
