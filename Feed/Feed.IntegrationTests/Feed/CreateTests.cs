using Alba;
using Bogus;
using Feed.API.FeedEndpoints;
using Microsoft.AspNetCore.Http;
using Shouldly;
using static Feed.API.FeedEndpoints.Create;
using static Feed.API.FeedEndpoints.Get;
using Feed.Core.FeedDomain;
using Bogus.DataSets;

namespace Feed.IntegrationTests.Feed;
public sealed class CreateTests(AppFixture fixture) : IntegrationContext(fixture) 
{
    private static readonly Faker faker = new();
    private static readonly Internet internet = new();
    private static readonly string validTitle = faker.Random.String2(10);
    private static readonly string validDescription = faker.Random.String2(1000);
    private static readonly string invalidTitle = faker.Random.String2(1);
    private static readonly string invalidDescription = faker.Random.String2(3000);    
    private static readonly Guid userId = Guid.NewGuid();
    private static readonly string imageUrl = internet.Url();
    private static readonly string videoUrl = internet.Url();

    [Fact]
    public async Task Creating_a_Text_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    userId,
                    validTitle, 
                    validDescription,
                    FeedType.Text))
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
                .Json(new CreateFeedCommand(
                    userId,
                    validTitle, 
                    validDescription,
                    FeedType.Image,
                    imageUrl))
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
                .Json(new CreateFeedCommand(
                    userId,
                    validTitle, 
                    validDescription,
                    FeedType.Video,
                    imageUrl,
                    videoUrl))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = scenario.ReadAsJson<Guid>();
    }

    [Fact]
    public async Task Creating_a_Text_Feed_should_create_the_Feed()
    {
        var postScenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    userId,
                    validTitle,
                    validDescription,
                    FeedType.Text))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = postScenario.ReadAsJson<Guid>();

        var getScenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + feedId);

            x.StatusCodeShouldBeOk();
        });

        var createdFeed = getScenario.ReadAsJson<FeedDto>();
        createdFeed.UserId.ShouldBe(userId);
        createdFeed.Title.ShouldBeEquivalentTo(validTitle);
        createdFeed.Description.ShouldBeEquivalentTo(validDescription);
        createdFeed.FeedType.ShouldBeEquivalentTo(FeedType.Text);        
    }

    [Fact]
    public async Task Creating_an_Image_Feed_should_create_the_Feed()
    {
        var postScenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    userId,
                    validTitle,
                    validDescription,
                    FeedType.Image,
                    imageUrl))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = postScenario.ReadAsJson<Guid>();

        var getScenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + feedId);

            x.StatusCodeShouldBeOk();
        });

        var createdFeed = getScenario.ReadAsJson<FeedDto>();
        createdFeed.UserId.ShouldBe(userId);
        createdFeed.Title.ShouldBeEquivalentTo(validTitle);
        createdFeed.Description.ShouldBeEquivalentTo(validDescription);
        createdFeed.FeedType.ShouldBeEquivalentTo(FeedType.Image);
        createdFeed.ImageUrl.ShouldBeEquivalentTo(imageUrl);
    }

    [Fact]
    public async Task Creating_a_Video_Feed_should_create_the_Feed()
    {
        var postScenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    userId,
                    validTitle,
                    validDescription,
                    FeedType.Video,
                    imageUrl,
                    videoUrl))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        var feedId = postScenario.ReadAsJson<Guid>();

        var getScenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + feedId);

            x.StatusCodeShouldBeOk();
        });

        var createdFeed = getScenario.ReadAsJson<FeedDto>();
        createdFeed.UserId.ShouldBe(userId);
        createdFeed.Title.ShouldBeEquivalentTo(validTitle);
        createdFeed.Description.ShouldBeEquivalentTo(validDescription);
        createdFeed.FeedType.ShouldBeEquivalentTo(FeedType.Video);
        createdFeed.ImageUrl.ShouldBeEquivalentTo(imageUrl);
        createdFeed.VideoUrl.ShouldBeEquivalentTo(videoUrl);
    }

    [Fact]
    public async Task Too_short_title_returns_validation_error()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    userId,
                    invalidTitle,
                    validDescription,
                    FeedType.Text))
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
                .Json(new CreateFeedCommand(
                    userId,
                    validTitle,
                    invalidDescription,
                    FeedType.Text))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status400BadRequest);
        });        
    }
}