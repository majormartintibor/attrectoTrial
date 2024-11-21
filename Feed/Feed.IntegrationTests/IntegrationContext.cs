using Alba;
using Feed.Persistence;
using Microsoft.EntityFrameworkCore;
using Wolverine.Tracking;

namespace Feed.IntegrationTests;

//xUnit specific
[CollectionDefinition("integration")]
public class IntegrationCollection : ICollectionFixture<AppFixture>
{
}

[Collection("integration")]
public abstract class IntegrationContext(
    AppFixture fixture) : IAsyncLifetime
{
    private readonly AppFixture _fixture = fixture;

    public IAlbaHost Host => _fixture.Host;

    public virtual async Task InitializeAsync()
    {
        //This is a quick and dirty hack now, if I have time I will clean this up later
        //if not, it is not the end of the world :)
        using var context 
            = CreateApplicationDbContext("Host=localhost;port=5432;Database=FeedDb;Username=postgres;Password=postgres;persist security info=true;");

        //Delete all db sets here you want, can be used with LINQ.
        //The goal is to reset the database to the initial state after
        //BaseLinedata has been applied (not implemented yet)
        await context.UserFeedLikes.ExecuteDeleteAsync();
        await context.Feeds.ExecuteDeleteAsync();
        await context.Users.ExecuteDeleteAsync();

        //This is another option: it would delete and recreate the db
        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();        
    }

    public Task DisposeAsync()
    {
        //Required because of the IAsyncLifetime interface
        //I purposefully do not tear down the database after running the tests!
        return Task.CompletedTask;
    }

    //Delegating to Alba to run HTTP requests end to end
    public async Task<IScenarioResult> Scenario(Action<Scenario> configure)
    {
        return await Host.Scenario(configure);
    }

    // This method allows us to make HTTP calls into our system
    // in memory with Alba, but do so within Wolverine's test support
    // for message tracking to both record outgoing messages and to ensure
    // that any cascaded work spawned by the initial command is completed
    // before passing control back to the calling test
    protected async Task<(ITrackedSession, IScenarioResult)> TrackedHttpCall(Action<Scenario> configuration)
    {
        IScenarioResult result = null;

        // The outer part is tying into Wolverine's test support
        // to "wait" for all detected message activity to complete
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            // The inner part here is actually making an HTTP request
            // to the system under test with Alba
            result = await Host.Scenario(configuration);
        });

        return (tracked!, result!);
    }

    private static ApplicationDbContext CreateApplicationDbContext(string connectionstring)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionstring);
        optionsBuilder.EnableSensitiveDataLogging();
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}