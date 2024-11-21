using Wolverine.Http;
using Wolverine;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Attributes;
using Feed.Core.FeedDomain;

namespace Feed.API.FeedEndpoints;

public sealed record UpdateFeedCommand(FeedDto Feed)
{
    public sealed class UpdateFeedCommandValidator : AbstractValidator<UpdateFeedCommand>
    {
        public UpdateFeedCommandValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Feed).NotNull();
        }
    }
}

public static class Update
{
    public const string UpdateEndpoint = "/api/feed/";

    //This is "a" way to check if the user can delete a feed. I wouldn't actually do it like this
    //in a real application for this use case, because it has an additional database roundtrip,
    //but I wanted to use this to show the WolverineBefore middleware option. You can run complex
    //precondition checks, logs, messaging etc here to keep the actual endpoint clean and focused
    //on the main logic.
    [WolverineBefore]
    public static async Task<ProblemDetails> CheckIfUserCanUpdateFeed(
        UpdateFeedCommand command,
        HttpContext context,
        IMessageBus bus)
    {
        if (!context.Request.RouteValues.TryGetValue("id", out var id))
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = "Route parameter id could not be read"
            };
        }

        var feed = await bus.InvokeAsync<Core.FeedDomain.Feed>(new FeedCommand.GetFeed(Guid.Parse(id!.ToString()!)));

        return feed.UserId == command.Feed.UserId ? WolverineContinue.NoProblems :
            new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Detail = "You have no right to update the resource"
            };
    }

    [WolverinePut(UpdateEndpoint + "{id:guid}")]
    public static async Task<IResult> UpdateAsync(
        UpdateFeedCommand command,
        IMessageBus bus)
    {
        try
        {
            FeedCommand.UpdateFeed updateFeed = new(Mappers.MapToDomainModel(command.Feed));

            await bus.InvokeAsync(updateFeed);            

            return TypedResults.Ok();

        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}