using CartService.BackgroundServices;
using CartService.Middleware.Exception;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CartService.Models.Carts;
using CartService.Models.Notifications;
using CartService.Models.Products;
using CartService.Models.WebHook.CartDeleting;
using CartService.Models.WebHook.Common;
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
            string connectionString = Configuration.GetConnectionString("connectionStringTest");

            services.AddControllers();
            
            services.AddTransient<ICartRepository, CartRepository>(_ => new CartRepository(connectionString));
            services.AddTransient<IProductRepository, ProductRepository>(_ => new ProductRepository(connectionString));
            services.AddTransient<IWebHookRepository, WebHookRepository>(_ => new WebHookRepository(connectionString));
            services.AddTransient<INotificationRepository, NotificationRepository>( _ => new NotificationRepository(connectionString));
            services.AddTransient<ICartDeletingWebHook, CartDeletingWebHook>();
            services.AddTransient<IReport, CartReport>();

            services.AddHttpClient();
            
            services.AddHostedService<CleaningService>();
            services.AddHostedService<ReportingService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
                });
            }

            app.UseExceptionMiddleware();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
