using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Migrations
{
    /// <inheritdoc />
    public partial class PTP1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Journeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: false),
                    Nights = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Journeys_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Journeys_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "USA" },
                    { 2, "Viet Nam" },
                    { 3, "Switzerland" }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "CHF" },
                    { 2, "USD" },
                    { 3, "VND" }
                });

            migrationBuilder.InsertData(
                table: "Journeys",
                columns: new[] { "Id", "Amount", "CountryId", "CurrencyId", "Days", "Description", "EndDate", "Name", "Nights", "PlaceId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 5000000, 2, 3, 5, "A trip with company at ...", new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Local), "Company Trip", 4, "3", new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Local), "Planning" },
                    { 2, 4000000, 2, 3, 5, "Đi chill cùng ae ...", new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Local), "Đà Lạt Trip", 4, "3,4", new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Local), "Planning" },
                    { 3, 4000000, 2, 3, 5, "Đi chill cùng ae ...", new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Local), "Đà Lạt Trip2", 4, "3,4", new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Local), "Planning" },
                    { 4, 4000000, 2, 3, 5, "Đi chill cùng ae ...", new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Local), "Đà Lạt Trip3", 4, "3", new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Local), "Planning" },
                    { 5, 4000000, 2, 3, 5, "Đi chill cùng ae ...", new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Local), "Đà Lạt Trip4", 4, "3,4", new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Local), "Planning" },
                    { 6, 4000000, 2, 3, 5, "Đi chill cùng ae ...", new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Local), "Đà Lạt Trip5", 4, "4", new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Local), "Planning" }
                });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Arizona" },
                    { 2, 1, "California" },
                    { 3, 2, "Đà Nẵng" },
                    { 4, 2, "Hà Nội" },
                    { 5, 3, "Berne" },
                    { 6, 3, "Aargau" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_CountryId",
                table: "Journeys",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_CurrencyId",
                table: "Journeys",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_CountryId",
                table: "Places",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Journeys");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
