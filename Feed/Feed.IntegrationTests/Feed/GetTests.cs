using Alba;
using Feed.IntegrationTests.Feed.Fixtures;
using Microsoft.AspNetCore.Http;
using static Feed.API.FeedEndpoints.Get;

namespace Feed.IntegrationTests.Feed;

public sealed class GetTests(AppFixture fixture) : GivenFeedsExist(fixture)
{
    [Fact]
    public async Task Querying_for_Text_Feed_by_id_returns_Text_Feed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get               
                .Url(GetEndpoint + InitialTextFeed.Id);

            x.StatusCodeShouldBeOk();
        });       
    }

    [Fact]
    public async Task Querying_for_Image_Feed_by_id_returns_Text_Feed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + InitialImageFeed.Id);

            x.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task Querying_for_Video_Feed_by_id_returns_Text_Feed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get                
                .Url(GetEndpoint + InitialVideoFeed.Id);

            x.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task Querying_for_a_non_existing_feed_returns_not_found()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + Guid.NewGuid());

            x.StatusCodeShouldBe(StatusCodes.Status404NotFound);
        });
    }
}