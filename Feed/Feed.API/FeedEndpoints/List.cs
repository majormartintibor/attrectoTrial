using Wolverine.Http;
using Wolverine;
using FluentValidation;
using Feed.Core.FeedDomain;

namespace Feed.API.FeedEndpoints;

//Could contain filter parameters. That's why I leave it in despite being unused for now.
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
        IMessageBus bus,
        LinkGenerator linkGenerator,
        HttpContext context)
    {
        try
        {
            FeedCommand.ListFeeds listFeeds = new();

            var feeds = await bus.InvokeAsync<List<Core.FeedDomain.Feed>>(listFeeds);            

            //Isn't this kind of Functional Programming approach with method chaining
            //and immutable data just beautiful? :) 
            var feedList = feeds
                .Select(Mappers.MapFromDomainModel)
                .Select(f => HATEOAS.AddLinks(f, linkGenerator, context))
                .ToList();

            return TypedResults.Ok(feedList);

        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }    
}