using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Modules.Users.Models;

namespace TaskManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // ← necesar pentru roluri
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews()
        .AddRazorOptions(options =>
        {
            // 1. View-uri specifice modulelor (ex: Modules/Users/Views/Index.cshtml)
            options.ViewLocationFormats.Add("/Modules/{1}/Views/{0}.cshtml");

            // 2. View-uri mutate în folderul central Modules (ex: Modules/Views/Home/Index.cshtml)
            options.ViewLocationFormats.Add("/Modules/Views/{1}/{0}.cshtml");

            // 3. IMPORTANT: View-uri Shared (Navbar, Layout) pentru Controller-e
            options.ViewLocationFormats.Add("/Modules/Views/Shared/{0}.cshtml");

            // 4. IMPORTANT: Aceasta permite paginilor Identity să vadă folderul Shared din Modules
            options.PageViewLocationFormats.Add("/Modules/Views/Shared/{0}.cshtml");

            // Păstrează fallback-ul pe vechiul folder
            options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

            // Configurare Identity
            options.ViewLocationFormats.Add("/Areas/Identity/Pages/Account/{0}.cshtml");
            options.ViewLocationFormats.Add("/Areas/Identity/Pages/{0}.cshtml");
        });

            var app = builder.Build();

            // ------------------------------
            //  CREARE ROLURI + ADMIN DEFAULT
            // ------------------------------
            async Task SeedDataAsync(WebApplication app)
            {
                using var scope = app.Services.CreateScope();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Rolurile dorite
                string[] roles = { "Admin", "Manager", "User" };

                // Creăm rolurile dacă nu există
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Creare Admin implicit
                string adminEmail = "admin@taskmanager.com";
                string adminPassword = "Admin123!";

                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Default Admin"
                    };

                    await userManager.CreateAsync(admin, adminPassword);
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // RULĂM SEEDING-UL
            await SeedDataAsync(app);



            // ------------------------------
            //   PIPELINE APP
            // ------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "modules",
                pattern: "{controller}/{action}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
