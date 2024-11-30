using DevForge_Connect.Data;
using Microsoft.EntityFrameworkCore;

namespace DevForge_Connect.Services.TeamBuilder
{
    public class TeamBuilder : ITeamBuilder
    {
        private readonly ApplicationDbContext _context;

        public TeamBuilder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTeamLeader(string userId, int teamId)
        {
            var userTeams = await _context.UserTeams.Where(ut => ut.TeamId.Equals(teamId)).OrderBy(ut => ut.Id).ToListAsync();
            if (!userTeams.Any()) return false;
            return userTeams.First().UserId == userId;
        }

    }
}
