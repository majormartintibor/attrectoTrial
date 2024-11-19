using Feed.API.FeedEndpoints;
using Microsoft.AspNetCore.Http;
using static Feed.API.FeedEndpoints.Create;

namespace Feed.IntegrationTests.Feed;
public sealed class CreateTests(AppFixture fixture) : IntegrationContext(fixture) 
{
    [Fact]
    public async Task Creating_a_Text_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand())
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = scenario.ReadAsJson<Guid>();
    }

    [Fact]
    public async Task Creating_an_Image_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand())
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = scenario.ReadAsJson<Guid>();
    }

    [Fact]
    public async Task Creating_a_Video_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand())
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = scenario.ReadAsJson<Guid>();
    }
}