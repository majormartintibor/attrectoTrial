using Oakton;
using Xunit.Abstractions;
using Xunit.Sdk;
 
[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestFramework("Feed.IntegrationTests.AssemblyFixture", "Feed.IntegrationTests")]
 
namespace Feed.IntegrationTests;

public sealed class AssemblyFixture : XunitTestFramework
{
    public AssemblyFixture(IMessageSink messageSink)
        : base(messageSink)
    {
        OaktonEnvironment.AutoStartHost = true;
    }
}