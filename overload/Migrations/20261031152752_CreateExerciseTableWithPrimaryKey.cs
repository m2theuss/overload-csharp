using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace overload_csharp.Migrations
{
    /// <inheritdoc />
    public partial class CreateExerciseTableWithPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoutineExercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineExercise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderExerciseInRoutine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    RoutineId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderExerciseInRoutine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderExerciseInRoutine_RoutineExercise_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "RoutineExercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderExerciseInRoutine_exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderExerciseInRoutine_users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderExerciseInRoutine_AuthorId",
                table: "OrderExerciseInRoutine",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderExerciseInRoutine_ExerciseId",
                table: "OrderExerciseInRoutine",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderExerciseInRoutine_RoutineId",
                table: "OrderExerciseInRoutine",
                column: "RoutineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderExerciseInRoutine");

            migrationBuilder.DropTable(
                name: "RoutineExercise");
        }
    }
}
