using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNetCore3.Data.Migrations
{
    public partial class MigratioRenameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Nombre","_TCursos", "Curso");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
