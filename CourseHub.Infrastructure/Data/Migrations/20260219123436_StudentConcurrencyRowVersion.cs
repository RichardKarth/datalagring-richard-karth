using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseHub.Infrastructure.Data.Migrations
{
    //Fattade nada av vad som gjordes här, migrationen lades till och fick massa errors så chatgpt sa att jag skulle ändra här inne och det gick :)

    /// <inheritdoc />
    public partial class StudentConcurrencyRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concurrency",
                table: "Students");

            migrationBuilder.AddColumn<byte[]>(
                name: "Concurrency",
                table: "Students",
                type: "rowversion",
                rowVersion: true,
                nullable: false);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concurrency",
                table: "Students");

            migrationBuilder.AddColumn<byte[]>(
                name: "Concurrency",
                table: "Students",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
