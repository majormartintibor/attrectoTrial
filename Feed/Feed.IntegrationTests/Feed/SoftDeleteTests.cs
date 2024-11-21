using Alba;
using Feed.API.FeedEndpoints;
using Feed.IntegrationTests.Feed.Fixtures;
using Feed.Persistence;
using Microsoft.AspNetCore.Http;
using static Feed.API.FeedEndpoints.Delete;
using static Feed.API.FeedEndpoints.Get;

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
                .ToUrl(DeleteEndpoint + SeedData.FeedGuids["Feed1"]);

            x.StatusCodeShouldBeOk();
        });

        var notFoundscenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + SeedData.FeedGuids["Feed1"]);

            x.StatusCodeShouldBe(StatusCodes.Status404NotFound);
        });
    }

    [Fact]
    public async Task Soft_deleting_an_Image_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand())
                .ToUrl(DeleteEndpoint + SeedData.FeedGuids["Feed2"]);

            x.StatusCodeShouldBeOk();
        });

        var notFoundscenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + SeedData.FeedGuids["Feed2"]);

            x.StatusCodeShouldBe(StatusCodes.Status404NotFound);
        });
    }

    [Fact]
    public async Task Soft_deleting_a_Video_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand())
                .ToUrl(DeleteEndpoint + SeedData.FeedGuids["Feed3"]);

            x.StatusCodeShouldBeOk();
        });

        var notFoundscenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + SeedData.FeedGuids["Feed3"]);

            x.StatusCodeShouldBe(StatusCodes.Status404NotFound);
        });
    }
}