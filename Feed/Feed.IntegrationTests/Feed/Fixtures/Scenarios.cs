using Alba;
using Feed.API.FeedEndpoints;
using Feed.Core.FeedDomain;
using Microsoft.AspNetCore.Http;
using static Feed.API.FeedEndpoints.Create;
using static Feed.API.FeedEndpoints.Delete;
using static Feed.API.FeedEndpoints.List;

namespace Feed.IntegrationTests.Feed.Fixtures;
public static class Scenarios
{
    public static async Task<FeedDto> CreateTextFeed(
        this IAlbaHost api)
    {
        var feedDto = new FeedDto()
        {            
            UserId = Guid.NewGuid(),
            Title = "Fine title",
            Description = "Fine description",
            FeedType = FeedType.Text,
        };

        var scenario = await api.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    feedDto.UserId,
                    feedDto.Title,
                    feedDto.Description,
                    feedDto.FeedType))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        feedDto.Id = scenario.ReadAsJson<Guid>();

        return feedDto;
    }

    public static async Task<FeedDto> CreateImageFeed(
        this IAlbaHost api)
    {
        var feedDto = new FeedDto()
        {
            UserId = Guid.NewGuid(),
            Title = "Fine title",
            Description = "Fine description",
            FeedType = FeedType.Image,
            ImageUrl = ""
        };

        var scenario = await api.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    feedDto.UserId,
                    feedDto.Title,
                    feedDto.Description,
                    feedDto.FeedType,
                    feedDto.ImageUrl))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        feedDto.Id = scenario.ReadAsJson<Guid>();

        return feedDto;
    }

    public static async Task<FeedDto> CreateVideoFeed(
        this IAlbaHost api)
    {
        var feedDto = new FeedDto()
        {
            UserId = Guid.NewGuid(),
            Title = "Fine title",
            Description = "Fine description",
            FeedType = FeedType.Video,
            ImageUrl = "",
            VideoUrl = ""
        };

        var scenario = await api.Scenario(x =>
        {
            x.Post
                .Json(new CreateFeedCommand(
                    feedDto.UserId,
                    feedDto.Title,
                    feedDto.Description,
                    feedDto.FeedType,
                    feedDto.ImageUrl,
                    feedDto.VideoUrl))
                .ToUrl(CreateEndpoint);

            x.StatusCodeShouldBe(StatusCodes.Status201Created);
        });

        feedDto.Id = scenario.ReadAsJson<Guid>();

        return feedDto;
    }

    public static async Task SoftDeletedOneFeed(this IAlbaHost api)
    {
        var listScenario = await api.Scenario(x =>
        {
            x.Get
                .Json(new ListFeedQuery())
                .ToUrl(ListEndpoint);

            x.StatusCodeShouldBeOk();
        });
        var feeds = listScenario.ReadAsJson<List<FeedDto>>();        

        var deleteScenario = await api.Scenario(x =>
        {
            x.Patch
                .Json(new DeleteFeedCommand(feeds.First().UserId))
                .ToUrl(DeleteEndpoint + feeds.First().Id);

            x.StatusCodeShouldBeOk();
        });
    }
}