using Feed.Core.FeedDomain.Ports;
using System.ComponentModel;
using static Feed.Core.FeedDomain.FeedCommand;

namespace Feed.Core.FeedDomain;

public abstract record Feed
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public FeedType FeedType { get; init; }
    public int Likes { get; init; } = 0;

    protected Feed() { }

    public static Feed CreateFeed(
        Guid Id,
        Guid UserId,
        string Title,
        string Description,
        FeedType FeedType,
        int Likes,
        string ImageUrl = "",
        string VideoUrl = ""
        )
    {
        return FeedType switch
        {
            FeedType.Text => TextFeed.CreateTextFeed(Id, UserId, Title, Description, Likes),
            FeedType.Image => ImageFeed.CreateImageFeed(Id, UserId, Title, Description, ImageUrl, Likes),
            FeedType.Video => VideoFeed.CreateVideoFeed(Id, UserId, Title, Description, ImageUrl, VideoUrl, Likes),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    public sealed record TextFeed : Feed
    {
        private TextFeed() { }

        internal static TextFeed CreateTextFeed(
            Guid Id,
            Guid UserId,
            string Title,
            string Description,
            int Likes)
        {
            return new TextFeed() with 
            { 
                Id = Id, 
                UserId = UserId,
                Title = Title,
                Description = Description,
                FeedType = FeedType.Text,
                Likes = Likes
            };
        }
    }

    public sealed record ImageFeed : Feed
    {
        public string ImageUrl { get; init; } = string.Empty;

        private ImageFeed() { }

        internal static ImageFeed CreateImageFeed(
            Guid Id,
            Guid UserId,
            string Title,
            string Description,            
            string ImageUrl,
            int Likes)
        {
            return new ImageFeed() with
            {
                Id = Id,
                UserId = UserId,
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                FeedType= FeedType.Image,
                Likes = Likes
            };
        }
    }

    public sealed record VideoFeed : Feed
    {
        public string ImageUrl { get; init; } = string.Empty;
        public string VideoUrl { get; init; } = string.Empty;

        private VideoFeed() { }

        internal static VideoFeed CreateVideoFeed(
            Guid Id,
            Guid UserId,
            string Title,
            string Description,            
            string ImageUrl,
            string VideoUrl,
            int Likes)
        {
            return new VideoFeed() with
            {
                Id = Id,
                UserId = UserId,
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                VideoUrl = VideoUrl,
                FeedType = FeedType.Video,
                Likes = Likes
            };
        }
    }
}

public abstract record FeedCommand
{
    public sealed record CreateFeed(
        Guid UserId, 
        string Title, 
        string Description,
        FeedType FeedType,
        string ImageUrl = "",
        string VideoUrl = "");
    public sealed record UpdateFeed();
    public sealed record SoftDeleteFeed();
    public sealed record HardDeleteFeed();
    public sealed record GetFeed(Guid FeedId);
    public sealed record ListFeeds();
}

//This is convention based. Wolverine will discover handlers by the command - commandHandler naming convention
//The method must be called either Handle or Consume
public static class CreateFeedHandler
{
    public static async Task<Guid> Handle(
        CreateFeed command,
        IFeedRepository feedRepository)
    {
        var id = Guid.NewGuid();

        var feed = Feed.CreateFeed(
            id,
            command.UserId,
            command.Title,
            command.Description,
            command.FeedType,
            0,
            command.ImageUrl,
            command.VideoUrl);

        await feedRepository.CreateFeedAsync(feed);

        return id;
    }
}

public static class UpdateFeedHandler
{
    public static async Task Handle(
        UpdateFeed command,
        IFeedRepository feedRepository)
    {
        await Task.CompletedTask;
    }
}

public static class SofDeleteFeedHandler
{
    public static async Task Handle(
        SoftDeleteFeed command,
        IFeedRepository feedRepository)
    {
        await Task.CompletedTask;
    }
}

public static class HardDeleteFeedHandler
{
    public static async Task Handle(
        HardDeleteFeed command,
        IFeedRepository feedRepository)
    {
        await Task.CompletedTask;
    }
}

public static class GetFeedHandler
{
    public static async Task<Feed> Handle(
        GetFeed query,
        IFeedRepository feedRepository)
    {
        return await feedRepository.GetAsync(query.FeedId);
    }
}

public static class ListFeedsHandler
{
    public static async Task<List<Feed>> Handle(
        ListFeeds query,
        IFeedRepository feedRepository)
    {
        return await feedRepository.GetFeedsAsync();
    }
}