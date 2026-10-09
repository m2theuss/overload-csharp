using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using overload.Entities;

namespace overload.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
         builder.HasKey(p => p.Id);

        builder.Property(p => p.First_name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Last_name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Password)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(p => p.Birthdate)
            .IsRequired();
    }
}