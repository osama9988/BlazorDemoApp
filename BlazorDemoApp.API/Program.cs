



using BlazorDemoApp.API.Services.Mappings;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace BlazorDemoApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("MyAppConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                 c.OperationFilter<AddAcceptLanguageHeaderParameter>();
            });

            //
            var assembly = Assembly.GetExecutingAssembly();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDataProtection();
            ////
            builder.Services.AddAutoMapper(cfg =>
            {

                cfg.AddProfile(new MapOf_Add(builder.Services.BuildServiceProvider().GetRequiredService<IDataProtectionProvider>()));
            });
            //


            //Delete checkers
            ServiceRegistration.Register_DeleteCheckers(builder.Services, assembly);
            //Delete checkers
            ServiceRegistration.Register_BaseOfServiceV2(builder.Services, assembly);


            const string defaultCulture = "ar";

            var supportedCultures = new[] {new CultureInfo(defaultCulture), new CultureInfo("en")};

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();

            app.UseRequestCulture(); // Add the middleware here

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            
            //app.UseRequestLocalization(options => options.AddSupportedCultures("en", "ar").AddSupportedUICultures("en", "ar").SetDefaultCulture("ar"));
            app.UseRequestLocalization();
            

            app.MapControllers();

            app.Run();
        }
    }
}
