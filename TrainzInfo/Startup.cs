
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;
using TrainzInfo.Data;
using TrainzInfo.Tools;

namespace TrainzInfo
{
    public class Startup
    {
        public static bool DEBUG_MODE = true;
        public static bool START_IN_PROD_DB = true;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LoggingExceptions.CreateFolder();
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //services.AddControllersWithViews();
            string connection = "";
            string trace = "";
            
            if (DEBUG_MODE == true)
            {
                if (START_IN_PROD_DB == false)
                {
                    connection = Configuration.GetConnectionString("DefaultConnection");
                    trace = "test connection good";
                }
                else if (START_IN_PROD_DB == true)
                {
                    connection = Configuration.GetConnectionString("WebProd");
                    trace = ("server connection good!!" + connection);
                }
            }
            else if (DEBUG_MODE == false)
            {

                connection = Configuration.GetConnectionString("WebProd");
                trace = ("server connection good!!" + connection);

            }
            LoggingExceptions.LogWright(connection);
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();

           

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static bool GetConfig()
        {
            return DEBUG_MODE;
        }
    }
}
