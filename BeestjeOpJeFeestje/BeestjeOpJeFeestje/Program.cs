using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace BeestjeOpJeFeestje
{
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add ApplicationDbContext and Identity services to the DI container
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<Account, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                try {
                    var userManager = services.GetRequiredService<UserManager<Account>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await SeedData.Initialize(services, userManager, roleManager);
                } catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            app.Run();
        }
    }
}
