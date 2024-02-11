using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TOMS.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketOrderedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RouteId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassengerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatRow = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketOrderTransaction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TnxNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentTypeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassengerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfTickets = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScreenShootUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketOrderTransaction", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "TicketOrderTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "NumberOfSeat",
                table: "BusLine",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
