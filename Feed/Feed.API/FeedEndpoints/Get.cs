using static Feed.Core.FeedDomain.FeedCommand;
using Wolverine.Http;
using Wolverine;
using FluentValidation;

namespace Feed.API.FeedEndpoints;

public sealed record GetFeedQuery()
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
    public const string GetEndpoint = "/api/feed/{id:guid}";
    
    [WolverineGet(GetEndpoint)]
    public static async Task<IResult> GetAsync(
        GetFeedQuery query,
        IMessageBus bus)
    {
        try
        {
            GetFeed getFeed = new();

            await bus.InvokeAsync(getFeed);

            //HATEOAS to be truly restful

            return TypedResults.Ok();

        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}