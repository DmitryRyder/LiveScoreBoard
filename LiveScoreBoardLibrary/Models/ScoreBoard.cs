namespace LiveScoreBoardLibrary.Models;

public class ScoreBoard
{
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
