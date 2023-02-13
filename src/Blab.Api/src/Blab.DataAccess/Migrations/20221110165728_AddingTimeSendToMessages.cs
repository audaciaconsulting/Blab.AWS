using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blab.DataAccess.Migrations
{
    public partial class AddingTimeSendToMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<DateTime>(
                name: "Sent",
                table: "Messages",
                type: "datetime2",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sent",
                table: "Messages");

        }
    }
}
