using static Feed.Core.FeedDomain.FeedCommand;

namespace Feed.Core.FeedDomain;

//only a draft
internal abstract record Feed
{
    Guid Id { get; init; }
    string Title { get; init; } = string.Empty;
    string Description { get; init; } = string.Empty;
    int Like {  get; init; }

    private Feed() { }

    internal sealed record TextFeed : Feed
    {
        private TextFeed() { }    
    }

    internal sealed record ImageFeed : Feed
    {
        private ImageFeed() { }
    }

    internal sealed record VideoFeed : Feed
    {
        private VideoFeed() { }
    }
}

public abstract record FeedCommand
{
    public sealed record CreateFeed();
    public sealed record UpdateFeed();
    public sealed record SoftDeleteFeed();
    public sealed record HardDeleteFeed();
    public sealed record GetFeed();
    public sealed record ListFeeds();
}

//This is convention based. Wolverine will discover handlers by the command - commandHandler naming convention
//The method must be called either Handle or Consume
public static class CreateFeedHandler
{
    public static async Task<Guid> Handle(CreateFeed command)
    {       
        await Task.CompletedTask;
        return Guid.NewGuid();
    }
}

public static class UpdateFeedHandler
{
    public static async Task Handle(UpdateFeed command)
    {
        await Task.CompletedTask;        
    }
}

public static class SofDeleteFeedHandler
{
    public static async Task Handle(SoftDeleteFeed command)
    {
        await Task.CompletedTask;       
    }
}

public static class HardDeleteFeedHandler
{
    public static async Task Handle(HardDeleteFeed command)
    {
        await Task.CompletedTask;
    }
}

public static class GetFeedHandler
{
    public static async Task Handle(GetFeed query)
    {
        await Task.CompletedTask;        
    }
}

public static class ListFeedsHandler
{
    public static async Task Handle(ListFeeds query)
    {
        await Task.CompletedTask;
    }
}