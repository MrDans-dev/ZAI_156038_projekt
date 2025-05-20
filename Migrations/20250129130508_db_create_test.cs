using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TomatisCRM_API.Migrations
{
    /// <inheritdoc />
    public partial class db_create_test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "AppConf",
                schema: "app",
                columns: table => new
                {
                    sconf_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sconf_nazwa = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    sconf_wartoscS = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    sconf_wartoscD = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    sconf_WartoscDt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysConf__9227CE49AE830300", x => x.sconf_id);
                });

            migrationBuilder.CreateTable(
                name: "Operatorzy",
                schema: "app",
                columns: table => new
                {
                    ope_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ope_login = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ope_haslo = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ope_nazwa = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    ope_email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ope_googleapikey = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ope_dataUtw = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ope_IsAdmin = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Operator__73A7AA01F34DC888", x => x.ope_id);
                });

            migrationBuilder.CreateTable(
                name: "Zadania",
                schema: "app",
                columns: table => new
                {
                    zad_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    zad_nazwa = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    zad_opis = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    zad_dataUkonczenia = table.Column<DateTime>(type: "datetime", nullable: true),
                    zad_opeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Zadania__AAB64147E26E387D", x => x.zad_id);
                });

            migrationBuilder.CreateTable(
                name: "Slowniki",
                schema: "app",
                columns: table => new
                {
                    slw_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    slw_typ = table.Column<int>(type: "int", nullable: true),
                    slw_wartoscS = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    slw_wartoscD = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    slw_opeUtw = table.Column<int>(type: "int", nullable: true),
                    slw_DataUtw = table.Column<DateTime>(type: "datetime", nullable: true),
                    slw_opeMod = table.Column<int>(type: "int", nullable: true),
                    slw_DataMod = table.Column<DateTime>(type: "datetime", nullable: true),
                    slw_nextSlw = table.Column<int>(type: "int", nullable: true),
                    OpeNazwa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Slowniki__37D060C973C4146B", x => x.slw_id);
                    table.ForeignKey(
                        name: "FK__Slowniki__slw_op__47DBAE45",
                        column: x => x.slw_opeUtw,
                        principalSchema: "app",
                        principalTable: "Operatorzy",
                        principalColumn: "ope_id");
                });

            migrationBuilder.CreateTable(
                name: "Klient",
                schema: "app",
                columns: table => new
                {
                    knt_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    knt_akronim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    knt_nazwa = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    knt_tel = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    knt_email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    knt_dataUtw = table.Column<DateTime>(type: "datetime", nullable: true),
                    knt_opeUtw = table.Column<int>(type: "int", nullable: true),
                    knt_opeMod = table.Column<int>(type: "int", nullable: true),
                    knt_dataMod = table.Column<DateTime>(type: "datetime", nullable: true),
                    knt_dataUrodzenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    knt_StatusSlw = table.Column<int>(type: "int", nullable: true),
                    knt_stacjonarny = table.Column<bool>(type: "bit", nullable: true),
                    knt_opis = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    knt_doKontaktu = table.Column<bool>(type: "bit", nullable: true),
                    knt_dataKontaktu = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Klient__id", x => x.knt_id);
                    table.ForeignKey(
                        name: "FK_Klient_Slowniki",
                        column: x => x.knt_StatusSlw,
                        principalSchema: "app",
                        principalTable: "Slowniki",
                        principalColumn: "slw_id");
                    table.ForeignKey(
                        name: "FK__Klient__knt_opeM__ope_id",
                        column: x => x.knt_opeMod,
                        principalSchema: "app",
                        principalTable: "Operatorzy",
                        principalColumn: "ope_id");
                    table.ForeignKey(
                        name: "FK__Klient__knt_opeU__ope_id",
                        column: x => x.knt_opeUtw,
                        principalSchema: "app",
                        principalTable: "Operatorzy",
                        principalColumn: "ope_id");
                });

            migrationBuilder.CreateTable(
                name: "Wizyty",
                schema: "app",
                columns: table => new
                {
                    wiz_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    wiz_typ = table.Column<int>(type: "int", nullable: true),
                    wiz_kntid = table.Column<int>(type: "int", nullable: true),
                    wiz_dataStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    wiz_dataKoniec = table.Column<DateTime>(type: "datetime", nullable: true),
                    wiz_opeUtw = table.Column<int>(type: "int", nullable: true),
                    wiz_dataUtw = table.Column<DateTime>(type: "datetime", nullable: true),
                    wiz_opeMod = table.Column<int>(type: "int", nullable: true),
                    wiz_dataMod = table.Column<DateTime>(type: "datetime", nullable: true),
                    wiz_opis = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    wiz_googlesync = table.Column<bool>(type: "bit", nullable: true),
                    wiz_poprzedniawizID = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wizyty__wiz_id", x => x.wiz_id);
                    table.ForeignKey(
                        name: "FK__Wizyty__wiz_knti__knt_id",
                        column: x => x.wiz_kntid,
                        principalSchema: "app",
                        principalTable: "Klient",
                        principalColumn: "knt_id");
                    table.ForeignKey(
                        name: "FK__Wizyty__wiz_opeM__ope_id",
                        column: x => x.wiz_opeMod,
                        principalSchema: "app",
                        principalTable: "Operatorzy",
                        principalColumn: "ope_id");
                    table.ForeignKey(
                        name: "FK__Wizyty__wiz_opeU__ope_id",
                        column: x => x.wiz_opeUtw,
                        principalSchema: "app",
                        principalTable: "Operatorzy",
                        principalColumn: "ope_id");
                    table.ForeignKey(
                        name: "FK__Wizyty__wiz_typ__slw_id",
                        column: x => x.wiz_typ,
                        principalSchema: "app",
                        principalTable: "Slowniki",
                        principalColumn: "slw_id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__SysConf__E92B942231ECCE06",
                schema: "app",
                table: "AppConf",
                column: "sconf_nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Klient_knt_opeMod",
                schema: "app",
                table: "Klient",
                column: "knt_opeMod");

            migrationBuilder.CreateIndex(
                name: "IX_Klient_knt_opeUtw",
                schema: "app",
                table: "Klient",
                column: "knt_opeUtw");

            migrationBuilder.CreateIndex(
                name: "IX_Klient_knt_StatusSlw",
                schema: "app",
                table: "Klient",
                column: "knt_StatusSlw");

            migrationBuilder.CreateIndex(
                name: "UQ__Klient__A0B2EA6EFF1B567E",
                schema: "app",
                table: "Klient",
                column: "knt_akronim",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Slowniki_slw_opeUtw",
                schema: "app",
                table: "Slowniki",
                column: "slw_opeUtw");

            migrationBuilder.CreateIndex(
                name: "UQ__Slowniki__534547542580D551",
                schema: "app",
                table: "Slowniki",
                column: "slw_wartoscS",
                unique: true,
                filter: "[slw_wartoscS] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyty_wiz_kntid",
                schema: "app",
                table: "Wizyty",
                column: "wiz_kntid");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyty_wiz_opeMod",
                schema: "app",
                table: "Wizyty",
                column: "wiz_opeMod");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyty_wiz_opeUtw",
                schema: "app",
                table: "Wizyty",
                column: "wiz_opeUtw");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyty_wiz_typ",
                schema: "app",
                table: "Wizyty",
                column: "wiz_typ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConf",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Wizyty",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Zadania",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Klient",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Slowniki",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Operatorzy",
                schema: "app");
        }
    }
}
