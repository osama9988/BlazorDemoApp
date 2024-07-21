using BlazorDemoApp.BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDemoApp.BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();


            builder.Services.AddScoped<CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<AuthenticatedHttpClientHandler>();

            // Load configuration from appsettings.json
            var configuration = builder.Configuration;
            builder.Services.AddSingleton(configuration);

            builder.Services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<AuthenticatedHttpClientHandler>();
                var apiBaseAddress = configuration["ApiBaseAddress"];
                var client = new HttpClient(handler) { BaseAddress = new Uri(apiBaseAddress) };
                return client;
            });

            builder.Services.AddScoped<AuthService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
