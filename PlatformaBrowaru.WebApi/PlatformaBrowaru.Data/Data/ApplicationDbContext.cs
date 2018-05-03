using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PlatformaBrowaru.Share.Models;

namespace PlatformaBrowaru.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandBrewingMethod> BrandBrewingMethods { get; set; }
        public DbSet<BrandFermentationType> BrandFermentationTypes { get; set; }
        public DbSet<BrandProduction> BrandProductions { get; set; }
        public DbSet<BrandSeason> BrandSeasons { get; set; }
        public DbSet<BrewingGroup> BrewingGroups { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Kind> Kinds { get; set; }
        public DbSet<FermentationType> FermentationTypes { get; set; }
        public DbSet<BrandWrapping> BrandWrappings { get; set; }
        public DbSet<Wrapping> Wrappings { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BrewingMethod> BrewingMethods { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Brand>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Brand>()
                .HasOne(b => b.Kind)
                .WithMany(b => b.Brands);
            modelBuilder.Entity<Brand>()
                .HasOne(b => b.AddedBy);
            modelBuilder.Entity<Brand>()
                .HasOne(b => b.EditedBy);
            modelBuilder.Entity<Brand>()
                .HasOne(b => b.DeletedBy);
            modelBuilder.Entity<Brand>()
                .HasMany(b => b.Ratings)
                .WithOne(b => b.Brand);
            modelBuilder.Entity<Brand>()
                .HasMany(b => b.Reviews)
                .WithOne(b => b.Brand);

            modelBuilder.Entity<BrandBrewingMethod>()
                .HasKey(b => new {b.BrandId, b.BrewingMethodId});
            modelBuilder.Entity<BrandBrewingMethod>()
                .HasOne(b => b.Brand)
                .WithMany(b => b.BrandBrewingMethods);
            modelBuilder.Entity<BrandBrewingMethod>()
                .HasOne(b => b.Brand)
                .WithMany(b => b.BrandBrewingMethods);

            modelBuilder.Entity<BrandFermentationType>()
                .HasKey(b => new { b.BrandId, b.FermentationTypeId });
            modelBuilder.Entity<BrandFermentationType>()
                .HasOne(b => b.Brand)
                .WithMany(b => b.BrandFermentationTypes);
            modelBuilder.Entity<BrandFermentationType>()
                .HasOne(b => b.FermentationType)
                .WithMany(b => b.BrandFermentationTypes);

            modelBuilder.Entity<BrandProduction>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<BrandProduction>()
                .HasOne(b => b.Brand);
            modelBuilder.Entity<BrandProduction>()
                .HasOne(b => b.ProducedBy)
                .WithMany(b => b.BrandProductions);
            modelBuilder.Entity<BrandProduction>()
                .HasOne(b => b.AddedBy);
            modelBuilder.Entity<BrandProduction>()
                .HasOne(b => b.EditedBy);

            modelBuilder.Entity<BrandSeason>()
                .HasKey(b => new {b.BrandId, b.SeasonId});
            modelBuilder.Entity<BrandSeason>()
                .HasOne(b => b.Brand)
                .WithMany(b => b.BrandSeasons)
                .HasForeignKey(b => b.BrandId);
            modelBuilder.Entity<BrandSeason>()
                .HasOne(b => b.Season)
                .WithMany(b => b.BrandSeasons)
                .HasForeignKey(b => b.SeasonId);

            modelBuilder.Entity<BrewingGroup>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<BrewingGroup>()
                .HasOne(b => b.AddedBy);
            modelBuilder.Entity<BrewingGroup>()
                .HasOne(b => b.EditedBy);

            modelBuilder.Entity<Brewery>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Brewery>()
                .HasOne(b => b.Owner)
                .WithMany(b => b.Breweries);
            modelBuilder.Entity<Brewery>()
                .HasOne(b => b.AddedBy);
            modelBuilder.Entity<Brewery>()
                .HasOne(b => b.EditedBy);

            modelBuilder.Entity<Kind>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Kind>()
                .HasOne(b => b.AddedBy);
            modelBuilder.Entity<Kind>()
                .HasOne(b => b.EditedBy);

            modelBuilder.Entity<FermentationType>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<BrandWrapping>()
                .HasKey(b => new {b.BrandId, b.WrappingId});
            modelBuilder.Entity<BrandWrapping>()
                .HasOne(b => b.Brand)
                .WithMany(b => b.BrandWrappings);
            modelBuilder.Entity<BrandWrapping>()
                .HasOne(b => b.Wrapping)
                .WithMany(b => b.BrandWrappings);

            modelBuilder.Entity<Wrapping>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Season>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Review>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Review>()
                .HasOne(b => b.Author);

            modelBuilder.Entity<Rating>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Rating>()
                .HasOne(b => b.Author);

            modelBuilder.Entity<BrewingMethod>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<RefreshToken>()
                .HasKey(b => b.Token);

            base.OnModelCreating(modelBuilder);
        }
    }
}
