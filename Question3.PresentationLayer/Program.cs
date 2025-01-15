using Core.Presentation.ViewComponents.Components;
using Microsoft.EntityFrameworkCore;
using Question3.BusinessLogicLayer;
using Question3.DataAccessLayer;

namespace Question3.PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Logging.AddProvider(new FileLoggerProvider());
            builder.Services.AddDbContext<WebDbContext>(config =>
            {
                config.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddServerSideBlazor();
            builder.Services.AddBusinessServices();
            builder.Services.AddControllersWithViews();
           
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
           
            app.MapBlazorHub();
            app.MapFallbackToController("Blazor", "Home");
            app.Run();
        }
    }
}
