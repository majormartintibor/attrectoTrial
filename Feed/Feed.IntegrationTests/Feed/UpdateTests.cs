using Alba;
using Feed.API.FeedEndpoints;
using Feed.IntegrationTests.Feed.Fixtures;
using static Feed.API.FeedEndpoints.Update;

namespace Feed.IntegrationTests.Feed;
public sealed class UpdateTests(AppFixture fixture) : GivenFeedsExist(fixture)
{
    [Fact]
    public async Task Updating_a_Text_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Put
                .Json(new UpdateFeedCommand())
                .ToUrl(UpdateEndpoint + Guid.NewGuid());

            x.StatusCodeShouldBeOk();
        });        
    }

    [Fact]
    public async Task Updating_an_Image_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Put
                .Json(new UpdateFeedCommand())
                .ToUrl(UpdateEndpoint + Guid.NewGuid());

            x.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task Updating_a_Video_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Put
                .Json(new UpdateFeedCommand())
                .ToUrl(UpdateEndpoint + Guid.NewGuid());

            x.StatusCodeShouldBeOk();
        });
    }
}