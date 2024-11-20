using static Feed.Core.FeedDomain.FeedCommand;
using Wolverine.Http;
using Wolverine;
using FluentValidation;

namespace Feed.API.FeedEndpoints;

public sealed record UpdateFeedCommand()
{
    public sealed class UpdateFeedCommandValidator : AbstractValidator<UpdateFeedCommand>
    {
        public UpdateFeedCommandValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }
}

public static class Update
{
    public const string UpdateEndpoint = "/api/feed/";

    [WolverinePut(UpdateEndpoint + "{id:guid}")]
    public static async Task<IResult> UpdateAsync(
        UpdateFeedCommand command,
        IMessageBus bus)
    {
        try
        {
            UpdateFeed updateFeed = new();

            await bus.InvokeAsync(updateFeed);            

            return TypedResults.Ok();

        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}