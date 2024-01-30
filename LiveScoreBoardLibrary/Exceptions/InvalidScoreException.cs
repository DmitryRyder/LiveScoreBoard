namespace LiveScoreBoardLibrary.Exceptions;

public class InvalidScoreException : Exception
{
    public InvalidScoreException() : base("Score must be a non-negative number")
    {
    }

    public InvalidScoreException(string message) : base(message)
    {
    }

    public InvalidScoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
