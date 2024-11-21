using Feed.Core.FeedDomain;
using static Feed.Core.FeedDomain.Feed;
using System.ComponentModel;

namespace Feed.API.FeedEndpoints;

public static class Mappers
{
    public static FeedDto MapFromDomainModel(Core.FeedDomain.Feed fetchedFeed)
    {
        return fetchedFeed.FeedType switch
        {
            FeedType.Text => MapFromTextFeed((TextFeed)fetchedFeed),
            FeedType.Image => MapFromImageFeed((ImageFeed)fetchedFeed),
            FeedType.Video => MapFromVideoFeed((VideoFeed)fetchedFeed),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    private static FeedDto MapFromTextFeed(TextFeed fetchedFeed)
    {
        return new FeedDto()
        {
            Id = fetchedFeed.Id,
            UserId = fetchedFeed.UserId,
            Title = fetchedFeed.Title,
            Description = fetchedFeed.Description,
            FeedType = fetchedFeed.FeedType,
            Likes = fetchedFeed.Likes,
        };
    }

    private static FeedDto MapFromImageFeed(ImageFeed fetchedFeed)
    {
        return new FeedDto()
        {
            Id = fetchedFeed.Id,
            UserId = fetchedFeed.UserId,
            Title = fetchedFeed.Title,
            Description = fetchedFeed.Description,
            FeedType = fetchedFeed.FeedType,
            ImageUrl = fetchedFeed.ImageUrl,
            Likes = fetchedFeed.Likes
        };
    }

    private static FeedDto MapFromVideoFeed(VideoFeed fetchedFeed)
    {
        return new FeedDto()
        {
            Id = fetchedFeed.Id,
            UserId = fetchedFeed.UserId,
            Title = fetchedFeed.Title,
            Description = fetchedFeed.Description,
            FeedType = fetchedFeed.FeedType,
            ImageUrl = fetchedFeed.ImageUrl,
            VideoUrl = fetchedFeed.VideoUrl,
            Likes = fetchedFeed.Likes
        };
    }
}