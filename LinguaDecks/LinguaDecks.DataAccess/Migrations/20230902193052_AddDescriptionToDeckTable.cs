using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinguaDecks.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToDeckTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Deck",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ13Tyw1AbQyAk2A0bweycajIqj4etN0P5PjSnhPjulcExQuYY1U0p1nqBUx+ghzBA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECoNwNS9Eurwp0u+CC4DJ3gTewRugdIcv+Dbc86U1HSXrYZHpXQ4nCmtiDN3T81Uww==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGl0eum4+nXiUy3Z3Fzt3PdzMIJ8PDJYjb0VNDahu7PgznxV0FWRkuOAxZloyGUMMw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Deck");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEB9hC/GiS1WuK8Q7fySNCnbG+criB1SY7WKTgL/+Nz8Djos/MtTZU1rhlhYsJKfoCQ==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHjovE/pVWE0FCL6x/TMcjMmBjJ/R7xXBU0hNCgiSXRGdnIy7YwWSL/L9DChQajrlA==");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJtJrG+8JymrrH9jKMCTPQd9vKke67ovM0ZQhMD2OsIt0AekevlaUJLqnitVyJncbw==");
        }
    }
}
