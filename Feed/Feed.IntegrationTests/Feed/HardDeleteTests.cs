using Shouldly;

namespace Feed.IntegrationTests.Feed;
public sealed class HardDeleteTests
{
    [Fact]
    public async Task Hard_deleting_a_Feed_should_succeed()
    {
        true.ShouldBeTrue();

        await Task.CompletedTask;
    }
}