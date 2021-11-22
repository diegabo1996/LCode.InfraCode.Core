using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LCode.InfraestruCode.Core.BD.Migrations
{
    public partial class Servidores_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "lcode");

            migrationBuilder.CreateTable(
                name: "SolicitudesServidores",
                schema: "lcode",
                columns: table => new
                {
                    GuidSolicitud = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Servidor = table.Column<string>(type: "varchar(5)", nullable: false),
                    TipoSolicitud = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "varchar(150)", nullable: true),
                    UsuarioSolicita = table.Column<string>(type: "varchar(50)", nullable: true),
                    FechaHoraRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoSolicitud = table.Column<int>(type: "int", nullable: false),
                    Mensaje = table.Column<string>(type: "varchar(max)", nullable: true),
                    FechaHoraUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesServidores", x => x.GuidSolicitud);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudesServidores",
                schema: "lcode");
        }
    }
}
