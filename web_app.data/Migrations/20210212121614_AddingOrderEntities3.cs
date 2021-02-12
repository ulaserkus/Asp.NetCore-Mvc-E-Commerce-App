using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace web_app.data.Migrations
{
    public partial class AddingOrderEntities3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDare",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnumPaymentType",
                table: "Order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "EnumPaymentType",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Order");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDare",
                table: "Order",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
