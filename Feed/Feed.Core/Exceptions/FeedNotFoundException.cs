namespace Feed.Core.Exceptions;
public sealed class FeedNotFoundException : Exception
{
    public FeedNotFoundException(string message) : base(message)
    {
    }
}