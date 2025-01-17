﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using proiectDAW.Data;
using proiectDAW.Repositories.DatabaseRepository;
using proiectDAW.Services;
using proiectDAW.Utilities;
using proiectDAW.Utilities.JWTUtils;

namespace proiectDAW
{
    public class Startup
    {
        private string CorsAllowSpecificOrigin = "frontendAllowOrigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "ASP.NET "
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<ProjectContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // inregistram toate repository-urile si toate serviciile (pt dependency injection)
            // transient: la fiecare injectare o instanta noua!
            services.AddTransient<IUtilizatorRepository, UtilizatorRepository>();
            services.AddTransient<IDatePersonaleRepository, DatePersonaleRepository>();
            services.AddTransient<IColectieRepository, ColectieRepository>();
            services.AddTransient<IRetetaRepository, RetetaRepository>();
            services.AddTransient<IBucatarieRepository, BucatarieRepository>();


            services.AddTransient<IColectieService, ColectieService>();
            services.AddTransient<IUtilizatorService, UtilizatorService>();
            services.AddTransient<IDatePersonaleService, DatePersonaleService>();
            services.AddTransient<IRetetaService, RetetaService>();
            services.AddTransient<IBucatarieService, BucatarieService>();


            services.AddScoped<IJWTUtils, JWTUtils>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddCors(option =>
            {
                option.AddPolicy(name: CorsAllowSpecificOrigin,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApi");
                c.RoutePrefix = string.Empty;
            });
            
            app.Use(async (c, n) => {
                c.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                await n.Invoke();
            });
            

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()) ;
            

            app.UseHttpsRedirection();
            app.UseMiddleware<JWTMiddleware>();
            app.UseMvc();
        }
    }
}
