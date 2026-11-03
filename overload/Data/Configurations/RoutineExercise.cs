using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace overload.Data;

public class OrderExerciseRoutineConfig : IEntityTypeConfiguration<RoutineExercise>
{
    public void Configure(EntityTypeBuilder<RoutineExercise> builder)
    {
        builder.HasKey(e => e.Id);
    }
}