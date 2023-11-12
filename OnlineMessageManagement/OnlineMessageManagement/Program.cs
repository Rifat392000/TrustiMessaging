using OnlineMessageManagement.Data;
using OnlineMessageManagement.DataAccess;
using OnlineMessageManagement.Services;


namespace OnlineMessageManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<ISocialServiceServices, SocialServiceServices>();
            builder.Services.AddTransient<ServiceUserRepository>();
            builder.Services.AddTransient<CustomerRepository>();
            builder.Services.AddTransient<SocialServiceRepository>();
            builder.Services.AddScoped<AppUserDataAccess>();
            builder.Services.AddScoped<UserRepository>();

            // Configure session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout set to 30 minutes
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add session middleware
            app.UseSession();

            // Add authentication middleware
            app.UseAuthentication(); 

            // Add authorization middleware
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
