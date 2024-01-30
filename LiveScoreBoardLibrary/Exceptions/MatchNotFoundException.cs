namespace LiveScoreBoardLibrary.Exceptions;

public class MatchNotFoundException : Exception
{
    public MatchNotFoundException() : base("Match with the specified identifier not found")
    {
    }

    public MatchNotFoundException(string message) : base(message)
    {
    }

    public MatchNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}