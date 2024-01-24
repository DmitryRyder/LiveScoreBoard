namespace LiveScoreBoardLibrary.Models;

public class Team
{
    public string Name { get; set; }

    public int Score { get; set; }

    public TeamType TeamType { get; set; }
}