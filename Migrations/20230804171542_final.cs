using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "Users",
                newName: "Salt");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiverUserName = table.Column<string>(type: "TEXT", nullable: true),
                    SenderUserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ReceiverUserName",
                        column: x => x.ReceiverUserName,
                        principalTable: "Users",
                        principalColumn: "UserName");
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderUserName",
                        column: x => x.SenderUserName,
                        principalTable: "Users",
                        principalColumn: "UserName");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverUserName",
                table: "Messages",
                column: "ReceiverUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderUserName",
                table: "Messages",
                column: "SenderUserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "Users",
                newName: "Dob");
        }
    }
}
