namespace Feed.Core.FeedDomain.Ports;
public interface IFeedRepository
{
    Task<Feed> GetAsync(Guid id);

    Task<List<Feed>> GetFeedsAsync();
    Task<List<Feed>> GetFeedsAsyncIncludeSoftDeleted();
    Task CreateFeedAsync(Feed feed);
    Task UpdateFeedAsync(Feed feed);
    Task SoftDeleteFeedAsync(Guid id);
    Task HardDeleteFeedsAsync();
}