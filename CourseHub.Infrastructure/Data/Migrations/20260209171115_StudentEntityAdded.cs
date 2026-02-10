using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class StudentEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true),
                    Concurrency = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Students_CreatedAtUtc"),
                    ModifiedAtUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(SYSUTCDATETIME())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Students_ModifiedAtUtc")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students_Id", x => x.Id);
                    table.CheckConstraint("CK_Student_Email_NotEmpty", "LTRIM(RTRIM([Email])) <> ''");
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Students_Email",
                table: "Student",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
