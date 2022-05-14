using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP.Data;
using TP.Models;

namespace TP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           

            var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider.GetService<ApplicationDbContext>(); 

            //var context = app.ApplicationServices.GetService<ApplicationDbContext>();
            service.Database.EnsureCreated();
            //UserAndRoleDataInitializer.SeedData(service);

            app.UseStaticFiles();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Imovels}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateRolesAndUsers(serviceProvider).Wait();
        }

        private async Task CreateRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Admin", "Gestor", "Funcionario", "Cliente" };
            IdentityResult roleResult;

            foreach(var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if(!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = new IdentityUser
            {

                UserName = "admin@airbnb.pt",
                Email = "admin@airbnb.pt",
                EmailConfirmed = true,
                PhoneNumber = "123456789"
            };
            

            if (userManager.Users.Where(u => u.UserName == adminUser.UserName).Count() == 0)
            {
                var result = await userManager.CreateAsync(adminUser,"Password1$");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            var gestor = new IdentityUser
            {
                UserName = "gestor1@airbnb.pt",
                Email = "gestor1@airbnb.pt",
                EmailConfirmed = true,
            };
            if (userManager.Users.Where(u => u.UserName == gestor.UserName).Count() == 0)
            {
                var result = await userManager.CreateAsync(gestor, "Password1$");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(gestor, "Gestor");
                    var utilizador = new Utilizador
                    {
                        email = gestor.Email,
                        IdentityUserId = context.Users.Where(u => u.UserName.CompareTo(gestor.UserName) == 0).First().Id,
                    };
                    context.Add(utilizador);
                    await context.SaveChangesAsync();
                }
            }
            var cliente = new IdentityUser
            {
                UserName = "cliente1@airbnb.pt",
                Email = "cliente1@airbnb.pt",
                EmailConfirmed = true,
            };
            if (userManager.Users.Where(u => u.UserName == cliente.UserName).Count() == 0)
            {
                var result = await userManager.CreateAsync(cliente, "Password1$");
                if (result.Succeeded)
                {
              
                    await userManager.AddToRoleAsync(cliente, "Cliente");
                    var utilizador = new Utilizador
                    {
                        email = cliente.Email,
                        IdentityUserId = context.Users.Where(u => u.UserName.CompareTo(cliente.UserName) == 0).First().Id,
                    };
                    context.Add(utilizador);
                    await context.SaveChangesAsync();
                }
            }
            var funcionario = new IdentityUser
            {
                UserName = "funcionario1@airbnb.pt",
                Email = "funcionario1@airbnb.pt",
                EmailConfirmed = true,
            };
            if (userManager.Users.Where(u => u.UserName == funcionario.UserName).Count() == 0)
            {
                var result = await userManager.CreateAsync(funcionario, "Password1$");
                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(funcionario, "Funcionario");
                    var utilizador = new Funcionario
                    {
                        email = funcionario.Email,
                        IdentityUserId = context.Users.Where(u => u.UserName.CompareTo(funcionario.UserName) == 0).First().Id,
                        GestorId = context.Users.Where(u => u.UserName.CompareTo(gestor.Email) == 0).First().Id,
                    };
                    context.Add(utilizador);
                    await context.SaveChangesAsync();
                }
            }


        }
    }
}
