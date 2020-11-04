using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Recipie.Data;
using Microsoft.AspNetCore.Identity;
using Recipie.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Recipie.Extensions;
using Recipie.Repositories.CategoryRepository.Interfaces;
using Recipie.Repositories.CategoryRepository;
using Recipie.Repositories.LoginRepository.Interfaces;
using Recipie.Repositories.LoginRepository;
using Recipie.Repositories.LogoutRepository.Interfaces;
using Recipie.Repositories.LogoutRepository;
using Recipie.Repositories.RegisterRepository.Interfaces;
using Recipie.Repositories.RegisterRepository;
using Recipie.Repositories.IngredientRepository.Interfaces;
using Recipie.Repositories.IngredientRepository;
using Recipie.Repositories.UserRepository.Interfaces;
using Recipie.Repositories.UserRepository;
using Recipie.Repositories.TagRepository.Interfaces;
using Recipie.Repositories.TagRepository;
using Recipie.Repositories.RecipeRepository.Interfaces;
using Recipie.Repositories.RecipeRepository;

namespace Recipie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
            services.ConfigureIISIntegration();

            services.AddControllers();
            services.AddDbContextPool<RecipeContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("RecipeConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReciPie", Version = "v1" });
            });

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<RecipeContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options=>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "credentials";
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
                options.Cookie.Domain = "localhost";
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthenticator, Authenticator>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILogoutRepository, LogoutRepository>();
            services.AddScoped<IRegisterRepository, RegisterRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
