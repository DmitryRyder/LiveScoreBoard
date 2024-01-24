namespace LiveScoreBoardLibrary.Models;

public class Match
{
    public Guid MatchId { get; set; }

    public Team HomeTeam { get; set; }

    public Team AwayTeam { get; set; }

    public MatchStatus Status { get; set; }

    public DateTime StartedDate { get; set; }

    public int GetTotalScore() => HomeTeam.Score + AwayTeam.Score;
}