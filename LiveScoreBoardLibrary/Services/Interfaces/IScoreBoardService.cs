using LiveScoreBoardLibrary.Models;

namespace LiveScoreBoardLibrary.Services.Interfaces;

public interface IScoreBoardService
{
    void FinsihMatch(Guid matchId);
    IEnumerable<Match> GetMatches();
    void StartNewMatch(Match newMatch);
    void UpdateMatchScore(Guid matchId, TeamType teamType, int score);
}