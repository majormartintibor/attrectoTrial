using Feed.Core.Exceptions;
using Feed.Core.FeedDomain;
using Feed.Core.FeedDomain.Ports;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static Feed.Core.FeedDomain.Feed;

namespace Feed.Persistence.Repositories;
internal sealed class FeedRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IFeedRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

    public async Task CreateFeedAsync(Core.FeedDomain.Feed feed)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        Entity.Feed feedToSave = MapFromDomainModel(feed);

        await context.Feeds.AddAsync(feedToSave);
        await context.SaveChangesAsync();        
    }    

    public async Task SoftDeleteFeedAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var feed = await context.Feeds.FirstOrDefaultAsync(f => f.Id == id) 
            ?? throw new FeedNotFoundException($"Feed with ID {id} not found.");
        feed.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task HardDeleteFeedAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var feedToRemove = await context.Feeds.FirstAsync(f => f.Id == id);
        context.Feeds.Remove(feedToRemove);        
        await context.SaveChangesAsync();
    }

    public async Task<Core.FeedDomain.Feed> GetAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var feed = await context.Feeds
            //performance optimization, we do not need change tracking
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted) 
                ?? throw new FeedNotFoundException($"Feed with ID {id} not found.");

        Core.FeedDomain.Feed feedToReturn = MapFromPersistenceModel(feed);

        return feedToReturn;
    }    

    public async Task<List<Core.FeedDomain.Feed>> GetFeedsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Feeds
            .AsNoTracking()
            .Where(f => !f.IsDeleted)
            .Include(f => f.UserFeedLikes)
            .Select(f => MapFromPersistenceModel(f))
            .ToListAsync();        
    }

    public async Task UpdateFeedAsync(Core.FeedDomain.Feed feed)
    {
        using var context = await _contextFactory.CreateDbContextAsync();        

        var feedToUpdate = await context.Feeds.FirstAsync(f => f.Id == feed.Id);
        //map missing

        await context.SaveChangesAsync();
    }

    private static Core.FeedDomain.Feed MapFromPersistenceModel(Entity.Feed feed)
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
    
    private static Entity.Feed MapFromDomainModel(Core.FeedDomain.Feed feed)
    {
        return feed.FeedType switch
        {
            FeedType.Text => MapToTextFeed(feed),
            FeedType.Image => MapToImageFeed(feed),
            FeedType.Video => MapToVideoFeed(feed),
            _ => throw new InvalidEnumArgumentException()
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