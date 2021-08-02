using CartService.BackgroundServices;
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
            
            services.AddTransient<ITryCatchWrapper, TryCatchWrapper>();
            
            services.AddTransient<ICartRepository, CartRepository>(provider =>
                new CartRepository(connectionString,provider.GetService<ITryCatchWrapper>()));
            services.AddTransient<IProductRepository, ProductRepository>(provider =>
                new ProductRepository(connectionString,provider.GetService<ITryCatchWrapper>()));
            services.AddTransient<ICartSubscriberRepository, CartSubscriberRepository>(provider =>
                new CartSubscriberRepository(connectionString,provider.GetService<ITryCatchWrapper>()));
            services.AddTransient<IDelCartNotificationRepository, DelCartNotificationRepository>( provider =>
                new DelCartNotificationRepository(connectionString,provider.GetService<ITryCatchWrapper>()));

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

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
