using LiveScoreBoardLibrary.Models;

namespace LiveScoreBoardLibrary.DataLayer;

internal class ScoreBoardStore
{
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}