using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace overload_csharp.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderExerciseInRoutine_RoutineExercise_RoutineId",
                table: "OrderExerciseInRoutine");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderExerciseInRoutine_exercises_ExerciseId",
                table: "OrderExerciseInRoutine");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderExerciseInRoutine_users_AuthorId",
                table: "OrderExerciseInRoutine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoutineExercise",
                table: "RoutineExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderExerciseInRoutine",
                table: "OrderExerciseInRoutine");

            migrationBuilder.RenameTable(
                name: "RoutineExercise",
                newName: "routine_exercises");

            migrationBuilder.RenameTable(
                name: "OrderExerciseInRoutine",
                newName: "order_exercise_routines");

            migrationBuilder.RenameIndex(
                name: "IX_OrderExerciseInRoutine_RoutineId",
                table: "order_exercise_routines",
                newName: "IX_order_exercise_routines_RoutineId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderExerciseInRoutine_ExerciseId",
                table: "order_exercise_routines",
                newName: "IX_order_exercise_routines_ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderExerciseInRoutine_AuthorId",
                table: "order_exercise_routines",
                newName: "IX_order_exercise_routines_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_routine_exercises",
                table: "routine_exercises",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_exercise_routines",
                table: "order_exercise_routines",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_exercise_routines_exercises_ExerciseId",
                table: "order_exercise_routines",
                column: "ExerciseId",
                principalTable: "exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_exercise_routines_routine_exercises_RoutineId",
                table: "order_exercise_routines",
                column: "RoutineId",
                principalTable: "routine_exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_exercise_routines_users_AuthorId",
                table: "order_exercise_routines",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_exercise_routines_exercises_ExerciseId",
                table: "order_exercise_routines");

            migrationBuilder.DropForeignKey(
                name: "FK_order_exercise_routines_routine_exercises_RoutineId",
                table: "order_exercise_routines");

            migrationBuilder.DropForeignKey(
                name: "FK_order_exercise_routines_users_AuthorId",
                table: "order_exercise_routines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_routine_exercises",
                table: "routine_exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_exercise_routines",
                table: "order_exercise_routines");

            migrationBuilder.RenameTable(
                name: "routine_exercises",
                newName: "RoutineExercise");

            migrationBuilder.RenameTable(
                name: "order_exercise_routines",
                newName: "OrderExerciseInRoutine");

            migrationBuilder.RenameIndex(
                name: "IX_order_exercise_routines_RoutineId",
                table: "OrderExerciseInRoutine",
                newName: "IX_OrderExerciseInRoutine_RoutineId");

            migrationBuilder.RenameIndex(
                name: "IX_order_exercise_routines_ExerciseId",
                table: "OrderExerciseInRoutine",
                newName: "IX_OrderExerciseInRoutine_ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_order_exercise_routines_AuthorId",
                table: "OrderExerciseInRoutine",
                newName: "IX_OrderExerciseInRoutine_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoutineExercise",
                table: "RoutineExercise",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderExerciseInRoutine",
                table: "OrderExerciseInRoutine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderExerciseInRoutine_RoutineExercise_RoutineId",
                table: "OrderExerciseInRoutine",
                column: "RoutineId",
                principalTable: "RoutineExercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderExerciseInRoutine_exercises_ExerciseId",
                table: "OrderExerciseInRoutine",
                column: "ExerciseId",
                principalTable: "exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderExerciseInRoutine_users_AuthorId",
                table: "OrderExerciseInRoutine",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
