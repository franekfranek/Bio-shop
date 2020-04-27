using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopCart.Infrastructure;

namespace ShopCart
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
            services.AddMemoryCache();
            services.AddSession(options => 
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(2); THIS CLEAR SESSION IN 2 SECONDS FROMSECOND DAY HOURS ETC
                //options.IdleTimeout = TimeSpan.FromDays(2);
            });

            services.AddControllersWithViews();

            services.AddDbContext<ShopCartContext>(options => options.UseSqlServer(Configuration.
                                                                GetConnectionString("ShopCartContext")));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //!!!!!!!HERE EVEN IF SPACE IS IN WRONG PLACE WONT WORK BE VERY CAREFUL WITH NEW ROUTES !!!!!!!!!!!!!!


                //WHEN U SPECIFY THE ROUTS U GO FROM MOST SPECIFIC TO LESS SPECIFIC
                //routes look like / or /products 
                //endpoints.MapControllerRoute(
                //    "pages",
                //    "{slug?}",
                //    defaults: new { controller = "Pages", action = "Page" }
                //);

                //endpoints.MapControllerRoute(
                //    "products",
                //    "products/{categorySlug?}",
                //    defaults: new { controller = "Products", action = "ProductsByCategory" }
                //);


                //endpoints.MapControllerRoute(
                //  name: "default",
                //  pattern: "{controller=Pages}/{action=Page}/{id?}"
                //);



                endpoints.MapAreaControllerRoute(
                        name: "areas",
                        areaName: "General",
                        pattern: "{area:exists}/{controller=Pages}/{action=Page}/{id?}"
                    //defaults: new { area = "General", controller = "Pages", action = "Page" }
                    );
                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "Admin",
                    pattern: "{area:exists}/{controller}/{action}/{id?}"
                //defaults: new { area = "Admin", controller="Products", action="Index" }
                );



                //    endpoints.MapControllerRoute(
                //        name: "areas",
                //        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                //);



            });
        }
    }
}
