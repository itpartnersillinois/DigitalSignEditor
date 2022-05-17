using System;
using DigitalSignEditor.Calendar;
using DigitalSignEditor.Data;
using DigitalSignEditor.Emergency;
using DigitalSignEditor.GithubExport;
using DigitalSignEditor.Helpers;
using DigitalSignEditor.Twitter;
using DigitalSignEditor.Weather;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace DigitalSignEditor {

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddRazorPages().AddMvcOptions(options => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDbContextFactory<SignContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AppConnection")).EnableSensitiveDataLogging(true));

            services.AddScoped<ISignRepository, SignRepository>(sp => new SignRepository(sp.GetRequiredService<IDbContextFactory<SignContext>>()));

            services.AddScoped<ISecurityHelper, SecurityHelper>(sp => new SecurityHelper(sp.GetRequiredService<ISignRepository>(), Configuration.GetValue<string>("AdminList")));

            services.AddSingleton(access => new EmergencyContainer(EmergencyChecker.Check));

            services.AddSingleton(access => new WeatherHelper(WeatherAccess.GetWeather));

            services.AddSingleton(access => new TwitterHelper(Configuration.GetValue<string>("Twitter:ConsumerKey"),
                Configuration.GetValue<string>("Twitter:ConsumerSecret"),
                Configuration.GetValue<string>("Twitter:OAuthToken"),
                Configuration.GetValue<string>("Twitter:OAuthTokenSecret")));

            services.AddScoped<ITwitterCache, TwitterCache>(sp => new TwitterCache(sp.GetRequiredService<ISignRepository>()));

            services.AddScoped(sp => new CalendarHelper(WebAccess.GetCalenderJson));
            services.AddScoped(sp => new CalendarIcsHelper(WebAccess.GetCalenderIcs));
            services.AddScoped<IFileCreatorFactory>(sp => new FileCreatorFactory(Configuration.GetValue<string>("Github:Owner"),
                Configuration.GetValue<string>("Github:Repository"),
                Configuration.GetValue<string>("Github:Token"),
                Configuration.GetValue<string>("DigitalSignUrl"),
                ImageHelper.Resize));
            services.AddScoped<Func<byte[], int, int, int, int, int, string>>(sp => ImageHelper.IsImageValid);
        }
    }
}