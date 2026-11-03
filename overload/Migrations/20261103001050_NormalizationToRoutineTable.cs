using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace overload_csharp.Migrations
{
    /// <inheritdoc />
    public partial class NormalizationToRoutineTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_routine_exercises_users_AuthorId",
                table: "routine_exercises");

            migrationBuilder.DropIndex(
                name: "IX_routine_exercises_AuthorId",
                table: "routine_exercises");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "routine_exercises");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "routine",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_routine_AuthorId",
                table: "routine",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_routine_users_AuthorId",
                table: "routine",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_routine_users_AuthorId",
                table: "routine");

            migrationBuilder.DropIndex(
                name: "IX_routine_AuthorId",
                table: "routine");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "routine");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "routine_exercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_routine_exercises_AuthorId",
                table: "routine_exercises",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_routine_exercises_users_AuthorId",
                table: "routine_exercises",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
