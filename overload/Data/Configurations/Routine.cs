using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using overload.Entities;

namespace overload.Data;

public class RoutineExerciseConfiguration : IEntityTypeConfiguration<Routine>
{
    public void Configure(EntityTypeBuilder<Routine> builder)
    {
        builder.HasKey(e => e.Id);
        
    }
}