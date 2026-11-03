using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace overload_csharp.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToBeMoreDeclarative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_exercise_routines");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "routine_exercises");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "routine_exercises");

            migrationBuilder.RenameColumn(
                name: "Day",
                table: "routine_exercises",
                newName: "RoutineId");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "routine_exercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "routine_exercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "routine_exercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "routine",
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
                    table.PrimaryKey("PK_routine", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_routine_exercises_AuthorId",
                table: "routine_exercises",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_routine_exercises_ExerciseId",
                table: "routine_exercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_routine_exercises_RoutineId",
                table: "routine_exercises",
                column: "RoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_routine_exercises_exercises_ExerciseId",
                table: "routine_exercises",
                column: "ExerciseId",
                principalTable: "exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_routine_exercises_routine_RoutineId",
                table: "routine_exercises",
                column: "RoutineId",
                principalTable: "routine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_routine_exercises_users_AuthorId",
                table: "routine_exercises",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_routine_exercises_exercises_ExerciseId",
                table: "routine_exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_routine_exercises_routine_RoutineId",
                table: "routine_exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_routine_exercises_users_AuthorId",
                table: "routine_exercises");

            migrationBuilder.DropTable(
                name: "routine");

            migrationBuilder.DropIndex(
                name: "IX_routine_exercises_AuthorId",
                table: "routine_exercises");

            migrationBuilder.DropIndex(
                name: "IX_routine_exercises_ExerciseId",
                table: "routine_exercises");

            migrationBuilder.DropIndex(
                name: "IX_routine_exercises_RoutineId",
                table: "routine_exercises");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "routine_exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "routine_exercises");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "routine_exercises");

            migrationBuilder.RenameColumn(
                name: "RoutineId",
                table: "routine_exercises",
                newName: "Day");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "routine_exercises",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "routine_exercises",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "order_exercise_routines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    RoutineId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_exercise_routines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_exercise_routines_exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_exercise_routines_routine_exercises_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "routine_exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_exercise_routines_users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_exercise_routines_AuthorId",
                table: "order_exercise_routines",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_order_exercise_routines_ExerciseId",
                table: "order_exercise_routines",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_order_exercise_routines_RoutineId",
                table: "order_exercise_routines",
                column: "RoutineId");
        }
    }
}
