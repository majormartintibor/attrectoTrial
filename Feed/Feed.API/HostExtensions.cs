using Feed.Persistence;

namespace Feed.API;

public static class HostExtensions
{
    public static IHost SeedDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            SeedData.Initialize(context);
        }
        return host;
    }
}