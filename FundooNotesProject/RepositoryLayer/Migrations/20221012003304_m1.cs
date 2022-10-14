using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTable",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTable", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "NoteTable",
                columns: table => new
                {
                    NoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReminderDate = table.Column<DateTime>(nullable: false),
                    BgColour = table.Column<string>(nullable: true),
                    IsArchieve = table.Column<bool>(nullable: false),
                    IsPin = table.Column<bool>(nullable: false),
                    IsTrash = table.Column<bool>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    EditedDateTime = table.Column<DateTime>(nullable: false),
                    userID = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTable", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_NoteTable_UserTable_userID",
                        column: x => x.userID,
                        principalTable: "UserTable",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollaboratorTable",
                columns: table => new
                {
                    CollaboratorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollaboratorEmail = table.Column<string>(nullable: true),
                    NoteId = table.Column<int>(nullable: false),
                    userID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorTable", x => x.CollaboratorId);
                    table.ForeignKey(
                        name: "FK_CollaboratorTable_NoteTable_NoteId",
                        column: x => x.NoteId,
                        principalTable: "NoteTable",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CollaboratorTable_UserTable_userID",
                        column: x => x.userID,
                        principalTable: "UserTable",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorTable_NoteId",
                table: "CollaboratorTable",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorTable_userID",
                table: "CollaboratorTable",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTable_userID",
                table: "NoteTable",  
                column: "userID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaboratorTable");

            migrationBuilder.DropTable(
                name: "NoteTable");

            migrationBuilder.DropTable(
                name: "UserTable");
        }
    }
}
