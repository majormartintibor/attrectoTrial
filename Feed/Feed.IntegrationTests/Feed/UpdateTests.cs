using Alba;
using Feed.API.FeedEndpoints;
using Feed.IntegrationTests.Feed.Fixtures;
using static Feed.API.FeedEndpoints.Update;
using static Feed.API.FeedEndpoints.Get;
using Shouldly;
using Bogus.DataSets;
using Feed.Core.FeedDomain;

namespace Feed.IntegrationTests.Feed;
public sealed class UpdateTests(AppFixture fixture) : GivenFeedsExist(fixture)
{
    private static readonly Internet internet = new();
    private static readonly string newImageUrl = internet.Url();
    private static readonly string newVideoUrl = internet.Url();

    [Fact]
    public async Task Updating_a_Text_Feed_should_succeed()
    {
        InitialTextFeed.Title = "Brand new Title";
        InitialTextFeed.Description = "Hot new decription";

        var scenario = await Host.Scenario(x =>
        {
            x.Put
                .Json(new UpdateFeedCommand(InitialTextFeed))
                .ToUrl(UpdateEndpoint + InitialTextFeed.Id);

            x.StatusCodeShouldBeOk();
        });
        
        var getScenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + InitialTextFeed.Id);

            x.StatusCodeShouldBeOk();
        });

        var updatedFeed = getScenario.ReadAsJson<FeedDto>();
        updatedFeed.Title.ShouldBeEquivalentTo(InitialTextFeed.Title);
        updatedFeed.Description.ShouldBeEquivalentTo(InitialTextFeed.Description);
    }

    [Fact]
    public async Task Updating_an_Image_Feed_should_succeed()
    {
        InitialImageFeed.Title = "Brand new Title";
        InitialImageFeed.Description = "Hot new decription";
        InitialImageFeed.ImageUrl = newImageUrl;

        var scenario = await Host.Scenario(x =>
        {
            x.Put
                .Json(new UpdateFeedCommand(InitialImageFeed))
                .ToUrl(UpdateEndpoint + InitialImageFeed.Id);

            x.StatusCodeShouldBeOk();
        });

        var getScenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + InitialImageFeed.Id);

            x.StatusCodeShouldBeOk();
        });

        var updatedFeed = getScenario.ReadAsJson<FeedDto>();
        updatedFeed.Title.ShouldBeEquivalentTo(InitialImageFeed.Title);
        updatedFeed.Description.ShouldBeEquivalentTo(InitialImageFeed.Description);
        updatedFeed.ImageUrl.ShouldBeEquivalentTo(InitialImageFeed.ImageUrl);
    }

    [Fact]
    public async Task Updating_a_Video_Feed_should_succeed()
    {
        InitialVideoFeed.Title = "Brand new Title";
        InitialVideoFeed.Description = "Hot new decription";
        InitialVideoFeed.ImageUrl = newImageUrl;
        InitialVideoFeed.VideoUrl = newVideoUrl;

        var scenario = await Host.Scenario(x =>
        {
            x.Put
                .Json(new UpdateFeedCommand(InitialVideoFeed))
                .ToUrl(UpdateEndpoint + InitialVideoFeed.Id);

            x.StatusCodeShouldBeOk();
        });

        var getScenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + InitialVideoFeed.Id);

            x.StatusCodeShouldBeOk();
        });

        var updatedFeed = getScenario.ReadAsJson<FeedDto>();
        updatedFeed.Title.ShouldBeEquivalentTo(InitialVideoFeed.Title);
        updatedFeed.Description.ShouldBeEquivalentTo(InitialVideoFeed.Description);
        updatedFeed.ImageUrl.ShouldBeEquivalentTo(InitialVideoFeed.ImageUrl);
        updatedFeed.VideoUrl.ShouldBeEquivalentTo(InitialVideoFeed.VideoUrl);
    }

    //This is one example scenario where you can switch between FeedTypes
    //due to lack of time I wont cover all possible scenarios
    [Fact]
    public async Task Updating_a_Text_Feed_To_a_Video_Feed_should_succeed()
    {        
        InitialTextFeed.ImageUrl = newImageUrl;
        InitialTextFeed.VideoUrl= newVideoUrl;
        InitialTextFeed.FeedType = Core.FeedDomain.FeedType.Video;

        var scenario = await Host.Scenario(x =>
        {
            x.Put
                .Json(new UpdateFeedCommand(InitialTextFeed))
                .ToUrl(UpdateEndpoint + InitialTextFeed.Id);

            x.StatusCodeShouldBeOk();
        });

        var getScenario = await Host.Scenario(x =>
        {
            x.Get
                .Url(GetEndpoint + InitialTextFeed.Id);

            x.StatusCodeShouldBeOk();
        });

        var updatedFeed = getScenario.ReadAsJson<FeedDto>();
        updatedFeed.FeedType.ShouldBeEquivalentTo(FeedType.Video);
        updatedFeed.ImageUrl.ShouldBeEquivalentTo(InitialTextFeed.ImageUrl);
        updatedFeed.VideoUrl.ShouldBeEquivalentTo(InitialTextFeed.VideoUrl);
    }
}