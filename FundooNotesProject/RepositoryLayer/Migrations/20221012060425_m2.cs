using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabelTable",
                columns: table => new
                {
                    LabelID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabelName = table.Column<string>(nullable: true),
                    NoteId = table.Column<int>(nullable: false),
                    userID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelTable", x => x.LabelID);
                    table.ForeignKey(
                        name: "FK_LabelTable_NoteTable_NoteId",
                        column: x => x.NoteId,
                        principalTable: "NoteTable",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabelTable_UserTable_userID",
                        column: x => x.userID,
                        principalTable: "UserTable",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelTable_NoteId",
                table: "LabelTable",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelTable_userID",
                table: "LabelTable",
                column: "userID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelTable");
        }
    }
}
