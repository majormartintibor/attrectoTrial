using Feed.API.FeedEndpoints;

namespace Feed.IntegrationTests.Feed.Fixtures;
public class GivenFeedsExist(AppFixture fixture) : IntegrationContext(fixture), IAsyncLifetime
{
    public override async Task InitializeAsync()
    {
        InitialTextFeed = await Host.CreateTextFeed();
        InitialImageFeed = await Host.CreateImageFeed();
        InitialVideoFeed = await Host.CreateVideoFeed();
    }  

    public FeedDto InitialTextFeed { get; protected set; } = default!;
    public FeedDto InitialImageFeed { get; protected set; } = default!;
    public FeedDto InitialVideoFeed { get; protected set; } = default!;
}