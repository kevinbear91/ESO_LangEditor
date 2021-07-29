using AutoMapper;
using ESO_LangEditor.API.Filters;
using ESO_LangEditor.API.Helpers;
using ESO_LangEditor.API.Services;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore;
using ESO_LangEditor.EFCore.DataRepositories;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorApi
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
            services.AddMvc(config =>
            {
                config.Filters.Add<JsonExceptionFilter>();

                config.EnableEndpointRouting = false;
                //config.Filters.Add<JsonExceptionFilter>();

                config.ReturnHttpNotAcceptable = true;
                //config.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                config.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 60
                    });

                config.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<LangtextApiDbContext>(config =>
            {
                config.UseNpgsql(Configuration.GetConnectionString("PostgreSqlConnection"),
                    optionBuilder => optionBuilder.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            });
            //services.AddScoped<ILangTextRepository, MockLangtextRepository>();
            services.AddScoped<ValidationFilter>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Tokens.ProviderMap.Add("RefreshTokenProvider",
                    new TokenProviderDescriptor(typeof(CustomRefreshTokenProvider<User>)));
            })
                .AddEntityFrameworkStores<LangtextApiDbContext>()
                .AddDefaultTokenProviders();

            var tokenSection = Configuration.GetSection("Security:Token");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {

                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = tokenSection["Issuer"],
                      ValidAudience = tokenSection["Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSection["Key"])),
                      ClockSkew = TimeSpan.Zero
                  };
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseDeveloperExceptionPage();
            //app.UseMvc();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            


        }
    }
}
