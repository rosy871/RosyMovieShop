using Microsoft.EntityFrameworkCore;
using RosyMovieShop.Data;
using RosyMovieShop.Services;

namespace RosyMovieShop
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();


			builder.Services.AddDistributedMemoryCache(); // Required for session state

			builder.Services.AddSession();
			builder.Services.AddHttpContextAccessor();//checking session in cshtml so needed to add this



			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<RSDBContext>(o => o.UseSqlServer(connectionString));

			//Add user services here
			builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<IMovieServices, MovieServices>();
			builder.Services.AddScoped<IOrderServices, OrderServices>();



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

			app.UseSession();


			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Customer}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
