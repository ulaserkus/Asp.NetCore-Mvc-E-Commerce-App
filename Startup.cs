using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.webui.EmailService;
using App.webui.identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using web_app.business.Abstract;
using web_app.business.Concrete;
using web_app.data;
using web_app.data.Abstract;
using web_app.data.Concrete;
using web_app.data.Concrete.EfCore;

namespace App.webui
{
    public class Startup
    {
        private IConfiguration Configuration;
        public Startup(IConfiguration config)
        {
            this.Configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Data Source=ShopDb"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                //lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;


                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder()
                {
                    HttpOnly = true,
                    Name = ".App.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };



            });

            services.AddControllersWithViews();

            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<IProductService, ProductManager>();

            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryManager>();


            services.AddScoped<ICartRepository, EfCoreCartRepository>();
            services.AddScoped<ICartService, CartManager>();

            services.AddScoped<IOrderRepository, EfCoreOrderRepository>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>

                 new SmtpEmailSender(
                     Configuration["EmailSender:Host"],
                     Configuration.GetValue<int>("EmailSender:Port"),
                     Configuration.GetValue<bool>("EmailSender:EnableSSl"),
                     Configuration["EmailSender:UserName"],
                     Configuration["EmailSender:Password"]
                    )
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();

            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),

                RequestPath = "/modules"
            });
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                  name: "orders",
                  pattern: "orders",
                  defaults: new { controller = "Cart", action = "GetOrders" }
                );
                endpoints.MapControllerRoute(
                  name: "checkout",
                  pattern: "checkout",
                  defaults: new { controller = "Cart", action = "Checkout" }
                );

                endpoints.MapControllerRoute(

                   name: "cart",
                   pattern: "cart",
                   defaults: new { controller = "Cart", action = "Index" }

                 );

                endpoints.MapControllerRoute(

                name: "adminusers",
                pattern: "admin/user/list",
                defaults: new { controller = "Admin", action = "UserList" }

              );

                endpoints.MapControllerRoute(
                  name: "adminuseredit",
                  pattern: "admin/user/{id?}",
                  defaults: new { controller = "Admin", action = "UserEdit" }

               );

                endpoints.MapControllerRoute(

                  name: "adminroles",
                  pattern: "admin/role/list",
                  defaults: new { controller = "Admin", action = "RoleList" }

                );

                endpoints.MapControllerRoute(

                  name: "adminrolecreate",
                  pattern: "admin/role/create",
                  defaults: new { controller = "Admin", action = "RoleCreate" }

                );
                endpoints.MapControllerRoute(
                   name: "adminroleedit",
                   pattern: "admin/role/{id?}",
                   defaults: new { controller = "Admin", action = "RoleEdit" }

                );

                endpoints.MapControllerRoute(

                  name: "adminproducts",
                  pattern: "admin/products",
                  defaults: new { controller = "Admin", action = "ProductList" }

                );
                endpoints.MapControllerRoute(

                 name: "adminproductcreate",
                 pattern: "admin/products/create",
                 defaults: new { controller = "Admin", action = "ProductCreate" }

               );
                endpoints.MapControllerRoute(
                   name: "adminproductedit",
                   pattern: "admin/products/{id?}",
                   defaults: new { controller = "Admin", action = "ProductEdit" }

                );


                endpoints.MapControllerRoute(

                 name: "admincategories",
                 pattern: "admin/categories",
                 defaults: new { controller = "Admin", action = "CategoryList" }

               );
                endpoints.MapControllerRoute(

                 name: "admincategorycreate",
                 pattern: "admin/categories/create",
                 defaults: new { controller = "Admin", action = "CategoryCreate" }

               );


                endpoints.MapControllerRoute(
                   name: "admincategoryedit",
                   pattern: "admin/categories/{id?}",
                   defaults: new { controller = "Admin", action = "CategoryEdit" }

                );


                //localhost/search
                endpoints.MapControllerRoute(
                  name: "search",
                  pattern: "search",
                  defaults: new { controller = "Shop", action = "Search" }

              );

                endpoints.MapControllerRoute(
                  name: "productdetails",
                  pattern: "{url}",
                  defaults: new { controller = "Shop", action = "details" }

              );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new { controller = "Shop", action = "list" }

                );
                endpoints.MapControllerRoute(

                   name: "default",
                   pattern: "{controller=Home}/{action=index}/{id?}"

                );


            });


            SeedIdentity.Seed(userManager, roleManager, configuration).Wait();

        }
    }
}
