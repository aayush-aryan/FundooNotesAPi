using BusinessLayer.interfaces;
using BusinessLayer.Services;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
//using NLog.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //IConfiguration -> Represents a set of key/value application configuration properties.
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            //adding FundooDBContext class as service
            //UseSqlServer()-configure the context to SqlServerdatabase
            services.AddDbContext<FundooDBContext>(opts => opts.UseSqlServer(Configuration["ConnectionStrings:FundooDB"]));
            //adding a transient services means that each time the service is requested;a new instance is created;
            services.AddTransient<IUserRL, UserRL>();
            services.AddTransient<IUserBL, UserBL>();
            services.AddTransient<INoteRL, NoteRL>();
            services.AddTransient<INoteBL, NoteBL>();
            services.AddTransient<ICollaboratorRL,CollaboratorRL>();
            services.AddTransient<ICollaboratorBL,CollaboratorBL>();
            services.AddTransient<ILabelRL, LabelRL>();
            services.AddTransient<ILabelBL, LabelBL>();
            services.AddSingleton<ResponseModel>();
            services.AddTransient<NoteRL>();
            services.AddStackExchangeRedisCache(options =>  
            {
                options.Configuration = "localhost:6379";
                services.AddMemoryCache();
            });
            services.AddSwaggerGen(setup =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            /*
            services.AddLogging(builder => 
                {
                    builder.SetMinimumLevel(LogLevel.Information);
                    builder.AddNLog("nlog.config");
                });
            */

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //IApplicationBuilder-> Define a class that provides the mechanisms to configure an application's request
        //     pipeline.
        //IWebHostEnvironment->Provides information about the web hosting environment an application is running
        //     in.
        //
        // Middleware is a piece of code in an application pipeline used to handle requests and responses.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Checks if the current host environment name is Microsoft.Extensions.Hosting.EnvironmentName.Development.
            if (env.IsDevelopment())
            {
                //This middleware is used reports app runtime errors in development environment.
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
            });
            //This middleware is used to redirects HTTP requests to HTTPS.
            app.UseHttpsRedirection();
            //This middleware is used to route requests. 
            app.UseRouting();
            //This middleware is used to authorizes a user to access secure resources. 
            app.UseAuthorization();
            app.UseAuthentication();
            //This middleware is used to add Controller endpoints to the request pipeline.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
