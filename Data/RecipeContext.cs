using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Recipie.Domain.Models;
using Recipie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipie.Data
{
    public class RecipeContext : IdentityDbContext<User>
    {
        public RecipeContext(DbContextOptions<RecipeContext> options)
            : base(options) { }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(s => new { s.IngredientId, s.RecipeId });
        }
    }
}
