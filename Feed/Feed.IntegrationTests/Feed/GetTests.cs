using Alba;
using Feed.IntegrationTests.Feed.Fixtures;
using static Feed.API.FeedEndpoints.Get;

namespace Feed.IntegrationTests.Feed;

public sealed class GetTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Querying_for_Text_Feed_by_id_returns_Text_Feed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get               
                .Url(GetEndpoint + BaselineData.DefaultTextFeedId);

            x.StatusCodeShouldBeOk();
        });       
    }

    [Fact]
    public async Task Querying_for_Image_Feed_by_id_returns_Text_Feed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + BaselineData.DefaultImageFeedId);

            x.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task Querying_for_Video_Feed_by_id_returns_Text_Feed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get                
                .Url(GetEndpoint + BaselineData.DefaultVideoFeedId);

            x.StatusCodeShouldBeOk();
        });
    }
}