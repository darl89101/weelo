using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weelo.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    IdOwner = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.IdOwner);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    IdProperty = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CodeInternal = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IdOwner = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.IdProperty);
                    table.ForeignKey(
                        name: "FK_Properties_Owners_IdOwner",
                        column: x => x.IdOwner,
                        principalTable: "Owners",
                        principalColumn: "IdOwner");
                });

            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    IdPropertyImage = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProperty = table.Column<long>(type: "bigint", nullable: false),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImages", x => x.IdPropertyImage);
                    table.ForeignKey(
                        name: "FK_PropertyImages_Properties_IdProperty",
                        column: x => x.IdProperty,
                        principalTable: "Properties",
                        principalColumn: "IdProperty");
                });

            migrationBuilder.CreateTable(
                name: "PropertyTraces",
                columns: table => new
                {
                    IdPropertyTrace = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSale = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdProperty = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTraces", x => x.IdPropertyTrace);
                    table.ForeignKey(
                        name: "FK_PropertyTraces_Properties_IdProperty",
                        column: x => x.IdProperty,
                        principalTable: "Properties",
                        principalColumn: "IdProperty");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Owners_DocumentNumber",
                table: "Owners",
                column: "DocumentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_IdOwner",
                table: "Properties",
                column: "IdOwner");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_IdProperty",
                table: "PropertyImages",
                column: "IdProperty");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTraces_IdProperty",
                table: "PropertyTraces",
                column: "IdProperty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "PropertyTraces");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
