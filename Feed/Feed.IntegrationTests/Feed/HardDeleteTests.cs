using Feed.IntegrationTests.Feed.Fixtures;
using Shouldly;

namespace Feed.IntegrationTests.Feed;
public sealed class HardDeleteTests(AppFixture fixture) : GivenSoftDeletedFeedExists(fixture), IAsyncLifetime
{

    //Wolverine 3.0 does not support trying to resolve IMessageBus at runtime.
    //This is why I only test the repository behaviour here
    [Fact]
    public async Task Hard_deleting_Feeds_should_delete_Feeds()
    {
        var initialFeedCount = (await FeedRepository.GetFeedsAsyncIncludeSoftDeleted()).Count;

        await FeedRepository.HardDeleteFeedsAsync();

        var feedCountAfterHardDelete = (await FeedRepository.GetFeedsAsyncIncludeSoftDeleted()).Count;

        feedCountAfterHardDelete.ShouldBeEquivalentTo(initialFeedCount - 1);
    }
}