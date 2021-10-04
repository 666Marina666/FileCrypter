using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class First2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RsaParameters_AspNetUsers_ApplicationUserId",
                table: "RsaParameters");

            migrationBuilder.DropIndex(
                name: "IX_RsaParameters_ApplicationUserId",
                table: "RsaParameters");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "RsaParameters");

            migrationBuilder.CreateTable(
                name: "ApplicationUserRsaParameters",
                columns: table => new
                {
                    OwnerUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RsaParametersPublicKey = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRsaParameters", x => new { x.OwnerUsersId, x.RsaParametersPublicKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRsaParameters_AspNetUsers_OwnerUsersId",
                        column: x => x.OwnerUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRsaParameters_RsaParameters_RsaParametersPublicKey",
                        column: x => x.RsaParametersPublicKey,
                        principalTable: "RsaParameters",
                        principalColumn: "PublicKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRsaParameters_RsaParametersPublicKey",
                table: "ApplicationUserRsaParameters",
                column: "RsaParametersPublicKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserRsaParameters");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "RsaParameters",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RsaParameters_ApplicationUserId",
                table: "RsaParameters",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RsaParameters_AspNetUsers_ApplicationUserId",
                table: "RsaParameters",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
