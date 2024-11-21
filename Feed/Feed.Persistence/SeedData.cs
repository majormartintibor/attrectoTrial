using Bogus;
using Feed.Core.FeedDomain;
using Feed.Persistence.Entity;

namespace Feed.Persistence;

//This approach allows me to use the exact same data to seed the database for local development
//and also widely use it in my integration tests, both to reset the database to the initially
//seeded state before each test run and also to reference the users and feeds in the integration tets.
public static class SeedData
{
    public static readonly Dictionary<string, Guid> UserGuids = new()
    {
        { "User1", Guid.Parse("9610605b-5689-4d54-af23-ef71807bd777") },
        { "User2", Guid.Parse("e0f22e7e-fc6e-4c17-840a-bac7026cbf7a") },
        { "User3", Guid.Parse("229ac9de-0763-4914-b292-ca25e7d5f72c") }
    };

    public static readonly Dictionary<string, Guid> FeedGuids = new()
    {
        { "Feed1", Guid.Parse("674d0299-5b3d-4088-9680-2b52ed6fef01") },
        { "Feed2", Guid.Parse("3d7b849e-4cb9-437d-88ed-82042b456b65") },
        { "Feed3", Guid.Parse("85e14b7e-fff3-4a34-93df-c0b08129f7c5") },
        { "Feed4", Guid.Parse("fae8b2c8-ac8e-42e1-8208-bcc6227818d5") },
        { "Feed5", Guid.Parse("ab0f6749-c43b-4fc3-b1ac-8b19b4442e4b") },
        { "Feed6", Guid.Parse("42a9034a-827c-4d79-930e-a9a3873df283") },
        { "Feed7", Guid.Parse("34b7e73a-dc7d-4f73-a6a2-00d29c7c3653") },
        { "Feed8", Guid.Parse("833c4111-643b-4201-b293-2428209add3d") },
        { "Feed9", Guid.Parse("db8daea3-c308-4afb-bcd4-0d67695c10da") },
        { "Feed10", Guid.Parse("a6ccdb9c-86ef-40f1-b218-d78ebc9dfdfe") }
    };

    public static readonly Dictionary<string, (Guid UserId, Guid FeedId)> UserFeedLikeGuids = new()
    {
        { "Like1", (UserGuids["User1"], FeedGuids["Feed1"]) },
        { "Like2", (UserGuids["User1"], FeedGuids["Feed2"]) },
        { "Like3", (UserGuids["User2"], FeedGuids["Feed3"]) },
        { "Like4", (UserGuids["User2"], FeedGuids["Feed4"]) },
        { "Like5", (UserGuids["User3"], FeedGuids["Feed5"]) },
        { "Like6", (UserGuids["User3"], FeedGuids["Feed6"]) },
        { "Like7", (UserGuids["User1"], FeedGuids["Feed7"]) },
        { "Like8", (UserGuids["User2"], FeedGuids["Feed8"]) },
        { "Like9", (UserGuids["User3"], FeedGuids["Feed9"]) }
    };

    public static void Initialize(ApplicationDbContext context)
    {
        // Ensure the database is created
        context.Database.EnsureCreated();

        // Check if the database is already seeded
        if (context.Users.Any() || context.Feeds.Any() || context.UserFeedLikes.Any())
        {
            return; // DB has been seeded
        }

        // Seed Users
        var users = UserGuids.Select(kvp => new User { Id = kvp.Value }).ToList();
        context.Users.AddRange(users);

        // Create a Faker instance for generating feed data
        var feedFaker = new Faker<Entity.Feed>()
            .RuleFor(f => f.Title, f =>
            {
                var title = f.Lorem.Sentence(1, 5);
                return title.Length > 20 ? title.Substring(0, 20) : title;
            })
            .RuleFor(f => f.Description, f =>
            {
                var description = f.Lorem.Paragraph();
                return description.Length > 2000 ? description.Substring(0, 2000) : description;
            })
            .RuleFor(f => f.ImageUrl, f => f.Internet.Url())
            .RuleFor(f => f.VideoUrl, f => f.Internet.Url());

        // Seed Feeds
        var feeds = new List<Entity.Feed>
        {
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed1"]).RuleFor(f => f.UserId, UserGuids["User1"]).RuleFor(f => f.FeedType, FeedType.Text).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed2"]).RuleFor(f => f.UserId, UserGuids["User1"]).RuleFor(f => f.FeedType, FeedType.Image).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed3"]).RuleFor(f => f.UserId, UserGuids["User2"]).RuleFor(f => f.FeedType, FeedType.Video).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed4"]).RuleFor(f => f.UserId, UserGuids["User2"]).RuleFor(f => f.FeedType, FeedType.Text).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed5"]).RuleFor(f => f.UserId, UserGuids["User3"]).RuleFor(f => f.FeedType, FeedType.Image).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed6"]).RuleFor(f => f.UserId, UserGuids["User3"]).RuleFor(f => f.FeedType, FeedType.Video).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed7"]).RuleFor(f => f.UserId, UserGuids["User1"]).RuleFor(f => f.FeedType, FeedType.Text).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed8"]).RuleFor(f => f.UserId, UserGuids["User2"]).RuleFor(f => f.FeedType, FeedType.Image).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed9"]).RuleFor(f => f.UserId, UserGuids["User3"]).RuleFor(f => f.FeedType, FeedType.Video).Generate(),
            feedFaker.Clone().RuleFor(f => f.Id, FeedGuids["Feed10"]).RuleFor(f => f.UserId, UserGuids["User1"]).RuleFor(f => f.FeedType, FeedType.Text).Generate()
        };

        // Ensure ImageUrl and VideoUrl rules are met
        foreach (var feed in feeds)
        {
            if (feed.FeedType == FeedType.Image && string.IsNullOrEmpty(feed.ImageUrl))
            {
                feed.ImageUrl = new Faker().Internet.Url();
            }
            if (feed.FeedType == FeedType.Video)
            {
                if (string.IsNullOrEmpty(feed.ImageUrl))
                {
                    feed.ImageUrl = new Faker().Internet.Url();
                }
                if (string.IsNullOrEmpty(feed.VideoUrl))
                {
                    feed.VideoUrl = new Faker().Internet.Url();
                }
            }
        }
        context.Feeds.AddRange(feeds);

        // Seed UserFeedLikes
        var userFeedLikes = UserFeedLikeGuids.Select(kvp => new UserFeedLike { UserId = kvp.Value.UserId, FeedId = kvp.Value.FeedId }).ToList();
        context.UserFeedLikes.AddRange(userFeedLikes);        

        context.SaveChanges();
    }
}