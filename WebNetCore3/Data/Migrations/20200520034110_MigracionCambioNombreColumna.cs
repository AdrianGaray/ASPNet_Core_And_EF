using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNetCore3.Data.Migrations
{
    public partial class MigracionCambioNombreColumna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__TInscripcion__TCursos_CursoID",
                table: "_TInscripcion");

            migrationBuilder.DropColumn(
                name: "CategoriaID",
                table: "_TInscripcion");

            migrationBuilder.AlterColumn<int>(
                name: "CursoID",
                table: "_TInscripcion",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__TInscripcion__TCursos_CursoID",
                table: "_TInscripcion",
                column: "CursoID",
                principalTable: "_TCursos",
                principalColumn: "CursoID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__TInscripcion__TCursos_CursoID",
                table: "_TInscripcion");

            migrationBuilder.AlterColumn<int>(
                name: "CursoID",
                table: "_TInscripcion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CategoriaID",
                table: "_TInscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK__TInscripcion__TCursos_CursoID",
                table: "_TInscripcion",
                column: "CursoID",
                principalTable: "_TCursos",
                principalColumn: "CursoID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
