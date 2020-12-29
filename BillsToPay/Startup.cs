using Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Models;
using Repository;
using System;
using System.IO.Compression;
using System.Reflection;

namespace BillsToPay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var configDatabase = new DatabaseSettings()
            {
                ConnectionString = Configuration["ConnectionStrings:DefaultConnection"]
            };

            services.AddSingleton(configDatabase);

            services.Configure<GzipCompressionProviderOptions>(opt => { opt.Level = CompressionLevel.Optimal; });
            services.AddResponseCompression(opt => { opt.Providers.Add<GzipCompressionProvider>(); });

            services.AddSingleton<IBillRepository, BillRepository>();
            services.AddSingleton<IBillBusiness, BillBusiness>();
            services.AddSingleton<IInterestRuleRepository, InterestRuleRepository>();
  
            services.AddDbContext<Context>(opt => opt.UseSqlServer(configDatabase.ConnectionString, b => b.MigrationsAssembly("BillsToPay")));

            services.AddControllers();

            var version = Assembly.GetEntryAssembly().GetName().Version;
            var versionpackageBuilder = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = versionpackageBuilder,
                    Title = "API ",
                    Description = "Description",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Name Contact",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Name License",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            var versionpackageBuilder = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", versionpackageBuilder);
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            System.Threading.Tasks.Task.Run(async () =>
            {
                await DataSeeder.SeedDataAsync(app);
            });
        }
    }
}
