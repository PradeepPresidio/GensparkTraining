using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patientsatients_PatientId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patientsatients",
                table: "Patientsatients");

            migrationBuilder.RenameTable(
                name: "Patientsatients",
                newName: "Patients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.RenameTable(
                name: "Patients",
                newName: "Patientsatients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patientsatients",
                table: "Patientsatients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patientsatients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patientsatients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
