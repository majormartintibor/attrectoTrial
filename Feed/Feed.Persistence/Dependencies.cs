using Feed.Core.FeedDomain.Ports;
using Feed.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Feed.Persistence;
public static class Dependencies
{
    public static IServiceCollection UsePersistence(this IServiceCollection serviceCollection, string connectionString)
    {
        return serviceCollection
                .AddDbContextFactory<ApplicationDbContext>(opt => opt.UseNpgsql(connectionString))
                .AddDbContext<ApplicationDbContext>(ServiceLifetime.Transient)
                .AddTransient((s) => CreateApplicationDbContext(connectionString))
                .AddTransient<IFeedRepository, FeedRepository>();
    }

    private static ApplicationDbContext CreateApplicationDbContext(string connectionstring)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionstring);
        optionsBuilder.EnableSensitiveDataLogging();
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}