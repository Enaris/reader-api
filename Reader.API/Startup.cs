using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Reader.API.AutoMapper;
using Reader.API.DataAccess.Context;
using Reader.API.DataAccess.DbModels;
using Reader.API.DataAccess.Repositories;
using Reader.API.Services.Services;

namespace Reader.API
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
            services.AddControllers();
            var migrationAssembly = $"{ nameof(Reader) }.{ nameof(Reader.API) }.{ nameof(Reader.API.DataAccess) }";
            services
                .AddDbContext<ReaderContext>(o =>
                    o.UseSqlServer(Configuration.GetConnectionString("defaultDb"),
                    o => o.MigrationsAssembly(migrationAssembly)));

            services.AddDefaultIdentity<AspUser>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ReaderContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]))
                    };
                });
            services.AddAuthorization();

            services.AddAutoMapper(RootProfiles.Maps);

            // services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IReadingTagService, ReadingTagService>();
            services.AddScoped<IReadingService, ReadingService>();
            services.AddScoped<IReaderUserService, ReaderUserService>();
            services.AddScoped<IReadingSessionService, ReadingSessionService>();
            services.AddScoped<IOptionsLogService, OptionsLogService>();

            // repos 
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IReadingRepository, ReadingRepository>();
            services.AddScoped<IReadingTagRepository, ReadingTagRepository>();
            services.AddScoped<IReaderUserRepository, ReaderUserRepository>();
            services.AddScoped<IReadingSessionRepository, ReadingSessionRepository>();
            services.AddScoped<IOptionsLogRepository, OptionsLogRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Static")),
                RequestPath = new PathString("/Static")
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
