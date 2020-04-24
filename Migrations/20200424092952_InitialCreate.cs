using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogIndex.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseAttributes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    AttributeName = table.Column<string>(nullable: true),
                    AttributeValue = table.Column<string>(nullable: true),
                    AttributeType = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GetUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(type: "varchar(200)", nullable: false),
                    FamilyName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true, computedColumnSql: "[FamilyName]+[LastName]"),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MxQuestionCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    CategoryName = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: false),
                    ParentName = table.Column<string>(nullable: true),
                    Hierarchy = table.Column<int>(nullable: false),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MxQuestionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MxQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Question = table.Column<string>(nullable: true),
                    QuestionType = table.Column<string>(nullable: false),
                    QuestionCate = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    Options = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Mx_QuestionCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MxQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MxQuestions_MxQuestionCategories_Mx_QuestionCategoryId",
                        column: x => x.Mx_QuestionCategoryId,
                        principalTable: "MxQuestionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MxQuestions_Mx_QuestionCategoryId",
                table: "MxQuestions",
                column: "Mx_QuestionCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseAttributes");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "MxQuestions");

            migrationBuilder.DropTable(
                name: "MxQuestionCategories");
        }
    }
}
