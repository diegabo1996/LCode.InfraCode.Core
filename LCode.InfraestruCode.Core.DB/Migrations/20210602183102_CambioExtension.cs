using Microsoft.EntityFrameworkCore.Migrations;

namespace LCode.InfraestruCode.Core.BD.Migrations
{
    public partial class CambioExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Servidor",
                schema: "lcode",
                table: "SolicitudesServidores",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Servidor",
                schema: "lcode",
                table: "SolicitudesServidores",
                type: "varchar(5)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldMaxLength: 8);
        }
    }
}
