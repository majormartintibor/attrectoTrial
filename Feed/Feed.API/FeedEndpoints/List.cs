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

            await bus.InvokeAsync(listFeeds);

            //HATEOAS to be truly restful

            return TypedResults.Ok();

        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}