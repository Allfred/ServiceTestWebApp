using CartService.BackgroundServices;
using CartService.Middleware;
using CartService.Middleware.Exception;
using CartService.Models.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CartService.Models.Carts;
using CartService.Models.CartSubscribers;
using CartService.Models.DelCartNotifications;
using CartService.Models.Products;
using CartService.Models.WebHook;
using CartService.Reporting;

namespace CartService
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
            string connectionString = Configuration.GetConnectionString("connectionStringTest2");

            services.AddControllers();
            
            services.AddTransient<ICartRepository, CartRepository>(_ =>
                new CartRepository(connectionString));
            services.AddTransient<IProductRepository, ProductRepository>(_ =>
                new ProductRepository(connectionString));
            services.AddTransient<ICartSubscriberRepository, CartSubscriberRepository>(_ =>
                new CartSubscriberRepository(connectionString));
            services.AddTransient<IDelCartNotificationRepository, DelCartNotificationRepository>( _ =>
                new DelCartNotificationRepository(connectionString));

            services.AddHttpClient();
            services.AddTransient<IDelCartWebHook, DelCartWebHook>();
            
            services.AddHostedService<CleaningService>();

            services.AddTransient<IReport<ICartRepository>, CartRepositoryReport>();
            services.AddHostedService<ReportingService>();

    
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/api/swagger.json", "Catalog API V1");
                });
            }

            app.ConfigureCustomExceptionMiddleware();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
