using Shouldly;

namespace Feed.IntegrationTests.Feed;
public class HardDeleteTests
{
    [Fact]
    public async Task Hard_deleting_a_Feed_should_succeed()
    {
        false.ShouldBeTrue();

        await Task.CompletedTask;
    }
}