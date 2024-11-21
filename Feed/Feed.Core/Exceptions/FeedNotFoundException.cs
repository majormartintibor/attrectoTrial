namespace Feed.Core.Exceptions;
public sealed class FeedNotFoundException : Exception
{
    public FeedNotFoundException(Guid id)
        : base($"Feed with ID {id} not found.")
    {
    }
}