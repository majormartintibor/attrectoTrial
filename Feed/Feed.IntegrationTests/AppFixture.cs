using Alba;
using Alba.Security;
using Oakton;
using Wolverine;

namespace Feed.IntegrationTests;
public sealed class AppFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; set; }

    public Task DisposeAsync()
    {
        if (Host != null)
        {
            return Host.DisposeAsync().AsTask();
        }

        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        OaktonEnvironment.AutoStartHost = true;

        Host = await AlbaHost.For<Program>(x =>
        {
            x.ConfigureServices(services =>
            {
                //just hinting at potential usage of message broker like RabbitMQ
                services.DisableAllExternalWolverineTransports();

                //Initialize Databse here with BaselineData
            });
        }, new AuthenticationStub());
    }
}