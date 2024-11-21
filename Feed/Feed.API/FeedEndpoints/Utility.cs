using Feed.Core.FeedDomain;
using static Feed.Core.FeedDomain.Feed;
using System.ComponentModel;

namespace Feed.API.FeedEndpoints;

internal static class Mappers
{
    internal static FeedDto MapFromDomainModel(Core.FeedDomain.Feed fetchedFeed)
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

    internal static Core.FeedDomain.Feed MapToDomainModel(FeedDto feedDto)
    {
        return Core.FeedDomain.Feed.CreateFeed(
                feedDto.Id,
                feedDto.UserId,
                feedDto.Title,
                feedDto.Description,
                feedDto.FeedType,
                feedDto.Likes,
                feedDto.ImageUrl,
                feedDto.VideoUrl
            );
    }
}

internal static class HATEOAS
{
    internal static FeedDto AddLinks(FeedDto feed, LinkGenerator linkGenerator, HttpContext context)
    {
        return feed with
        {
            Links = [
                    new LinkDto("self", linkGenerator.GetUriByName(context, "GET_api_feed_id", new{feed.Id}) ?? string.Empty, "GET"),
                    new LinkDto("list", linkGenerator.GetUriByName(context, "GET_api_feed", []) ?? string.Empty, "GET"),
                    new LinkDto("update", linkGenerator.GetUriByName(context, "PUT_api_feed_id", new{feed.Id}) ?? string.Empty, "PUT"),
                    new LinkDto("delete", linkGenerator.GetUriByName(context, "PATCH_api_feed_id", new{feed.Id}) ?? string.Empty, "PATCH"),
                ]
        };
    }
}