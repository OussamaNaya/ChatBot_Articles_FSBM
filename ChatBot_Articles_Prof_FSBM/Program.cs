using ChatBot_Articles_Prof_FSBM.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatBot_Articles_Prof_FSBM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Ajouter les services de session et cache en mémoire pour la session
            builder.Services.AddDistributedMemoryCache(); // Cache en mémoire
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // durée de vie session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();  // Obligatoire pour activer la session

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Connexion}");

            app.Run();
        }
    }
}
