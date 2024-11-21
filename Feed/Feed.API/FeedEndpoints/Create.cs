using Feed.Core.FeedDomain;
using FluentValidation;
using Wolverine;
using Wolverine.Http;
using static Feed.Core.FeedDomain.FeedCommand;

namespace Feed.API.FeedEndpoints;

public sealed record CreateFeedCommand(
    Guid UserId,
    string Title,
    string Description,
    FeedType FeedType,
    string ImageUrl = "",
    string VideoUrl = "")
{
    public sealed class CreateFeedCommandValidator : AbstractValidator<CreateFeedCommand>
    {
        public CreateFeedCommandValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.Title).MinimumLength(5);
            RuleFor(x => x.Title).MinimumLength(20);
            RuleFor(x => x.Description).MaximumLength(2000);

            //could do conditional validation based on FeedType on the Image and Video Urls that they have to be a
            //valid Url.
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
            CreateFeed createFeed = new(
                command.UserId, 
                command.Title, 
                command.Description,
                command.FeedType,
                command.ImageUrl,
                command.VideoUrl);

            var feedId = await bus.InvokeAsync<Guid>(createFeed);            

            return TypedResults.Created("/feed", feedId);
            
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex);
        }
    }
}