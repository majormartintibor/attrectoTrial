using Alba;
using Feed.API.FeedEndpoints;
using Shouldly;
using static Feed.API.FeedEndpoints.List;

namespace Feed.IntegrationTests.Feed;
public sealed class ListTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Querying_for_all_Feeds_returns_all_Feeds()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Get
                .Json(new ListFeedQuery())
                .ToUrl(ListEndpoint);

            x.StatusCodeShouldBeOk();
        });

        var response = scenario.ReadAsJson<List<FeedDto>>();
        response.Count.ShouldBeGreaterThan(0);
    }  
}