using BeestjeLibrary.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeestjeLibrary.Models;

namespace BeestjeOpJeFeestje {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add scopedServices to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BOJFContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            }).AddEntityFrameworkStores<BOJFContext>();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if(!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Prognoses/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            //seedDatabase(app.Services);

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        //private static void seedDatabase(IServiceProvider services) {
        //    // Create a new scope to be able to use scoped scopedServices
        //    using(var scope = services.CreateScope()) {
        //        var scopedServices = scope.ServiceProvider;
        //        var context = scopedServices.GetRequiredService<BOJFContext>();

        //        context.Database.Migrate();

        //        try {
        //            Task.Run(async () => await new UserAndRoleSeeder().SeedData(context, scopedServices.GetRequiredService<UserManager<ApplicationUser>>(), scopedServices.GetRequiredService<RoleManager<IdentityRole>>())).Wait();
        //        }
        //        catch(Exception ex) {
        //            var logger = scopedServices.GetRequiredService<ILogger<Program>>();
        //            logger.LogError(ex, "An error occurred while seeding the database.");
        //        }
        //    }
        //}
    }
}