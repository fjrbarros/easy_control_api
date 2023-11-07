using EasyControl.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyControl.Api.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user").HasKey(p => p.Id);

            builder.Property(p => p.Name).HasColumnType("varchar").IsRequired();

            builder.Property(p => p.Email).HasColumnType("varchar").IsRequired();

            builder.Property(p => p.Password).HasColumnType("varchar").IsRequired();

            builder.Property(p => p.CreatedAt).HasColumnType("timestamp").IsRequired();

            builder.Property(p => p.UpdatedAt).HasColumnType("timestamp").IsRequired();

            builder.HasIndex(p => p.Email).IsUnique();
        }
    }
}
