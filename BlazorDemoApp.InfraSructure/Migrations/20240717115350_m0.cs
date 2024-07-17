using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorDemoApp.InfraSructure.Migrations
{
    /// <inheritdoc />
    public partial class m0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyAppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q1 = table.Column<short>(type: "smallint", nullable: true),
                    Ans1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q2 = table.Column<short>(type: "smallint", nullable: true),
                    Ans2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isEmp = table.Column<bool>(type: "bit", nullable: true),
                    isParent = table.Column<bool>(type: "bit", nullable: true),
                    isStd = table.Column<bool>(type: "bit", nullable: true),
                    isPasswordReset = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IDFK_MyAppUser = table.Column<int>(type: "int", nullable: true),
                    added_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    added_by = table.Column<int>(type: "int", nullable: true),
                    Modify_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modify_by = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyAppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyAppUsers_MyAppUsers_IDFK_MyAppUser",
                        column: x => x.IDFK_MyAppUser,
                        principalTable: "MyAppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Add0Govs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    added_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    added_by = table.Column<int>(type: "int", nullable: true),
                    Modify_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modify_by = table.Column<int>(type: "int", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add0Govs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Add0Govs_MyAppUsers_added_by",
                        column: x => x.added_by,
                        principalTable: "MyAppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Add1Markazs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    added_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    added_by = table.Column<int>(type: "int", nullable: true),
                    Modify_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modify_by = table.Column<int>(type: "int", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IdFK_Gov = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add1Markazs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Add1Markazs_Add0Govs_IdFK_Gov",
                        column: x => x.IdFK_Gov,
                        principalTable: "Add0Govs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Add1Markazs_MyAppUsers_added_by",
                        column: x => x.added_by,
                        principalTable: "MyAppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Add2Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    added_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    added_by = table.Column<int>(type: "int", nullable: true),
                    Modify_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modify_by = table.Column<int>(type: "int", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IdFK_Markaz = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add2Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Add2Cities_Add1Markazs_IdFK_Markaz",
                        column: x => x.IdFK_Markaz,
                        principalTable: "Add1Markazs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Add2Cities_MyAppUsers_added_by",
                        column: x => x.added_by,
                        principalTable: "MyAppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "MyAppUsers",
                columns: new[] { "Id", "Ans1", "Ans2", "IDFK_MyAppUser", "IsActive", "Modify_by", "Modify_date", "Q1", "Q2", "UserName", "UserPass", "added_by", "added_date", "isEmp", "isParent", "isPasswordReset", "isStd" },
                values: new object[,]
                {
                    { 1, "admin1", "admin2", 1, true, null, null, (short)1, (short)2, "admin", "admin@12345", null, null, true, null, false, null },
                    { 2, "emp1emp1", "emp11emp11", 2, true, null, null, (short)1, (short)2, "emp1", "emp1@12345", null, null, true, null, true, null },
                    { 3, "emp2emp2", "emp22emp22", 3, true, null, null, (short)1, (short)2, "emp2", "emp2@12345", null, null, true, null, false, null },
                    { 4, "parent", "parent1", 4, true, null, null, (short)1, (short)2, "Parent", "parent@12345", null, null, false, true, false, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Add0Govs_added_by",
                table: "Add0Govs",
                column: "added_by");

            migrationBuilder.CreateIndex(
                name: "IX_Add1Markazs_added_by",
                table: "Add1Markazs",
                column: "added_by");

            migrationBuilder.CreateIndex(
                name: "IX_Add1Markazs_IdFK_Gov",
                table: "Add1Markazs",
                column: "IdFK_Gov");

            migrationBuilder.CreateIndex(
                name: "IX_Add2Cities_added_by",
                table: "Add2Cities",
                column: "added_by");

            migrationBuilder.CreateIndex(
                name: "IX_Add2Cities_IdFK_Markaz",
                table: "Add2Cities",
                column: "IdFK_Markaz");

            migrationBuilder.CreateIndex(
                name: "IX_MyAppUsers_IDFK_MyAppUser",
                table: "MyAppUsers",
                column: "IDFK_MyAppUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Add2Cities");

            migrationBuilder.DropTable(
                name: "Add1Markazs");

            migrationBuilder.DropTable(
                name: "Add0Govs");

            migrationBuilder.DropTable(
                name: "MyAppUsers");
        }
    }
}
