using static Feed.Core.FeedDomain.FeedCommand;
using Wolverine.Http;
using Wolverine;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Feed.Core.Exceptions;

namespace Feed.API.FeedEndpoints;

public sealed record GetFeedQuery(Guid Id)
{
    public sealed class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
    {
        public GetFeedQueryValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }
}

public static class Get
{
    public const string GetEndpoint = "/api/feed/";

    [WolverineGet(GetEndpoint + "{id:guid}", RouteName = "Fasz")]
    public static async Task<IResult> GetAsync(
        [FromRoute] Guid id,
        IMessageBus bus,
        LinkGenerator linkGenerator,
        HttpContext context
        )
    {
        try
        {
            GetFeed getFeed = new(id);

            //HATEOAS to be truly restful
            List<LinkDto> links = [
                    new LinkDto("self", linkGenerator.GetUriByName(context, "GET_api_feed_id", new{id}) ?? string.Empty, "GET"),
                    new LinkDto("list", linkGenerator.GetUriByName(context, "GET_api_feed", []) ?? string.Empty, "GET"),
                    new LinkDto("update", linkGenerator.GetUriByName(context, "PUT_api_feed_id", new{id}) ?? string.Empty, "PUT"),
                    new LinkDto("delete", linkGenerator.GetUriByName(context, "PATCH_api_feed_id", new{id}) ?? string.Empty, "PATCH"),
                ];

            var fetchedFeed = await bus.InvokeAsync<Core.FeedDomain.Feed>(getFeed);

            FeedDto feed = Mappers.MapFromDomainModel(fetchedFeed);
            feed.Links = links;

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