using static Feed.Core.FeedDomain.FeedCommand;
using Wolverine.Http;
using Wolverine;
using FluentValidation;

namespace Feed.API.FeedEndpoints;

public sealed record ListFeedQuery()
{
    public sealed class ListFeedQueryValidator : AbstractValidator<ListFeedQuery>
    {
        public ListFeedQueryValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }
}

public static class List
{
    public const string ListEndpoint = "/api/feed";

    [WolverineGet(ListEndpoint)]
    public static async Task<IResult> ListAsync(
        ListFeedQuery query,
        IMessageBus bus)
    {
        try
        {
            ListFeeds listFeeds = new();

            var feeds = await bus.InvokeAsync<List<Core.FeedDomain.Feed>>(listFeeds);            

            var feedList = feeds.Select(Mappers.MapFromDomainModel).ToList();

            return TypedResults.Ok(feedList);

        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}