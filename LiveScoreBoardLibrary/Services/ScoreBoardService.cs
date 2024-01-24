using LiveScoreBoardLibrary.Models;
using LiveScoreBoardLibrary.Services.Interfaces;

namespace LiveScoreBoardLibrary.Services;

public class ScoreBoardService : IScoreBoardService
{
    private readonly ScoreBoard scoreBoard;

    public ScoreBoardService()
    {
        scoreBoard = new ScoreBoard();
    }

    public IEnumerable<Match> GetMatches()
    {
        throw new NotImplementedException();
    }

    public void StartNewMatch(Match newMatch)
    {
        throw new NotImplementedException();
    }

    public void UpdateMatchScore(Guid matchId, TeamType teamType, int score)
    {
        throw new NotImplementedException();
    }

    public void FinsihMatch(Guid matchId)
    {
        throw new NotImplementedException();
    }
}