using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "KPAssessment");

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "KPAssessment",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "KPAssessment",
                columns: table => new
                {
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "KPAssessment",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "GroupPermission",
                schema: "KPAssessment",
                columns: table => new
                {
                    GroupsGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionsPermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermission", x => new { x.GroupsGroupId, x.PermissionsPermissionId });
                    table.ForeignKey(
                        name: "FK_GroupPermission_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalSchema: "KPAssessment",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPermission_Permissions_PermissionsPermissionId",
                        column: x => x.PermissionsPermissionId,
                        principalSchema: "KPAssessment",
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                schema: "KPAssessment",
                columns: table => new
                {
                    GroupsGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsGroupId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalSchema: "KPAssessment",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalSchema: "KPAssessment",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "FirstName", "LastName" },
                values: new object[]
                    {
                        Guid.NewGuid(), "Marius", "Bierman"
                    });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "GroupId", "Name" },
                values: new object[,]
                    {
                        { Guid.NewGuid(), "Administrator" },
                        { Guid.NewGuid(), "Developer" },
                        { Guid.NewGuid(), "Tester" }
                    });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Name" },
                values: new object[,]
                    {
                        { Guid.NewGuid(), "Add" },
                        { Guid.NewGuid(), "Edit" },
                        { Guid.NewGuid(), "Update" },
                        { Guid.NewGuid(), "Delete" }
                    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupPermission",
                schema: "KPAssessment");

            migrationBuilder.DropTable(
                name: "GroupUser",
                schema: "KPAssessment");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "KPAssessment");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "KPAssessment");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "KPAssessment");
        }
    }
}
