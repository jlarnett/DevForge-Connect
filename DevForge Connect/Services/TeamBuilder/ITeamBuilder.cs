namespace DevForge_Connect.Services.TeamBuilder
{
    public interface ITeamBuilder
    {
        public Task<bool> IsTeamLeader(string userId, int teamId);
    }
}
