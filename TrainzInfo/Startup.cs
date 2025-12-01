using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Tools;
using TrainzInfo.Tools.JWT;

namespace TrainzInfo
{
    public class Startup
    {
        public static bool DEBUG_MODE = true;
        public static bool START_IN_PROD_DB = false;
        static string _connectionString = "";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Init("Startup", nameof(ConfigureServices));
            Log.Start();
            Log.Wright("Try add services");
            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"])),
                    RoleClaimType = ClaimTypes.Role
                };
            });

            services.AddAuthorization();

            string connection = "";
            string trace = "";
            Log.Wright("Try add DB context");
            services.AddMemoryCache();
            Log.Wright("Try add session");
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            Log.Wright("Try add MVC");
            services.AddMvc();

            Log.Wright("Try find connection string");
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


            Log.Wright(trace);
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            Log.Wright("Try add identity");
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            //services.AddControllersWithViews();
            //services.AddRazorPages();
            services.AddControllers(); // API
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(
                                "https://trainzinfo.com.ua",
                                "https://www.trainzinfo.com.ua",
                                "https://localhost:5001",
                                "https://localhost:5000",
                                "http://localhost:5001",
                                "http://localhost:5000",
                                "https://localhost:7235",
                                "https://localhost:7004"
                            )
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                     .AllowCredentials();
                });
            });

            var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.AddSingleton(jwtSettings);
            services.AddSingleton<JwtService>(sp =>
            {
                var settings = sp.GetRequiredService<JwtSettings>();
                return new JwtService(settings.Secret, settings.Issuer);
            });

            Mail mail = new Mail();
            Log.Finish();
            _connectionString = connection;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Init("Startup", nameof(Configure));
            Log.Start();
            Log.Wright("Try configure app");
            if (env.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(); // <- обязательно перед авторизацией

            app.Use(async (context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "https://www.trainzinfo.com.ua");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Authorization");
                    await context.Response.CompleteAsync();
                }
                else
                {
                    await next();
                }
            });


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();          // API
                endpoints.MapFallbackToFile("index.html"); // Blazor WASM
            });

            var endpointDataSource = app.ApplicationServices.GetRequiredService<EndpointDataSource>();
            Log.Wright("Try get all registered endpoints");
            
            foreach (var endpoint in endpointDataSource.Endpoints)
            {
                Log.Wright($"Registered endpoint: {endpoint.DisplayName}");
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                CreateRoles(serviceProvider).Wait();
            }
            var supportedCultures = new[] { new CultureInfo("uk-UA") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("uk-UA"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            
            
            Log.Finish();

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

        public static string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
