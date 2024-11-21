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
                .Json(new DeleteFeedCommand(SeedData.UserGuids["User1"]))
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
                .Json(new DeleteFeedCommand(SeedData.UserGuids["User2"]))
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

    [Fact]
    public async Task Soft_deleting_a_Video_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand(SeedData.UserGuids["User3"]))
                .ToUrl(DeleteEndpoint + SeedData.FeedGuids["Feed5"]);

            x.StatusCodeShouldBeOk();
        });

        var notFoundscenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + SeedData.FeedGuids["Feed5"]);

            x.StatusCodeShouldBe(StatusCodes.Status404NotFound);
        });
    }

    [Fact]
    public async Task Soft_deleting_another_users_feed_returns_unauthorized()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand(SeedData.UserGuids["User1"]))
                .ToUrl(DeleteEndpoint + SeedData.FeedGuids["Feed5"]);

            x.StatusCodeShouldBe(StatusCodes.Status401Unauthorized);
        });        
    }
}