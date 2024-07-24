using BlazorDemoApp.BlazorServer2.Data;
using BlazorDemoApp.BlazorServer2.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDemoApp.BlazorServer2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load configuration from appsettings.json
            var configuration = builder.Configuration;
            builder.Services.AddSingleton(configuration);
            var apiBaseAddress = configuration["ApiBaseAddress"];

            builder.Services.AddBlazoredSessionStorage(); // Add this line

            builder.Services.AddAuthenticationCore();
            builder.Services.AddAuthorization();

            builder.Services.AddLogging();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddHttpClient("YourApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7249/");
            });
            builder.Services.AddHttpClient<ApiService>(client =>{client.BaseAddress = new Uri(apiBaseAddress); }); // Your API base address
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddHttpClient<CustomAuthenticationStateProvider>(client => { client.BaseAddress = new Uri(apiBaseAddress); }); // Your API base address
            builder.Services.AddHttpClient<AuthService>(client => { client.BaseAddress = new Uri(apiBaseAddress); }); // Your API base address
     


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
