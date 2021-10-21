using AutoMapper;
using Klika.Dinero.Api.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Klika.Dinero.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Klika.Dinero.Model.Email;
using Microsoft.FeatureManagement;
using Klika.Dinero.Api.Extensions.ErrorMiddleware;
using Klika.Dinero.Api.Extensions.Api;
using Klika.Dinero.Api.Extensions.Startup;
using Klika.Dinero.Api.Extensions.Swagger;
using Hangfire;
using System;
using Hangfire.SqlServer;

namespace Klika.Dinero.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private MapperConfiguration _mapperConfig { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => 
            { 
                options.AddPolicy("CORS", builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()); 
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            });

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddAppVersioning();
            services.AddAppInsights();
            services.AddSingleton(sp => _mapperConfig.CreateMapper());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Authority:ApiUrl"];
                    options.RequireHttpsMetadata = true;
                    options.Audience = Configuration["Authority:Audience"];
                });

            services.AddSwaggerConfiguration();

            services.AddDbContextPool<DineroDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DineroDbContext")));

            services.AddFeatureManagement(); 

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 });
            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration.GetConnectionString("DineroDbContext"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));

            services.AddHangfireServer();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));


            services.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DineroDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                dbContext.Database.Migrate();
            }

            app.UseHangfireDashboard();
            app.UseFileServer();
            app.UseSwaggerConfiguration();
            app.UseCors("CORS");
            app.UseAppInsights();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiEndpoints();
        }
    }
}
