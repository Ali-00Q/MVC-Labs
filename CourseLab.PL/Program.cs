using CourseLab.BLL.Services.Abstraction;
using CourseLab.BLL.Services.Implementation;
using CourseLab.DAL.Database;
using CourseLab.DAL.Entities;
using CourseLab.DAL.Repository.Abstraction;
using CourseLab.DAL.Repository.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseLab.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");
            builder.Services.AddDbContext<CourseLabDbContext>(options =>
              options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<CourseLabDbContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();

            

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

            // 2. CRITICAL: Explicitly map your uploaded Files folder so the server updates in real-time
            var filesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");

            // Create the directory if it somehow doesn't exist yet
            if (!Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }

            
            app.UseRouting();

            app.Use(async (httpContext, next) =>
            {
                var currentTime = DateTime.Now.ToString();
                httpContext.Response.Headers["X-Request-Time"] = currentTime;
                await next();
            });

            app.UseSession();
            app.UseAuthorization();

            app.Map("/admin", adminApp => {
                adminApp.Run(async httpContext =>
                {
                    await httpContext.Response.WriteAsync("Employee Admin Middleware");
                });
            });

            app.Map("/hello", helloApp =>
            {
                helloApp.Run(async httpContext =>
                {
                    await httpContext.Response.WriteAsync("Hello from middleware");
                });
            });

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
