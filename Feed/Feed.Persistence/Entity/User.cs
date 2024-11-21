namespace Feed.Persistence.Entity;
public sealed class User
{
    public User()
    {
        UserFeedLikes = [];
    }

    public Guid Id { get; set; }
    public ICollection<UserFeedLike> UserFeedLikes { get; set; }
}
