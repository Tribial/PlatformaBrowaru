using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlatformaBrowaru.Data.Migrations
{
    public partial class RemovedBrandProductionFromBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_BrandProductions_BrandProductionId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandProductionId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandProductionId",
                table: "Brands");

            migrationBuilder.AddColumn<long>(
                name: "BrandId",
                table: "BrandProductions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrandProductions_BrandId",
                table: "BrandProductions",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandProductions_Brands_BrandId",
                table: "BrandProductions",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandProductions_Brands_BrandId",
                table: "BrandProductions");

            migrationBuilder.DropIndex(
                name: "IX_BrandProductions_BrandId",
                table: "BrandProductions");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "BrandProductions");

            migrationBuilder.AddColumn<long>(
                name: "BrandProductionId",
                table: "Brands",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandProductionId",
                table: "Brands",
                column: "BrandProductionId",
                unique: true,
                filter: "[BrandProductionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_BrandProductions_BrandProductionId",
                table: "Brands",
                column: "BrandProductionId",
                principalTable: "BrandProductions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
