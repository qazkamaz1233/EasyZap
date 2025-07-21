using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyZap.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppointmentNotMappedMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Masters_MasterId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MasterId",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MasterId",
                table: "Appointments",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Masters_MasterId",
                table: "Appointments",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
