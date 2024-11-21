using Feed.Persistence.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feed.Persistence.Configurations;
internal sealed class UserFeedLikeConfiguration : IEntityTypeConfiguration<UserFeedLike>
{
    public void Configure(EntityTypeBuilder<UserFeedLike> builder)
    {
        builder.ToTable("UserFeedLikes");

        builder.HasKey(uf => new { uf.UserId, uf.FeedId });

        builder.HasOne(uf => uf.User)
            .WithMany(u => u.UserFeedLikes)
            .HasForeignKey(uf => uf.UserId);

        builder.HasOne(uf => uf.Feed)
            .WithMany(f => f.UserFeedLikes)
            .HasForeignKey(uf => uf.FeedId);        
    }    
}