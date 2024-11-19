namespace Feed.API.FeedEndpoints;

//starting DTO draft
public sealed record FeedDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;    
    public List<LinkDto> Links { get; set; } = [];
}