using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LoggingExceptions.LogInit("Startup", nameof(ConfigureServices));
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("Try add services");
            services.AddControllersWithViews();
            string connection = "";
            string trace = "";
            LoggingExceptions.LogWright("Try add DB context");
            services.AddMemoryCache();
            LoggingExceptions.LogWright("Try add session");
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            LoggingExceptions.LogWright("Try add MVC");
            services.AddMvc();

            LoggingExceptions.LogWright("Try find connection string");
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
            LoggingExceptions.LogWright(trace);
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            LoggingExceptions.LogWright("Try add identity");
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            services.AddControllersWithViews();
            Mail mail = new Mail();
            LoggingExceptions.LogFinish();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            LoggingExceptions.LogInit("Startup", nameof(Configure));
            LoggingExceptions.LogStart();
            LoggingExceptions.LogWright("Try configure app");
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
            LoggingExceptions.LogWright("Try use HTTPS redirection");
            app.UseHttpsRedirection();
            LoggingExceptions.LogWright("Try use static files");
            app.UseStaticFiles();
            LoggingExceptions.LogWright("Try use routing");
            app.UseRouting();
            LoggingExceptions.LogWright("Try use authorization");
            app.UseAuthentication();
            app.UseAuthorization();
            LoggingExceptions.LogWright("Try use session");
            app.UseSession();
            LoggingExceptions.LogWright("Try use endpoints");
            app.UseEndpoints(endpoints =>
            {
                // Спочатку MVC контролери
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Потім Razor Pages
                endpoints.MapRazorPages();
            });

            var endpointDataSource = app.ApplicationServices.GetRequiredService<EndpointDataSource>();
            LoggingExceptions.LogWright("Try get all registered endpoints");
            
            foreach (var endpoint in endpointDataSource.Endpoints)
            {
                LoggingExceptions.LogWright($"Registered endpoint: {endpoint.DisplayName}");
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                CreateRoles(serviceProvider).Wait();
            }
            LoggingExceptions.LogFinish();

        }

        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Superadmin", "Admin", "Manager", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Приклад: створити адміна
            var adminUser = await userManager.FindByEmailAsync("alexander.vinichuk@outlook.com");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "alexander.vinichuk@outlook.com",
                    Email = "alexander.vinichuk@outlook.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
            }

            await userManager.AddToRoleAsync(adminUser, "Superadmin");
        }

        public static bool GetConfig()
        {
            return DEBUG_MODE;
        }
    }
}
