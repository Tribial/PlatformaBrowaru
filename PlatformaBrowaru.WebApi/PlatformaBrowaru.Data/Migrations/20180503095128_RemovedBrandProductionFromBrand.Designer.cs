﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PlatformaBrowaru.Data.Data;
using System;

namespace PlatformaBrowaru.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180503095128_RemovedBrandProductionFromBrand")]
    partial class RemovedBrandProductionFromBrand
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.ApplicationUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<Guid>("Guid");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsVerified");

                    b.Property<string>("LastName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Role");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Brand", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<long?>("AddedById");

                    b.Property<decimal>("AlcoholAmountPercent");

                    b.Property<string>("Color");

                    b.Property<DateTime?>("CreationDate");

                    b.Property<long?>("DeletedById");

                    b.Property<string>("DeletionReason");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EditedAt");

                    b.Property<long?>("EditedById");

                    b.Property<decimal>("ExtractPercent");

                    b.Property<int>("HopIntensity");

                    b.Property<string>("Ingredients");

                    b.Property<bool>("IsAccepted");

                    b.Property<bool>("IsFiltered");

                    b.Property<bool>("IsPasteurized");

                    b.Property<long?>("KindId");

                    b.Property<string>("Name");

                    b.Property<int>("Sweetness");

                    b.Property<int>("TasteFullness");

                    b.HasKey("Id");

                    b.HasIndex("AddedById");

                    b.HasIndex("DeletedById");

                    b.HasIndex("EditedById");

                    b.HasIndex("KindId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandBrewingMethod", b =>
                {
                    b.Property<long>("BrandId");

                    b.Property<long>("BrewingMethodId");

                    b.HasKey("BrandId", "BrewingMethodId");

                    b.HasIndex("BrewingMethodId");

                    b.ToTable("BrandBrewingMethods");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandFermentationType", b =>
                {
                    b.Property<long>("BrandId");

                    b.Property<long>("FermentationTypeId");

                    b.HasKey("BrandId", "FermentationTypeId");

                    b.HasIndex("FermentationTypeId");

                    b.ToTable("BrandFermentationTypes");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandProduction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<long?>("AddedById");

                    b.Property<long?>("BrandId");

                    b.Property<DateTime?>("EditedAt");

                    b.Property<long?>("EditedById");

                    b.Property<long?>("ProducedById");

                    b.Property<DateTime>("ProducedFrom");

                    b.Property<DateTime>("ProducedTo");

                    b.HasKey("Id");

                    b.HasIndex("AddedById");

                    b.HasIndex("BrandId");

                    b.HasIndex("EditedById");

                    b.HasIndex("ProducedById");

                    b.ToTable("BrandProductions");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandSeason", b =>
                {
                    b.Property<long>("BrandId");

                    b.Property<long>("SeasonId");

                    b.HasKey("BrandId", "SeasonId");

                    b.HasIndex("SeasonId");

                    b.ToTable("BrandSeasons");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandWrapping", b =>
                {
                    b.Property<long>("BrandId");

                    b.Property<long>("WrappingId");

                    b.HasKey("BrandId", "WrappingId");

                    b.HasIndex("WrappingId");

                    b.ToTable("BrandWrappings");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Brewery", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<long?>("AddedById");

                    b.Property<string>("Adress");

                    b.Property<decimal>("AnnualProduction");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("DeletionReason");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EditedAt");

                    b.Property<long?>("EditedById");

                    b.Property<string>("Name");

                    b.Property<long?>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("AddedById");

                    b.HasIndex("EditedById");

                    b.HasIndex("OwnerId");

                    b.ToTable("Breweries");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrewingGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<long?>("AddedById");

                    b.Property<string>("Adress");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("DeletionReason");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EditedAt");

                    b.Property<long?>("EditedById");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AddedById");

                    b.HasIndex("EditedById");

                    b.ToTable("BrewingGroups");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrewingMethod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Method");

                    b.HasKey("Id");

                    b.ToTable("BrewingMethods");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.FermentationType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("FermentationTypes");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Kind", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<long?>("AddedById");

                    b.Property<string>("CountryOfOrigin");

                    b.Property<DateTime?>("EditedAt");

                    b.Property<long?>("EditedById");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AddedById");

                    b.HasIndex("EditedById");

                    b.ToTable("Kinds");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AuthorId");

                    b.Property<long?>("BrandId");

                    b.Property<decimal>("Rate");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BrandId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.RefreshToken", b =>
                {
                    b.Property<string>("Token")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("TokenExpirationDate");

                    b.Property<long>("UserId");

                    b.HasKey("Token");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Review", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<long?>("AuthorId");

                    b.Property<long?>("BrandId");

                    b.Property<string>("Content");

                    b.Property<DateTime?>("EditedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BrandId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Season", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Wrapping", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Wrappings");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Brand", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "AddedBy")
                        .WithMany()
                        .HasForeignKey("AddedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.Kind", "Kind")
                        .WithMany("Brands")
                        .HasForeignKey("KindId");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandBrewingMethod", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.Brand", "Brand")
                        .WithMany("BrandBrewingMethods")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlatformaBrowaru.Share.Models.BrewingMethod", "BrewingMethod")
                        .WithMany("BrandBrewingMethods")
                        .HasForeignKey("BrewingMethodId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandFermentationType", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.Brand", "Brand")
                        .WithMany("BrandFermentationTypes")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlatformaBrowaru.Share.Models.FermentationType", "FermentationType")
                        .WithMany("BrandFermentationTypes")
                        .HasForeignKey("FermentationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandProduction", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "AddedBy")
                        .WithMany()
                        .HasForeignKey("AddedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId");

                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.Brewery", "ProducedBy")
                        .WithMany("BrandProductions")
                        .HasForeignKey("ProducedById");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandSeason", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.Brand", "Brand")
                        .WithMany("BrandSeasons")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlatformaBrowaru.Share.Models.Season", "Season")
                        .WithMany("BrandSeasons")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrandWrapping", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.Brand", "Brand")
                        .WithMany("BrandWrappings")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlatformaBrowaru.Share.Models.Wrapping", "Wrapping")
                        .WithMany("BrandWrappings")
                        .HasForeignKey("WrappingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Brewery", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "AddedBy")
                        .WithMany()
                        .HasForeignKey("AddedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.BrewingGroup", "Owner")
                        .WithMany("Breweries")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.BrewingGroup", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "AddedBy")
                        .WithMany()
                        .HasForeignKey("AddedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Kind", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "AddedBy")
                        .WithMany()
                        .HasForeignKey("AddedById");

                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "EditedBy")
                        .WithMany()
                        .HasForeignKey("EditedById");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Rating", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("PlatformaBrowaru.Share.Models.Brand", "Brand")
                        .WithMany("Ratings")
                        .HasForeignKey("BrandId");
                });

            modelBuilder.Entity("PlatformaBrowaru.Share.Models.Review", b =>
                {
                    b.HasOne("PlatformaBrowaru.Share.Models.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("PlatformaBrowaru.Share.Models.Brand", "Brand")
                        .WithMany("Reviews")
                        .HasForeignKey("BrandId");
                });
#pragma warning restore 612, 618
        }
    }
}
