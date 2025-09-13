using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyZap.Migrations
{
    /// <inheritdoc />
    public partial class AddOnModelCreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MasterLinks_Token",
                table: "MasterLinks",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MasterLinks_Token",
                table: "MasterLinks");
        }
    }
}
