using Wolverine.Http;
using Wolverine;
using Microsoft.AspNetCore.Mvc;
using Feed.Core.Exceptions;
using Feed.Core.FeedDomain;

namespace Feed.API.FeedEndpoints;

public static class Get
{
    public const string GetEndpoint = "/api/feed/";

    [WolverineGet(GetEndpoint + "{id:guid}")]
    public static async Task<IResult> GetAsync(
        [FromRoute] Guid id,
        IMessageBus bus,
        LinkGenerator linkGenerator,
        HttpContext context
        )
    {
        try
        {
            FeedCommand.GetFeed getFeed = new(id);
            var fetchedFeed = await bus.InvokeAsync<Core.FeedDomain.Feed>(getFeed);

            FeedDto feed = HATEOAS.AddLinks(
                Mappers.MapFromDomainModel(fetchedFeed), linkGenerator, context);

            return TypedResults.Ok(feed);
        }
        catch (FeedNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }    
}