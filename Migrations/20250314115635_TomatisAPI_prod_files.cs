using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TomatisCRM_API.Migrations
{
    /// <inheritdoc />
    public partial class TomatisAPI_prod_files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pliki",
                schema: "app",
                columns: table => new
                {
                    File_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File_KntId = table.Column<int>(type: "int", nullable: true),
                    File_WizId = table.Column<int>(type: "int", nullable: true),
                    File_Nazwa = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    File_GUID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    File_Path = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    File_FullPath = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pliki__0FFFC99601C9FF05", x => x.File_Id);
                    table.ForeignKey(
                        name: "FK_Pliki_Knt",
                        column: x => x.File_KntId,
                        principalSchema: "app",
                        principalTable: "Klient",
                        principalColumn: "knt_id");
                    table.ForeignKey(
                        name: "FK_Pliki_Wiz",
                        column: x => x.File_WizId,
                        principalSchema: "app",
                        principalTable: "Wizyty",
                        principalColumn: "wiz_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pliki_File_KntId",
                schema: "app",
                table: "Pliki",
                column: "File_KntId");

            migrationBuilder.CreateIndex(
                name: "IX_Pliki_File_WizId",
                schema: "app",
                table: "Pliki",
                column: "File_WizId");

            migrationBuilder.CreateIndex(
                name: "UQ__Pliki__12E9E40437197B5A",
                schema: "app",
                table: "Pliki",
                column: "File_GUID",
                unique: true,
                filter: "[File_GUID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pliki",
                schema: "app");
        }
    }
}
