using Backend.DAL;
using Backend.Model;
using Backend.Pricing.BL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend
{
    public class Startup
    {
        private const string CorsPolicy = "AllowAnyOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "HEAD");
                    });
            });

            services.AddControllers();
            //services.AddScoped(typeof(IDataRepository<Price>), typeof(DataRepository<Price>));
            //services.AddScoped(typeof(IDataUpdater<Price>), typeof(PriceUpdater<Price>));
            //services.AddScoped(typeof(ICalculationService), typeof(CalculationService));

            services.AddSingleton<IDataRepository<Price>, DataRepository<Price>>();
            services.AddSingleton<IDataUpdater<Price>, PriceUpdater<Price>>();
            services.AddSingleton<ICalculationService, CalculationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
