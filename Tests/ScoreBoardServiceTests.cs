﻿using FluentAssertions;
using FluentValidation;
using LiveScoreBoardLibrary.Services.Interfaces;


[TestFixture]
public class ScoreBoardServiceTests
{
    private IScoreBoardService _scoreboardService;

    [SetUp]
    public void SetUp()
    {
        _scoreboardService = new ScoreBoardService();
    }

    [TearDown]
    public void TearDown()
    {
        _scoreboardService.ClearBoard();
    }

    [Test]
    public void GetMatches_ReturnsEmptyListInitially()
    {
        // Arrange

        // Act
        var matches = _scoreboardService.GetMatches();

        // Assert
        Assert.That(matches, Is.Empty);
    }

    [TestCaseSource(nameof(TestCases))]
    public void GetMatches_SortedByTotalScoreAndStartedDate_ReturnsCorrectOrder(List<(Team, Team, DateTime)> input, List<(Team, Team, DateTime)> expected)
    {
        // Arrange
        foreach (var match in input)
        {
            _scoreboardService.StartNewMatch(match.Item2, match.Item1, match.Item3);
        }

        // Act
        var result = _scoreboardService.GetMatches().ToList();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(input.Count));

        for (int i = 0; i < input.Count; i++)
        {
            Assert.That(result[i].GetTotalScore(), Is.EqualTo(expected[i].Item1.Score + expected[i].Item2.Score));
            Assert.That(result[i].StartDate, Is.EqualTo(expected[i].Item3));
        }
    }

    [Test]
    public void StartNewMatch_WithUniqueTeamNames_ShouldAddToScoreBoard()
    {
        // Arrange
        var homeTeam = new Team { Name = "TeamA", Score = 10, TeamType = TeamType.Home };
        var awayTeam = new Team { Name = "TeamB", Score = 5, TeamType = TeamType.Away };
        var startTime = DateTime.UtcNow;

        // Act
        _scoreboardService.StartNewMatch(homeTeam, awayTeam, startTime);

        // Assert
        Assert.That(_scoreboardService.GetMatches().ToList(), Has.Count.EqualTo(1));
    }

    [Test]
    public void StartNewMatch_WithSameTeamNames_ShouldThrowValidationException()
    {
        // Arrange
        var homeTeam = new Team { Name = "TeamA", Score = 10, TeamType = TeamType.Home };
        var awayTeam = new Team { Name = "TeamA", Score = 5, TeamType = TeamType.Away };
        var startTime = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ValidationException>(() => _scoreboardService.StartNewMatch(homeTeam, awayTeam, startTime));
    }

    [Test]
    public void UpdateMatchScore_WithInvalidMatchId_ShouldThrowException()
    {
        // Arrange
        var invalidMatchId = Guid.NewGuid();

        // Act & Assert
        Action updateAction = () => _scoreboardService.UpdateMatchScore(invalidMatchId, TeamType.Home, 15);
        updateAction.Should().Throw<ValidationException>().WithMessage("Match with the specified identifier not found");
    }

    [Test]
    public void UpdateMatchScore_WithNegativeScore_ShouldThrowException()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = new Match
        {
            MatchId = matchId,
            HomeTeam = new Team { Name = "TeamA", Score = 10, TeamType = TeamType.Home },
            AwayTeam = new Team { Name = "TeamB", Score = 5, TeamType = TeamType.Away },
            StartDate = DateTime.UtcNow
        };
        _scoreboardService.StartNewMatch(match.HomeTeam, match.AwayTeam, match.StartDate);

        // Act & Assert
        Action updateAction = () => _scoreboardService.UpdateMatchScore(matchId, TeamType.Home, -5);
        updateAction.Should().Throw<ValidationException>().WithMessage("Score must be a non-negative number");
    }

    [Test]
    public void UpdateMatchScore_WithValidDataForAwayTeam_ShouldUpdateAwayTeamScore()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = new Match
        {
            MatchId = matchId,
            HomeTeam = new Team { Name = "TeamA", Score = 10, TeamType = TeamType.Home },
            AwayTeam = new Team { Name = "TeamB", Score = 5, TeamType = TeamType.Away },
            StartDate = DateTime.UtcNow
        };
        _scoreboardService.StartNewMatch(match.HomeTeam, match.AwayTeam, match.StartDate);

        // Act
        _scoreboardService.UpdateMatchScore(matchId, TeamType.Away, 15);

        // Assert
        var updatedMatch = _scoreboardService.GetMatches().FirstOrDefault(m => m.MatchId == matchId);
        updatedMatch.Should().NotBeNull();
        updatedMatch?.AwayTeam.Score.Should().Be(15);
    }

    [Test]
    public void UpdateMatchScore_WithInvalidScore_ShouldThrowException()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = new Match
        {
            MatchId = matchId,
            HomeTeam = new Team { Name = "TeamA", Score = 10, TeamType = TeamType.Home },
            AwayTeam = new Team { Name = "TeamB", Score = 5, TeamType = TeamType.Away },
            StartDate = DateTime.UtcNow
        };
        _scoreboardService.StartNewMatch(match.HomeTeam, match.AwayTeam, match.StartDate);

        // Act & Assert
        Action updateAction = () => _scoreboardService.UpdateMatchScore(matchId, TeamType.Home, -1);
        updateAction.Should().Throw<ValidationException>().WithMessage("Score must be a non-negative number");
    }

    [Test]
    public void UpdateMatchScore_WithInvalidTeamType_ShouldThrowException()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = new Match
        {
            MatchId = matchId,
            HomeTeam = new Team { Name = "TeamA", Score = 10, TeamType = TeamType.Home },
            AwayTeam = new Team { Name = "TeamB", Score = 5, TeamType = TeamType.Away },
            StartDate = DateTime.UtcNow
        };
        _scoreboardService.StartNewMatch(match.HomeTeam, match.AwayTeam, match.StartDate);

        // Act & Assert
        Action updateAction = () => _scoreboardService.UpdateMatchScore(matchId, (TeamType)99, 15);
        updateAction.Should().Throw<ValidationException>().WithMessage("Invalid team type");
    }

    [Test]
    public void FinishMatch_WithValidData_ShouldRemoveMatchFromList()
    {
        // Arrange
        var match = new Match
        {
            HomeTeam = new Team { Name = "TeamA", Score = 10, TeamType = TeamType.Home },
            AwayTeam = new Team { Name = "TeamB", Score = 5, TeamType = TeamType.Away },
            StartDate = DateTime.UtcNow
        };
        _scoreboardService.StartNewMatch(match.HomeTeam, match.AwayTeam, match.StartDate);

        // Act
        var matchesBeforeFinish = _scoreboardService.GetMatches().ToList();
        _scoreboardService.FinishMatch(matchesBeforeFinish.First().MatchId);

        // Assert
        var matchesAfterFinish = _scoreboardService.GetMatches().ToList();
        matchesAfterFinish.Should().NotContain(m => m.MatchId == matchesBeforeFinish.First().MatchId);
    }

    [Test]
    public void FinishMatch_WithNonexistentMatchId_ShouldThrowException()
    {
        // Arrange
        var invalidMatchId = Guid.NewGuid();

        // Act & Assert
        Action finishAction = () => _scoreboardService.FinishMatch(invalidMatchId);
        finishAction.Should().Throw<ValidationException>().WithMessage("Match with the specified identifier not found");
    }

    public static IEnumerable<TestCaseData> TestCases
    {
        get
        {
            var testDate = new DateTime(2024, 1, 30, 0, 0, 0);

            var match1 = (
                new Team { Name = "testAwayTeam1", Score = 0, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam1", Score = 0, TeamType = TeamType.Home },
                testDate.AddHours(1));

            var match2 = (
                new Team { Name = "testAwayTeam2", Score = 10, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam2", Score = 20, TeamType = TeamType.Home },
                testDate.AddHours(2));

            var match3 = (
                new Team { Name = "testAwayTeam3", Score = 20, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam3", Score = 10, TeamType = TeamType.Home },
                testDate.AddHours(3));

            var match4 = (
                new Team { Name = "testAwayTeam4", Score = 30, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam4", Score = 30, TeamType = TeamType.Home },
                testDate.AddHours(4)
            );

            var match5 = (
                new Team { Name = "testAwayTeam5", Score = 40, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam5", Score = 30, TeamType = TeamType.Home },
                testDate.AddHours(5)
            );

            var match6 = (
                new Team { Name = "testAwayTeam6", Score = 30, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam6", Score = 40, TeamType = TeamType.Home },
                testDate.AddHours(6)
            );

            var match7 = (
                new Team { Name = "testAwayTeam7", Score = 50, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam7", Score = 50, TeamType = TeamType.Home },
                testDate.AddHours(7)
            );

            var match8 = (
                new Team { Name = "testAwayTeam8", Score = 60, TeamType = TeamType.Away },
                new Team { Name = "testHomeTeam8", Score = 40, TeamType = TeamType.Home },
                testDate.AddHours(8)
            );

            yield return new TestCaseData(new List<(Team, Team, DateTime)> { match1, match2, match3, match4, match5, match6, match7, match8 }, new List<(Team, Team, DateTime)> { match8, match7, match6, match5, match4, match3, match2, match1 });
            yield return new TestCaseData(new List<(Team, Team, DateTime)> { match8, match7, match6, match5, match4, match3, match2, match1 }, new List<(Team, Team, DateTime)> { match8, match7, match6, match5, match4, match3, match2, match1 });
            yield return new TestCaseData(new List<(Team, Team, DateTime)> { match4, match3, match2, match1, match8, match7, match6, match5 }, new List<(Team, Team, DateTime)> { match8, match7, match6, match5, match4, match3, match2, match1 });
            yield return new TestCaseData(new List<(Team, Team, DateTime)> { match1, match3, match5, match7, match2, match4, match6, match8 }, new List<(Team, Team, DateTime)> { match8, match7, match6, match5, match4, match3, match2, match1 });
            yield return new TestCaseData(new List<(Team, Team, DateTime)> { match8, match6, match4, match2, match7, match5, match3, match1 }, new List<(Team, Team, DateTime)> { match8, match7, match6, match5, match4, match3, match2, match1 });
        }
    }
}