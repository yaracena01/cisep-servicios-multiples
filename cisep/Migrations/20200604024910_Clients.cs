using Microsoft.EntityFrameworkCore.Migrations;

namespace cisep.Migrations
{
    public partial class Clients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(unicode: false, maxLength: 25, nullable: false),
                    last_name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    suffix = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    mi = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    username = table.Column<string>(unicode: false, maxLength: 25, nullable: false),
                    password = table.Column<string>(unicode: false, maxLength: 25, nullable: false),
                    address = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    city = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    state = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    zip = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    phone = table.Column<string>(unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });   
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
