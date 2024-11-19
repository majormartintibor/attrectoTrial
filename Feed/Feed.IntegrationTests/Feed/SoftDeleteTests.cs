using Alba;

namespace Feed.IntegrationTests.Feed;

public class SoftDeleteTests(AppFixture fixture) : IntegrationContext(fixture)
{
    [Fact]
    public async Task Soft_deleting_a_Feed_should_succeed()
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