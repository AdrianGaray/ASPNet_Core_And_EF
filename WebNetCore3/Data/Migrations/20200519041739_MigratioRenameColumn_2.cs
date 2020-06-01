using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNetCore3.Data.Migrations
{
    public partial class MigratioRenameColumn_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Descripcion", "_TCursos", "Informacion");
            migrationBuilder.RenameColumn("Nombre", "_TCategoria", "Categoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
