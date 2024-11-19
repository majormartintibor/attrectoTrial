using Alba;

namespace Feed.IntegrationTests.Feed;
public sealed class UpdateTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Updating_a_Feed_should_succeed()
    {
        var scenario = await Host.Scenario(x =>
        {
            x.Post
                .Json("")
                .ToUrl("");

            x.StatusCodeShouldBeOk();
        });        
    }
}