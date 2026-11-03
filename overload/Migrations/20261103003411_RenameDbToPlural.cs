using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace overload_csharp.Migrations
{
    /// <inheritdoc />
    public partial class RenameDbToPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_routine_users_AuthorId",
                table: "routine");

            migrationBuilder.DropForeignKey(
                name: "FK_routine_exercises_routine_RoutineId",
                table: "routine_exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_routine",
                table: "routine");

            migrationBuilder.RenameTable(
                name: "routine",
                newName: "routines");

            migrationBuilder.RenameIndex(
                name: "IX_routine_AuthorId",
                table: "routines",
                newName: "IX_routines_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_routines",
                table: "routines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_routine_exercises_routines_RoutineId",
                table: "routine_exercises",
                column: "RoutineId",
                principalTable: "routines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_routines_users_AuthorId",
                table: "routines",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_routine_exercises_routines_RoutineId",
                table: "routine_exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_routines_users_AuthorId",
                table: "routines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_routines",
                table: "routines");

            migrationBuilder.RenameTable(
                name: "routines",
                newName: "routine");

            migrationBuilder.RenameIndex(
                name: "IX_routines_AuthorId",
                table: "routine",
                newName: "IX_routine_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_routine",
                table: "routine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_routine_users_AuthorId",
                table: "routine",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_routine_exercises_routine_RoutineId",
                table: "routine_exercises",
                column: "RoutineId",
                principalTable: "routine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
