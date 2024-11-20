using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feed.Persistence;
internal sealed class FeedConfiguration : IEntityTypeConfiguration<Entity.Feed>
{
    public void Configure(EntityTypeBuilder<Entity.Feed> builder)
    {
        builder.ToTable("Feeds");

        builder.HasKey(e => e.Id);

        builder.Property(f => f.ImageUrl)
                .IsRequired(false);

        builder.Property(f => f.VideoUrl)
                .IsRequired(false);

        Seed(builder);
    }

    //Seeding the database with Feeds
    private static void Seed(EntityTypeBuilder<Entity.Feed> builder)
    {        
        builder.HasData
        (
            //new Entity.Feed()
            //{

            //}
        );
    }
}