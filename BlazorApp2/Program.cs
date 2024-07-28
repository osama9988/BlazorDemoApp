using BlazorApp2.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;

namespace BlazorApp2
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

            builder.Services.AddTransient<AuthorizationMessageHandler>();

            builder.Services.AddTransient<LoginService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddHttpClient<ILoginServiceBackendApiHttpClient, LoginServiceBackendApiHttpClient>(options =>
            {
                options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:BackendApi"));
                options.Timeout = TimeSpan.FromSeconds(30);
                options.DefaultRequestHeaders.TryAddWithoutValidation("Service", Assembly.GetAssembly(typeof(Program))?.GetName().Name);
            });
            builder.Services.AddHttpClient<ApiService>(client => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:BackendApi")); }) // Your API base address
                 ;  // add auth headers 

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
