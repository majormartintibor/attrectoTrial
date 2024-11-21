namespace Feed.Persistence.Entity;
public sealed class UserFeedLike
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid FeedId { get; set; }
    public Feed Feed { get; set; } = null!;
}