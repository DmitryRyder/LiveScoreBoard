using LiveScoreBoardLibrary.Models;
using LiveScoreBoardLibrary.Services.Interfaces;
using Match = LiveScoreBoardLibrary.Models.Match;

namespace LiveScoreBoardLibrary.Services;

public class ScoreBoardService : IScoreBoardService
{
    private readonly ScoreBoard _scoreBoard;

    public ScoreBoardService()
    {
        _scoreBoard = new ScoreBoard();

    }

    public IEnumerable<Match> GetMatches()
    {
        return _scoreBoard.Matches
                          .OrderBy(m => m)
                          .ToList();
    }

    public void StartNewMatch(Team homeTeam, Team awayTeam, DateTime? startTime)
    {
        if (startTime is null || startTime == DateTime.MinValue)
        {
            startTime = DateTime.UtcNow;
        }

        var match = new Match
        {
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            StartDate = startTime.Value,
        };

        _scoreBoard.Matches.Add(match);
    }

    public void UpdateMatchScore(Guid matchId, TeamType teamType, int score)
    {
        throw new NotImplementedException();
    }

    public void FinishMatch(Guid matchId)
    {
        throw new NotImplementedException();
    }

    public void ClearBoard()
    {
        _scoreBoard.Matches.Clear();
    }
}