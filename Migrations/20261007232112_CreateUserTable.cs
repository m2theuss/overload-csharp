using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace overload_csharp.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    First_name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Last_name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Birthdate = table.Column<string>(type: "NVARCHAR2(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
