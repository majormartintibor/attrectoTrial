using FluentValidation;
using Wolverine;
using Wolverine.Http;
using static Feed.Core.FeedDomain.FeedCommand;

namespace Feed.API.FeedEndpoints;

public sealed record CreateFeedCommand(
    string Title,
    string Description)
{
    public sealed class CreateFeedCommandValidator : AbstractValidator<CreateFeedCommand>
    {
        public CreateFeedCommandValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Title).MinimumLength(5);
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }
}

public static class Create
{    
    public const string CreateEndpoint = "/api/feed";

    [WolverinePost(CreateEndpoint)]
    public static async Task<IResult> CreateAsync(
        CreateFeedCommand command,
        IMessageBus bus)
    {
        try
        {
            UpdateFeed createFeed = new();

            var feedId = await bus.InvokeAsync<Guid>(createFeed);            

            return TypedResults.Created("/feed", feedId);
            
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}