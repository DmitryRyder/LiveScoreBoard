namespace LiveScoreBoardLibrary.Models;

public class Match : IComparable<Match>
{
    public Guid Id { get; set; }

    public Team HomeTeam { get; set; }

    public Team AwayTeam { get; set; }

    public DateTime StartDate { get; set; }

    public Match()
    {
        Id = Guid.NewGuid();
    }

    public int GetTotalScore() => HomeTeam.Score + AwayTeam.Score;

    public int CompareTo(Match other)
    {
        var scoreComparison = other.GetTotalScore().CompareTo(GetTotalScore());

        if (scoreComparison != 0)
        {
            return scoreComparison;
        }
        else
        {
            return other.StartDate.CompareTo(StartDate);
        }
    }
}