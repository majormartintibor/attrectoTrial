using Feed.API.FeedEndpoints;

namespace Feed.IntegrationTests.Feed.Fixtures;

//With the seeding strategy I use, this is not actually needed.
//I just wanted to show the possibility of my Integration Test harness
//that I can provide any number of Given scenarios for my tests, 
//making this very flexible!
public class GivenFeedsExist(AppFixture fixture) : IntegrationContext(fixture), IAsyncLifetime
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        InitialTextFeed = await Host.CreateTextFeed();
        InitialImageFeed = await Host.CreateImageFeed();
        InitialVideoFeed = await Host.CreateVideoFeed();
    }  

    public FeedDto InitialTextFeed { get; protected set; } = default!;
    public FeedDto InitialImageFeed { get; protected set; } = default!;
    public FeedDto InitialVideoFeed { get; protected set; } = default!;
}