using CartService.BackgroundServices;
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

namespace CartService
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
            string connectionString = "Data Source=DESKTOP-7A2QTBA;Initial Catalog=Test;Persist Security Info=True;Integrated Security=true; MultipleActiveResultSets=true;";

            services.AddControllers();
            services.AddTransient<ICartRepository, CartRepository>(provider => new CartRepository(connectionString));
            services.AddTransient<IProductRepository, ProductRepository>(provider => new ProductRepository(connectionString));
            services.AddTransient<ICartSubscriberRepository, CartSubscriberRepository>(provider => new CartSubscriberRepository(connectionString));
            services.AddTransient<IDelCartNotificationRepository, DelCartNotificationRepository>(provider => new DelCartNotificationRepository(connectionString));
            services.AddTransient<IDelCartWebHook, DelCartWebHook>();
            
            services.AddHostedService<CleaningService>();
            services.AddHostedService<ReportingService>();
            services.AddHttpClient();
            
    
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
