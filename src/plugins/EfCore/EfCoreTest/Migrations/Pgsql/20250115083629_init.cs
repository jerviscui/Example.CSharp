using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EfCoreTest.Migrations.Pgsql
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fact_sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    date_id = table.Column<int>(type: "integer", nullable: true),
                    product_id = table.Column<int>(type: "integer", nullable: true),
                    store_id = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(7,2)", nullable: true),
                    other_data = table.Column<string>(type: "char(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    OldFamilyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Families_Families_OldFamilyId",
                        column: x => x.OldFamilyId,
                        principalTable: "Families",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MssqlRowVersions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MssqlRowVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MysqlRowVersions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RowVersion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MysqlRowVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Buyer = table.Column<string>(type: "text", nullable: false),
                    StreetAddress_Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StreetAddress_City = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PgsqlRowVersions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PgsqlRowVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "SplitOrder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    BillingAddress = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlogId = table.Column<long>(type: "bigint", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    TagId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogTags_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Decimal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    FamilyId = table.Column<long>(type: "bigint", nullable: false),
                    Long = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TeacherId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persons_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Content", "IsDelete", "Title" },
                values: new object[] { 1L, "bbbb", false, "aaaa" });

            migrationBuilder.InsertData(
                table: "Families",
                columns: new[] { "Id", "Address", "OldFamilyId" },
                values: new object[] { 1L, "address", null });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Content", "Title" },
                values: new object[,]
                {
                    { 1, "content1", "title1" },
                    { 2, "content2", "title2" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                column: "TagId",
                values: new object[]
                {
                    "tag1",
                    "tag2"
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2L, "teacher" });

            migrationBuilder.InsertData(
                table: "BlogTags",
                columns: new[] { "Id", "BlogId", "IsDelete", "TagId" },
                values: new object[,]
                {
                    { 1L, 1L, false, "tag1" },
                    { 2L, 1L, false, "tag2" }
                });

            migrationBuilder.InsertData(
                table: "Families",
                columns: new[] { "Id", "Address", "OldFamilyId" },
                values: new object[] { 2L, "address", 1L });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "FamilyId", "Name", "TeacherId" },
                values: new object[] { 1L, 1L, "name", null });

            migrationBuilder.InsertData(
                table: "PostTag",
                columns: new[] { "PostId", "TagId" },
                values: new object[,]
                {
                    { 1, "tag1" },
                    { 1, "tag2" },
                    { 2, "tag1" },
                    { 2, "tag2" }
                });

            migrationBuilder.InsertData(
                table: "Families",
                columns: new[] { "Id", "Address", "OldFamilyId" },
                values: new object[] { 3L, "address", 2L });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Decimal", "FamilyId", "Long", "Name", "TeacherId" },
                values: new object[] { 2L, 22.22m, 2L, 11L, "name", 2L });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "FamilyId", "Name", "TeacherId" },
                values: new object[,]
                {
                    { 100L, 2L, "name0", null },
                    { 101L, 2L, "name1", null },
                    { 102L, 2L, "name2", null },
                    { 103L, 2L, "name3", null },
                    { 104L, 2L, "name4", null },
                    { 105L, 2L, "name5", null },
                    { 106L, 2L, "name6", null },
                    { 107L, 2L, "name7", null },
                    { 108L, 2L, "name8", null },
                    { 109L, 2L, "name9", null },
                    { 110L, 2L, "name10", null },
                    { 111L, 2L, "name11", null },
                    { 112L, 2L, "name12", null },
                    { 113L, 2L, "name13", null },
                    { 114L, 2L, "name14", null },
                    { 115L, 2L, "name15", null },
                    { 116L, 2L, "name16", null },
                    { 117L, 2L, "name17", null },
                    { 118L, 2L, "name18", null },
                    { 119L, 2L, "name19", null },
                    { 120L, 2L, "name20", null },
                    { 121L, 2L, "name21", null },
                    { 122L, 2L, "name22", null },
                    { 123L, 2L, "name23", null },
                    { 124L, 2L, "name24", null },
                    { 125L, 2L, "name25", null },
                    { 126L, 2L, "name26", null },
                    { 127L, 2L, "name27", null },
                    { 128L, 2L, "name28", null },
                    { 129L, 2L, "name29", null },
                    { 130L, 2L, "name30", null },
                    { 131L, 2L, "name31", null },
                    { 132L, 2L, "name32", null },
                    { 133L, 2L, "name33", null },
                    { 134L, 2L, "name34", null },
                    { 135L, 2L, "name35", null },
                    { 136L, 2L, "name36", null },
                    { 137L, 2L, "name37", null },
                    { 138L, 2L, "name38", null },
                    { 139L, 2L, "name39", null },
                    { 140L, 2L, "name40", null },
                    { 141L, 2L, "name41", null },
                    { 142L, 2L, "name42", null },
                    { 143L, 2L, "name43", null },
                    { 144L, 2L, "name44", null },
                    { 145L, 2L, "name45", null },
                    { 146L, 2L, "name46", null },
                    { 147L, 2L, "name47", null },
                    { 148L, 2L, "name48", null },
                    { 149L, 2L, "name49", null },
                    { 150L, 2L, "name50", null },
                    { 151L, 2L, "name51", null },
                    { 152L, 2L, "name52", null },
                    { 153L, 2L, "name53", null },
                    { 154L, 2L, "name54", null },
                    { 155L, 2L, "name55", null },
                    { 156L, 2L, "name56", null },
                    { 157L, 2L, "name57", null },
                    { 158L, 2L, "name58", null },
                    { 159L, 2L, "name59", null },
                    { 160L, 2L, "name60", null },
                    { 161L, 2L, "name61", null },
                    { 162L, 2L, "name62", null },
                    { 163L, 2L, "name63", null },
                    { 164L, 2L, "name64", null },
                    { 165L, 2L, "name65", null },
                    { 166L, 2L, "name66", null },
                    { 167L, 2L, "name67", null },
                    { 168L, 2L, "name68", null },
                    { 169L, 2L, "name69", null },
                    { 170L, 2L, "name70", null },
                    { 171L, 2L, "name71", null },
                    { 172L, 2L, "name72", null },
                    { 173L, 2L, "name73", null },
                    { 174L, 2L, "name74", null },
                    { 175L, 2L, "name75", null },
                    { 176L, 2L, "name76", null },
                    { 177L, 2L, "name77", null },
                    { 178L, 2L, "name78", null },
                    { 179L, 2L, "name79", null },
                    { 180L, 2L, "name80", null },
                    { 181L, 2L, "name81", null },
                    { 182L, 2L, "name82", null },
                    { 183L, 2L, "name83", null },
                    { 184L, 2L, "name84", null },
                    { 185L, 2L, "name85", null },
                    { 186L, 2L, "name86", null },
                    { 187L, 2L, "name87", null },
                    { 188L, 2L, "name88", null },
                    { 189L, 2L, "name89", null },
                    { 190L, 2L, "name90", null },
                    { 191L, 2L, "name91", null },
                    { 192L, 2L, "name92", null },
                    { 193L, 2L, "name93", null },
                    { 194L, 2L, "name94", null },
                    { 195L, 2L, "name95", null },
                    { 196L, 2L, "name96", null },
                    { 197L, 2L, "name97", null },
                    { 198L, 2L, "name98", null },
                    { 199L, 2L, "name99", null }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Decimal", "FamilyId", "Long", "Name", "TeacherId" },
                values: new object[] { 3L, 22.22m, 3L, 11L, "name", 2L });

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_BlogId",
                table: "BlogTags",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "ci",
                table: "fact_sales",
                column: "date_id");

            migrationBuilder.CreateIndex(
                name: "IX_Families_OldFamilyId",
                table: "Families",
                column: "OldFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StreetAddress_City",
                table: "Orders",
                column: "StreetAddress_City");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StreetAddress_Street",
                table: "Orders",
                column: "StreetAddress_Street");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_FamilyId",
                table: "Persons",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_TeacherId",
                table: "Persons",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                table: "PostTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTags");

            migrationBuilder.DropTable(
                name: "fact_sales");

            migrationBuilder.DropTable(
                name: "MssqlRowVersions");

            migrationBuilder.DropTable(
                name: "MysqlRowVersions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "PgsqlRowVersions");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "SplitOrder");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
