using Feed.Core.FeedDomain;
using static Feed.Core.FeedDomain.Feed;
using System.ComponentModel;

namespace Feed.Persistence.Mappers;
internal static class FeedMappers
{
    internal static Core.FeedDomain.Feed MapFromPersistenceModel(Entity.Feed feed)
    {
        return CreateFeed(
                feed.Id,
                feed.UserId,
                feed.Title,
                feed.Description,
                feed.FeedType,
                feed.UserFeedLikes.Count,
                feed.ImageUrl,
                feed.VideoUrl
            );
    }

    internal static Entity.Feed MapFromDomainModel(Core.FeedDomain.Feed feed)
    {
        return feed.FeedType switch
        {
            FeedType.Text => MapToTextFeed(feed),
            FeedType.Image => MapToImageFeed(feed),
            FeedType.Video => MapToVideoFeed(feed),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    internal static void MapFromDomainModel(Core.FeedDomain.Feed feed, Entity.Feed entity)
    {
        entity.Title = feed.Title;
        entity.Description = feed.Description;
        entity.FeedType = feed.FeedType;

        //Pattern matching is probably my top favorite "new" C# feature :)
        (entity.ImageUrl, entity.VideoUrl) = feed switch
        {
            ImageFeed imageFeed => (imageFeed.ImageUrl, string.Empty),
            VideoFeed videoFeed => (videoFeed.ImageUrl, videoFeed.VideoUrl),
            _ => (string.Empty, string.Empty)
        };
    }

    private static Entity.Feed MapToTextFeed(Core.FeedDomain.Feed feed)
    {
        var textFeed = feed as TextFeed;
        return new Entity.Feed()
        {
            Id = textFeed!.Id,
            UserId = textFeed.UserId,
            Title = textFeed.Title,
            Description = textFeed.Description,
            FeedType = FeedType.Text
        };
    }

    private static Entity.Feed MapToImageFeed(Core.FeedDomain.Feed feed)
    {
        var imageFeed = feed as ImageFeed;

        return new Entity.Feed()
        {
            Id = imageFeed!.Id,
            UserId = imageFeed.UserId,
            Title = imageFeed.Title,
            Description = imageFeed.Description,
            FeedType = FeedType.Image,
            ImageUrl = imageFeed.ImageUrl
        };
    }

    private static Entity.Feed MapToVideoFeed(Core.FeedDomain.Feed feed)
    {
        var videoFeed = feed as VideoFeed;

        return new Entity.Feed()
        {
            Id = videoFeed!.Id,
            UserId = videoFeed.UserId,
            Title = videoFeed.Title,
            Description = videoFeed.Description,
            FeedType = FeedType.Video,
            ImageUrl = videoFeed.ImageUrl,
            VideoUrl = videoFeed.VideoUrl
        };
    }
}
