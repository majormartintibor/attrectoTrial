using Wolverine.Http;
using Wolverine;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Feed.Core.Exceptions;
using Wolverine.Attributes;
using Feed.Core.FeedDomain;

namespace Feed.API.FeedEndpoints;

public sealed record DeleteFeedCommand(Guid UserId)
{
    public sealed class DeleteFeedCommandValidator : AbstractValidator<DeleteFeedCommand>
    {
        public DeleteFeedCommandValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }
}

public static class Delete
{
    public const string DeleteEndpoint = "/api/feed/";

    //This is "a" way to check if the user can delete a feed. I wouldn't actually do it like this
    //in a real application for this use case, because it has an additional database roundtrip,
    //but I wanted to use this to show the WolverineBefore middleware option. You can run complex
    //precondition checks, logs, messaging etc here to keep the actual endpoint clean and focused
    //on the main logic.
    [WolverineBefore]
    public static async Task<ProblemDetails> CheckIfUserCanDeleteFeed(
        DeleteFeedCommand command,
        HttpContext context,
        IMessageBus bus)
    {
        if(!context.Request.RouteValues.TryGetValue("id", out var id))
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = "Route parameter id could not be read"
            };
        }

        var feed = await bus.InvokeAsync<Core.FeedDomain.Feed>(new FeedCommand.GetFeed(Guid.Parse(id!.ToString()!)));

        return feed.UserId == command.UserId ? WolverineContinue.NoProblems :
            new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Detail = "You have no right to delete the resource"
            };            
    }

    //DELETE implies that the resource is permanently removed, which isn't the case for a soft delete.
    //Using PATCH makes it clear that I am modifying the resource's state rather than removing it.
    //Also PATCH is an idempotent operation, which means applying the same operation multiple times will have the same effect.
    [WolverinePatch(DeleteEndpoint + "{id:guid}")]
    public static async Task<IResult> DeleteAsync(
        [FromRoute] Guid id,
        DeleteFeedCommand command,
        IMessageBus bus)
    {
        try
        {
            FeedCommand.SoftDeleteFeed softDeleteFeed = new(id);

            await bus.InvokeAsync(softDeleteFeed);            

            return TypedResults.Ok();

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