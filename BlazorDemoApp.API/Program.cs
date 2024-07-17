



using BlazorDemoApp.API.Services.Mappings;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
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
            builder.Services.AddSwaggerGen();


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


            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo> { new CultureInfo("ar-EG"),new CultureInfo("en-US"), new CultureInfo("fr-FR"), };

                options.DefaultRequestCulture = new RequestCulture("ar-EG");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // Optionally, set fallback to default culture if no match found
                options.FallBackToParentCultures = true;
                options.FallBackToParentUICultures = true;
            });
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ar-EG");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseRequestCulture(); // Add the middleware here

            app.MapControllers();

            app.Run();
        }
    }
}
