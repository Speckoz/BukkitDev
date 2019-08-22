using Speckoz.BukkitDev.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Models;
using System;

namespace Speckoz.BukkitDev
{
    public class Startup
    {
        public Startup(IConfiguration configurations)
        {
            Configuration = configurations;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.Configure<CookiePolicyOptions>(options =>
              {
                  // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                  options.CheckConsentNeeded = context => true;
                  options.MinimumSameSitePolicy = SameSiteMode.None;
              });

            _ = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            _ = services.AddDbContext<BukkitDevSystemContext>
                (
                options => options.UseMySql(Configuration.GetConnectionString("BukkitDevSystemContext"),
                builder => builder.MigrationsAssembly("BukkitDevSystem"))
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
