using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlatformaBrowaru.Data.Migrations
{
    public partial class initialMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrewingMethods",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Method = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewingMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FermentationTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FermentationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(nullable: false),
                    TokenExpirationDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wrappings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wrappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrewingGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedAt = table.Column<DateTime>(nullable: false),
                    AddedById = table.Column<long>(nullable: true),
                    Adress = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DeletionReason = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: false),
                    EditedById = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrewingGroups_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrewingGroups_Users_EditedById",
                        column: x => x.EditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kinds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedAt = table.Column<DateTime>(nullable: false),
                    AddedById = table.Column<long>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    EditedById = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kinds_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kinds_Users_EditedById",
                        column: x => x.EditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Breweries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedAt = table.Column<DateTime>(nullable: false),
                    AddedById = table.Column<long>(nullable: true),
                    Adress = table.Column<string>(nullable: true),
                    AnnualProduction = table.Column<decimal>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DeletionReason = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: false),
                    EditedById = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breweries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breweries_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Breweries_Users_EditedById",
                        column: x => x.EditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Breweries_BrewingGroups_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BrewingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrandProductions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedAt = table.Column<DateTime>(nullable: false),
                    AddedById = table.Column<long>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    EditedById = table.Column<long>(nullable: true),
                    ProducedById = table.Column<long>(nullable: true),
                    ProducedFrom = table.Column<DateTime>(nullable: false),
                    ProducedTo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandProductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandProductions_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrandProductions_Users_EditedById",
                        column: x => x.EditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrandProductions_Breweries_ProducedById",
                        column: x => x.ProducedById,
                        principalTable: "Breweries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedAt = table.Column<DateTime>(nullable: false),
                    AddedById = table.Column<long>(nullable: true),
                    AlcoholAmountPercent = table.Column<decimal>(nullable: false),
                    BrandProductionId = table.Column<long>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<long>(nullable: true),
                    DeletionReason = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    EditedById = table.Column<long>(nullable: true),
                    ExtractPercent = table.Column<decimal>(nullable: false),
                    HopIntensity = table.Column<int>(nullable: false),
                    Ingredients = table.Column<string>(nullable: true),
                    IsAccepted = table.Column<bool>(nullable: false),
                    IsFiltered = table.Column<bool>(nullable: false),
                    IsPasteurized = table.Column<bool>(nullable: false),
                    KindId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Sweetness = table.Column<int>(nullable: false),
                    TasteFullness = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brands_Users_AddedById",
                        column: x => x.AddedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brands_BrandProductions_BrandProductionId",
                        column: x => x.BrandProductionId,
                        principalTable: "BrandProductions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brands_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brands_Users_EditedById",
                        column: x => x.EditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brands_Kinds_KindId",
                        column: x => x.KindId,
                        principalTable: "Kinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrandBrewingMethods",
                columns: table => new
                {
                    BrandId = table.Column<long>(nullable: false),
                    BrewingMethodId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandBrewingMethods", x => new { x.BrandId, x.BrewingMethodId });
                    table.ForeignKey(
                        name: "FK_BrandBrewingMethods_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandBrewingMethods_BrewingMethods_BrewingMethodId",
                        column: x => x.BrewingMethodId,
                        principalTable: "BrewingMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandFermentationTypes",
                columns: table => new
                {
                    BrandId = table.Column<long>(nullable: false),
                    FermentationTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandFermentationTypes", x => new { x.BrandId, x.FermentationTypeId });
                    table.ForeignKey(
                        name: "FK_BrandFermentationTypes_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandFermentationTypes_FermentationTypes_FermentationTypeId",
                        column: x => x.FermentationTypeId,
                        principalTable: "FermentationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandSeasons",
                columns: table => new
                {
                    BrandId = table.Column<long>(nullable: false),
                    SeasonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandSeasons", x => new { x.BrandId, x.SeasonId });
                    table.ForeignKey(
                        name: "FK_BrandSeasons_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandSeasons_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandWrappings",
                columns: table => new
                {
                    BrandId = table.Column<long>(nullable: false),
                    WrappingId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandWrappings", x => new { x.BrandId, x.WrappingId });
                    table.ForeignKey(
                        name: "FK_BrandWrappings_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandWrappings_Wrappings_WrappingId",
                        column: x => x.WrappingId,
                        principalTable: "Wrappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<long>(nullable: true),
                    BrandId = table.Column<long>(nullable: true),
                    Rate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedAt = table.Column<DateTime>(nullable: false),
                    AuthorId = table.Column<long>(nullable: true),
                    BrandId = table.Column<long>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    EditedAt = table.Column<DateTime>(nullable: true),
                    IsDleted = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandBrewingMethods_BrewingMethodId",
                table: "BrandBrewingMethods",
                column: "BrewingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandFermentationTypes_FermentationTypeId",
                table: "BrandFermentationTypes",
                column: "FermentationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandProductions_AddedById",
                table: "BrandProductions",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_BrandProductions_EditedById",
                table: "BrandProductions",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_BrandProductions_ProducedById",
                table: "BrandProductions",
                column: "ProducedById");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_AddedById",
                table: "Brands",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandProductionId",
                table: "Brands",
                column: "BrandProductionId",
                unique: true,
                filter: "[BrandProductionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_DeletedById",
                table: "Brands",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_EditedById",
                table: "Brands",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_KindId",
                table: "Brands",
                column: "KindId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandSeasons_SeasonId",
                table: "BrandSeasons",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandWrappings_WrappingId",
                table: "BrandWrappings",
                column: "WrappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Breweries_AddedById",
                table: "Breweries",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Breweries_EditedById",
                table: "Breweries",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_Breweries_OwnerId",
                table: "Breweries",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BrewingGroups_AddedById",
                table: "BrewingGroups",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_BrewingGroups_EditedById",
                table: "BrewingGroups",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_AddedById",
                table: "Kinds",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_EditedById",
                table: "Kinds",
                column: "EditedById");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AuthorId",
                table: "Ratings",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BrandId",
                table: "Ratings",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BrandId",
                table: "Reviews",
                column: "BrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandBrewingMethods");

            migrationBuilder.DropTable(
                name: "BrandFermentationTypes");

            migrationBuilder.DropTable(
                name: "BrandSeasons");

            migrationBuilder.DropTable(
                name: "BrandWrappings");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "BrewingMethods");

            migrationBuilder.DropTable(
                name: "FermentationTypes");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Wrappings");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "BrandProductions");

            migrationBuilder.DropTable(
                name: "Kinds");

            migrationBuilder.DropTable(
                name: "Breweries");

            migrationBuilder.DropTable(
                name: "BrewingGroups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
