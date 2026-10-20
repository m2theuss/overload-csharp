using Microsoft.EntityFrameworkCore;
namespace overload.Data.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using overload.Entities;

public class ExerciceConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        throw new NotImplementedException();
    }
}