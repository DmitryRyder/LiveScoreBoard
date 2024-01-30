using LiveScoreBoardLibrary.Models;

namespace LiveScoreBoardLibrary.Services.Interfaces;

public interface IScoreBoardService
{
    void FinishMatch(Guid matchId);

    IEnumerable<Match> GetMatches();

    void StartNewMatch(Team homeTeam, Team awayTeam, DateTime? startTime);

    void UpdateMatchScore(Guid matchId, TeamType teamType, int score);

    void ClearBoard();
}