using Microsoft.EntityFrameworkCore;
namespace overload.Data.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using overload.Entities;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.IsPublic).IsRequired();
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.Muscle).IsRequired();
    }
}