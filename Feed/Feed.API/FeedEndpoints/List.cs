using Wolverine.Http;
using Wolverine;
using FluentValidation;
using Feed.Core.FeedDomain;

namespace Feed.API.FeedEndpoints;

//Could contain filter parameters
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
            FeedCommand.ListFeeds listFeeds = new();

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