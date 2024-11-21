using Feed.Core.FeedDomain;

namespace Feed.API.FeedEndpoints;

public sealed record FeedDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;    
    public List<LinkDto> Links { get; set; } = [];
    public Guid UserId { get; set; }
    public FeedType FeedType { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public int Likes { get; set; }
}