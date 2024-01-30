using FluentValidation;
using LiveScoreBoardLibrary.DataLayer;
using LiveScoreBoardLibrary.Exceptions;
using LiveScoreBoardLibrary.Models;
using LiveScoreBoardLibrary.Services.Interfaces;
using LiveScoreBoardLibrary.Validation;
using Match = LiveScoreBoardLibrary.Models.Match;

namespace LiveScoreBoardLibrary.Services;

public class ScoreBoardService : IScoreBoardService
{
    private readonly ScoreBoardStore _scoreBoard;

    public ScoreBoardService()
    {
        _scoreBoard = new ScoreBoardStore();

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

        var teamValidator = new TeamValidator(_scoreBoard.Matches);
        teamValidator.ValidateAndThrow(homeTeam);
        teamValidator.ValidateAndThrow(awayTeam);

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
        var updatedMatch = _scoreBoard.Matches.FirstOrDefault(match => match.Id == matchId);

        if (updatedMatch == null)
        {
            throw new MatchNotFoundException("Match with the specified identifier not found");
        }

        if (score < 0)
        {
            throw new InvalidScoreException("Score must be a non-negative number");
        }

        switch (teamType)
        {
            case TeamType.Away:
                updatedMatch.AwayTeam.Score = score;
                break;
            case TeamType.Home:
                updatedMatch.HomeTeam.Score = score;
                break;
        }
    }

    public void FinishMatch(Guid matchId)
    {
        var match = _scoreBoard.Matches.FirstOrDefault(match => match.Id == matchId);

        if (match == null)
        {
            throw new MatchNotFoundException("Match with the specified identifier not found");
        }

        _scoreBoard.Matches.Remove(match);
    }

    public void ClearBoard()
    {
        _scoreBoard.Matches.Clear();
    }
}