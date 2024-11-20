using Alba;
using Feed.API.FeedEndpoints;
using Feed.IntegrationTests.Feed.Fixtures;
using static Feed.API.FeedEndpoints.Delete;

namespace Feed.IntegrationTests.Feed;

public sealed class SoftDeleteTests(AppFixture fixture) : GivenFeedsExist(fixture)
{
    [Fact]
    public async Task Soft_deleting_a_Text_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand())
                .ToUrl(DeleteEndpoint + BaselineData.DefaultTextFeedId);

            x.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task Soft_deleting_an_Image_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand())
                .ToUrl(DeleteEndpoint + BaselineData.DefaultImageFeedId);

            x.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task Soft_deleting_a_Video_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand())
                .ToUrl(DeleteEndpoint + BaselineData.DefaultVideoFeedId);

            x.StatusCodeShouldBeOk();
        });
    }
}