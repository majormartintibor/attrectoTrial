using static Feed.Core.FeedDomain.FeedCommand;
using Wolverine.Http;
using Wolverine;
using FluentValidation;

namespace Feed.API.FeedEndpoints;

public sealed record DeleteFeedCommand()
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

    //DELETE implies that the resource is permanently removed, which isn't the case for a soft delete.
    //Using PATCH makes it clear that I am modifying the resource's state rather than removing it.
    //Also PATCH is an idempotent operation, which means applying the same operation multiple times will have the same effect.
    [WolverinePatch(DeleteEndpoint + "{id:guid}")]
    public static async Task<IResult> DeleteAsync(
        DeleteFeedCommand command,
        IMessageBus bus)
    {
        try
        {
            SoftDeleteFeed softDeleteFeed = new();

            await bus.InvokeAsync(softDeleteFeed);            

            return TypedResults.Ok();

        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}