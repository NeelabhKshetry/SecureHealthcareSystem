using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using HealthcareSystemDesign.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HealthcareSystemDesign.Models;
using HealthcareSystemDesign.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using Stripe;
using Rotativa.AspNetCore;

namespace HealthcareSystemDesign
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

            //Hangfire for recurring jobs
            services.AddHangfire(config =>
              config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
              .UseSimpleAssemblyNameTypeSerializer()
              .UseDefaultTypeSerializer()
              .UseMemoryStorage());

            services.AddHangfireServer();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<healthcareContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DataConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            //Support sendgrid account confirmation and password reset
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddRazorPages();
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
        }
        //Create admin role
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


            //Adding Admin Role and default admin user
            var roleCheck = await RoleManager.RoleExistsAsync("CEO");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(new IdentityRole("CEO"));
            }
            var admin = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@myhealthcare.com",
                EmailConfirmed = true,


            };

            var user = await UserManager.FindByEmailAsync("admin@myhealthcare.com");
            if (user == null)
            {
                var createpoweruser = await UserManager.CreateAsync(admin, "Superuser1!");
                if (createpoweruser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, "CEO");
                }

            }
            //Staff Role and generic staff email
            var staffCheck = await RoleManager.RoleExistsAsync("Staff");
            if (!staffCheck)
            {
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(new IdentityRole("Staff"));
            }
            var staff = new IdentityUser
            {
                UserName = "staff",
                Email = "staff@myhealthcare.com",
                EmailConfirmed = true,


            };

            var staffuser = await UserManager.FindByEmailAsync("staff@myhealthcare.com");
            if (staffuser == null)
            {
                var createpoweruser = await UserManager.CreateAsync(staff, "Superuser1!");
                if (createpoweruser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, "Staff");
                }

            }

            //Doctor role and generic staff addition
            var doctorCheck = await RoleManager.RoleExistsAsync("Doctor");
            if (!doctorCheck)
            {
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(new IdentityRole("Doctor"));
            }
            var doctor = new IdentityUser
            {
                UserName = "doctor@myhealthcare.com",
                Email = "doctor@myhealthcare.com",
                EmailConfirmed = true,


            };

            var doctoruser = await UserManager.FindByEmailAsync("doctor@myhealthcare.com");
            if (doctoruser == null)
            {
                var createpoweruser = await UserManager.CreateAsync(doctor, "Superuser1!");
                if (createpoweruser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, "Doctor");
                }

            }
            //Nurse

            var nurseCheck = await RoleManager.RoleExistsAsync("Nurse");
            if (!nurseCheck)
            {
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(new IdentityRole("Nurse"));
            }
            var nurse = new IdentityUser
            {
                UserName = "nurse",
                Email = "nurse@myhealthcare.com",
                EmailConfirmed = true,


            };

            var nurseuser = await UserManager.FindByEmailAsync("nurse@myhealthcare.com");
            if (nurseuser == null)
            {
                var createpoweruser = await UserManager.CreateAsync(nurse, "Superuser1!");
                if (createpoweruser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(nurse, "Nurse");
                }

            }
            // Patient, create only patient role
            var patientroleCheck = await RoleManager.RoleExistsAsync("Patient");
            if (!patientroleCheck)
            {
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(new IdentityRole("Patient"));
            }

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            
            //Stripe Payment
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            //Create user roles
            CreateUserRoles(serviceProvider).Wait();

            RotativaConfiguration.Setup(env.ContentRootPath, "wwwroot/Rotativa");
            app.UseHangfireDashboard();

            //backgroundJobClient.Enqueue<JobScheduling>(x => x.ClearAppointment());
            recurringJobManager.AddOrUpdate<JobScheduling>("Clear Appointment", x => x.ClearAppointment(), Cron.Daily(00, 08));

        }
    }
}
