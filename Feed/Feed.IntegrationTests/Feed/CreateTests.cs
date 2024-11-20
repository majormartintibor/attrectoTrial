using Bogus;
using Feed.API.FeedEndpoints;
using Microsoft.AspNetCore.Http;
using static Feed.API.FeedEndpoints.Create;

namespace Feed.IntegrationTests.Feed;
public sealed class CreateTests(AppFixture fixture) : IntegrationContext(fixture) 
{
    private static readonly Faker faker = new();
    private readonly string validTitle = faker.Random.String2(10);
    private readonly string validDescription = faker.Random.String2(1000);
    private readonly string invalidTitle = faker.Random.String2(1);
    private readonly string invalidDescription = faker.Random.String2(3000);    


    [Fact]
    public async Task Creating_a_Text_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(validTitle, validDescription))
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
                .Json(new CreateFeedCommand(validTitle, validDescription))
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
                .Json(new CreateFeedCommand(validTitle, validDescription))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = scenario.ReadAsJson<Guid>();
    }

    [Fact]
    public async Task Too_short_title_returns_validation_error()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(invalidTitle, validDescription))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status400BadRequest);
        });
        

    }

    [Fact]
    public async Task Too_long_description_returns_validation_error()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(validTitle, invalidDescription))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status400BadRequest);
        });        
    }
}