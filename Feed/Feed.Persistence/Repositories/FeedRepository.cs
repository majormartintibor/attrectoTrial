using Feed.Core.Exceptions;
using Feed.Core.FeedDomain.Ports;
using Feed.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Feed.Persistence.Repositories;
internal sealed class FeedRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IFeedRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

    public async Task CreateFeedAsync(Core.FeedDomain.Feed feed)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        Entity.Feed feedToSave = FeedMappers.MapFromDomainModel(feed);

        await context.Feeds.AddAsync(feedToSave);
        await context.SaveChangesAsync();        
    }    

    public async Task SoftDeleteFeedAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var feed = await context.Feeds.FirstOrDefaultAsync(f => f.Id == id) 
            ?? throw new FeedNotFoundException(id);
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
                ?? throw new FeedNotFoundException(id);

        Core.FeedDomain.Feed feedToReturn = FeedMappers.MapFromPersistenceModel(feed);

        return feedToReturn;
    }    

    public async Task<List<Core.FeedDomain.Feed>> GetFeedsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Feeds
            .AsNoTracking()
            .Where(f => !f.IsDeleted)
            .Include(f => f.UserFeedLikes)
            .Select(f => FeedMappers.MapFromPersistenceModel(f))
            .ToListAsync();        
    }

    public async Task UpdateFeedAsync(Core.FeedDomain.Feed feed)
    {
        using var context = await _contextFactory.CreateDbContextAsync();        

        var feedToUpdate = await context.Feeds.FirstOrDefaultAsync(f => f.Id == feed.Id)
            ?? throw new FeedNotFoundException(feed.Id);

        FeedMappers.MapFromDomainModel(feed, feedToUpdate);

        await context.SaveChangesAsync();
    }       
}