using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Recipie.Data;
using Recipie.Domain.Models;

namespace Recipie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            /*var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    InsertData(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();*/
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void InsertData(IServiceProvider serviceProvider)
        {
            Recipe recipe1 = new Recipe("Spagetti","Olasz tésztaétel");
            Recipe recipe2 = new Recipe("Rántott hús", "Hús beforgatva panírba, kisütve");
            Ingredient ingredient1 = new Ingredient("Paradicsom", "piros bogyótermésû növény");
            Ingredient ingredient2 = new Ingredient("Liszt", "Általában búza örlemény, sok mindenhez jó.");

            using (var context = new RecipeContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RecipeContext>>()))
            {
                context.Recipes.Add(recipe1);
                context.Recipes.Add(recipe2);
                context.Ingredients.Add(ingredient1);
                context.Ingredients.Add(ingredient2);
                context.SaveChanges();
            }
        }
    }
}
