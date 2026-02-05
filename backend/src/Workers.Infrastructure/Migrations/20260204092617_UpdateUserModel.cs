using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_rooms_work_requests_work_request_id",
                table: "chat_rooms");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_rooms_work_requests_work_request_id",
                table: "chat_rooms",
                column: "work_request_id",
                principalTable: "work_requests",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_rooms_work_requests_work_request_id",
                table: "chat_rooms");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_rooms_work_requests_work_request_id",
                table: "chat_rooms",
                column: "work_request_id",
                principalTable: "work_requests",
                principalColumn: "id");
        }
    }
}
