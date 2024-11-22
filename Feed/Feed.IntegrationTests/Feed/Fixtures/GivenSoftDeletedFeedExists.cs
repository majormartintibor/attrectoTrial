namespace Feed.IntegrationTests.Feed.Fixtures;
public class GivenSoftDeletedFeedExists(AppFixture fixture) : IntegrationContext(fixture), IAsyncLifetime
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Host.SoftDeletedOneFeed();        
    }    
}