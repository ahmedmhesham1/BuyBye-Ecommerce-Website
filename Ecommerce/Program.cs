using Ecommerce.Data;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebPWrecover.Services;

namespace Ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
				options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultUI()
				.AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
			builder.Services.AddSession();

			builder.Services.AddScoped<IAddressRepo, AddressRepoService>();
			builder.Services.AddScoped<ICategoryRepo, CategoryRepoService>();
			builder.Services.AddScoped<ICartRepo, CartRepoService>();
			builder.Services.AddScoped<ICartItemRepo, CartItemRepoService>();
			builder.Services.AddScoped<IOrderItemRepo, OrderItemRepoService>();
			builder.Services.AddScoped<IOrderRepo, OrderRepoService>();
			builder.Services.AddScoped<IProductItemRepo, ProductItemRepoService>();
			builder.Services.AddScoped<IProductRepo, ProductRepoService>();
			builder.Services.AddScoped<IUserRepo, UserRepoService>();
			builder.Services.AddScoped<IReviewRepo, ReviewRepoService>();
			builder.Services.AddScoped<IVariationOptionRepo, VariationOptionRepoService>();
			builder.Services.AddScoped<IVariationRepo, VariationRepoService>();
            builder.Services.AddScoped<IWishlistRepo, WishlistRepoService>();



            builder.Services.AddAuthentication().AddFacebook(opt =>
            {
                opt.ClientId = "1447238029233778";
                opt.ClientSecret = "0adc14fe92a61b7dda246415e6ac0d43";
            });
            builder.Services.AddAuthentication().AddGoogle(opt =>
            {
                opt.ClientId = "1037130982527-oqc50qpjea5i70ejj1ubdronun5hk0gc.apps.googleusercontent.com";
                opt.ClientSecret = "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";
            });

            //Stripe
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();


            //Email
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
			
            
			
			app.UseAuthentication();
            app.UseAuthorization();
			app.UseSession();


			using var scope = app.Services.CreateScope();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			var adminEmail = "admin@admin.com";
			var adminPassword = "ASD@asd123";

			if (userManager.FindByEmailAsync(adminEmail).Result == null)
			{
				if (roleManager.FindByNameAsync("Admin").Result == null)
				{
					var adminRole = new IdentityRole("Admin");
					roleManager.CreateAsync(adminRole).Wait();
				}
				var adminUser = new IdentityUser()
				{
					UserName = adminEmail,
					Email = adminEmail,
                    EmailConfirmed = true,
                    PasswordHash = adminPassword
				};
				IdentityResult result = userManager.CreateAsync(adminUser, adminPassword).Result;
				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(adminUser, "Admin").Wait();
				}
			}

			//2-User (Customer and Seller as a User)
			using var sellerScope = app.Services.CreateScope();
			var UserRoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			if (UserRoleManager.FindByNameAsync("User").Result == null)
			{
				var UserRole = new IdentityRole("User");
				roleManager.CreateAsync(UserRole).Wait();
			}


			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
