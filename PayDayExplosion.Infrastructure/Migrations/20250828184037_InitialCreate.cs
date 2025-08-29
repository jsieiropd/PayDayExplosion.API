using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayDayExplosion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpanDetailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpanDetailTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpanTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegularFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    OvertimeFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpanTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubspanDetailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubspanDetailTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkdayTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkdayTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkshiftTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegularFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    OvertimeFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshiftTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subspans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpanId = table.Column<int>(type: "int", nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTimeIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubTimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayCategoryId = table.Column<int>(type: "int", nullable: false),
                    WorkshiftTypeId = table.Column<int>(type: "int", nullable: false),
                    SpanDetailTypeId = table.Column<int>(type: "int", nullable: true),
                    Factor1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Factor2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Factor3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Factor4 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Substract = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClockedHours = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MealHours = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    IsInSchedule = table.Column<bool>(type: "bit", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubspanDetailTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subspans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subspans_PayCategories_PayCategoryId",
                        column: x => x.PayCategoryId,
                        principalTable: "PayCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subspans_SpanDetailTypes_SpanDetailTypeId",
                        column: x => x.SpanDetailTypeId,
                        principalTable: "SpanDetailTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subspans_SubspanDetailTypes_SubspanDetailTypeId",
                        column: x => x.SubspanDetailTypeId,
                        principalTable: "SubspanDetailTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subspans_WorkshiftTypes_WorkshiftTypeId",
                        column: x => x.WorkshiftTypeId,
                        principalTable: "WorkshiftTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubspanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubspanId = table.Column<int>(type: "int", nullable: false),
                    SubspanDetailTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubspanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubspanDetails_SubspanDetailTypes_SubspanDetailTypeId",
                        column: x => x.SubspanDetailTypeId,
                        principalTable: "SubspanDetailTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubspanDetails_Subspans_SubspanId",
                        column: x => x.SubspanId,
                        principalTable: "Subspans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubspanDetails_SubspanDetailTypeId",
                table: "SubspanDetails",
                column: "SubspanDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubspanDetails_SubspanId",
                table: "SubspanDetails",
                column: "SubspanId");

            migrationBuilder.CreateIndex(
                name: "IX_Subspans_PayCategoryId",
                table: "Subspans",
                column: "PayCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subspans_SpanDetailTypeId",
                table: "Subspans",
                column: "SpanDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subspans_SubspanDetailTypeId",
                table: "Subspans",
                column: "SubspanDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subspans_WorkshiftTypeId",
                table: "Subspans",
                column: "WorkshiftTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");

            migrationBuilder.DropTable(
                name: "PayTypes");

            migrationBuilder.DropTable(
                name: "SpanTypes");

            migrationBuilder.DropTable(
                name: "SubspanDetails");

            migrationBuilder.DropTable(
                name: "WorkdayTypes");

            migrationBuilder.DropTable(
                name: "Subspans");

            migrationBuilder.DropTable(
                name: "PayCategories");

            migrationBuilder.DropTable(
                name: "SpanDetailTypes");

            migrationBuilder.DropTable(
                name: "SubspanDetailTypes");

            migrationBuilder.DropTable(
                name: "WorkshiftTypes");
        }
    }
}
