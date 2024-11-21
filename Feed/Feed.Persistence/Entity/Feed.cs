using Feed.Core.FeedDomain;

namespace Feed.Persistence.Entity;
public sealed class Feed
{
    public Feed()
    {
        UserFeedLikes = [];
    }

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public FeedType FeedType { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<UserFeedLike> UserFeedLikes { get; set; }
}