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
        string ImageUrl = "",
        string VideoUrl = "")
    {
        return FeedType switch
        {
            FeedType.Text => TextFeed.CreateTextFeed(Id, UserId, Title, Description),
            FeedType.Image => ImageFeed.CreateImageFeed(Id, UserId, Title, Description, ImageUrl),
            FeedType.Video => VideoFeed.CreateVideoFeed(Id, UserId, Title, Description, ImageUrl, VideoUrl),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    public sealed record TextFeed : Feed
    {
        private TextFeed() { }

        public static TextFeed CreateTextFeed(
            Guid Id,
            Guid UserId,
            string Title,
            string Description)
        {
            return new TextFeed() with 
            { 
                Id = Id, 
                UserId = UserId,
                Title = Title,
                Description = Description,
                FeedType = FeedType.Text
            };
        }
    }

    public sealed record ImageFeed : Feed
    {
        public string ImageUrl { get; init; } = string.Empty;

        private ImageFeed() { }

        public static ImageFeed CreateImageFeed(
            Guid Id,
            Guid UserId,
            string Title,
            string Description,            
            string ImageUrl)
        {
            return new ImageFeed() with
            {
                Id = Id,
                UserId = UserId,
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                FeedType= FeedType.Image
            };
        }
    }

    public sealed record VideoFeed : Feed
    {
        public string ImageUrl { get; init; } = string.Empty;
        public string VideoUrl { get; init; } = string.Empty;

        private VideoFeed() { }

        public static VideoFeed CreateVideoFeed(
            Guid Id,
            Guid UserId,
            string Title,
            string Description,            
            string ImageUrl,
            string VideoUrl)
        {
            return new VideoFeed() with
            {
                Id = Id,
                UserId = UserId,
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                VideoUrl = VideoUrl,
                FeedType = FeedType.Video
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
    public static async Task Handle(
        ListFeeds query,
        IFeedRepository feedRepository)
    {
        await Task.CompletedTask;
    }
}