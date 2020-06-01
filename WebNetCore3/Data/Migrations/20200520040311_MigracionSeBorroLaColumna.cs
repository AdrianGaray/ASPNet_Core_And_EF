using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNetCore3.Data.Migrations
{
    public partial class MigracionSeBorroLaColumna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__TInscripcion__TCursos_CursoID",
                table: "_TInscripcion");

            migrationBuilder.AlterColumn<int>(
                name: "CursoID",
                table: "_TInscripcion",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK__TInscripcion__TCursos_CursoID",
                table: "_TInscripcion",
                column: "CursoID",
                principalTable: "_TCursos",
                principalColumn: "CursoID",
                onDelete: ReferentialAction.Restrict);
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
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__TInscripcion__TCursos_CursoID",
                table: "_TInscripcion",
                column: "CursoID",
                principalTable: "_TCursos",
                principalColumn: "CursoID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
