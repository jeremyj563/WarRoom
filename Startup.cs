using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace WarRoom {
    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());

            if (HybridSupport.IsElectronActive) {
                ElectronBootstrap();
            }
        }

        public async void ElectronBootstrap() {
            var options = this.NewBrowserWindowOptions();
            var window = await Electron.WindowManager.CreateWindowAsync(options);
            await window.WebContents.Session.ClearCacheAsync();
            window.OnReadyToShow += window.Show;
            window.SetTitle("Electron.NET API Demos");
        }

        public BrowserWindowOptions NewBrowserWindowOptions() {
            return new BrowserWindowOptions {
                Width = 1152,
                Height = 940,
                Show = false
            };
        }
    }
}
