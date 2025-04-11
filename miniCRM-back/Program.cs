
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.Services;
using System.Text.Json.Serialization;

namespace miniCRM_back {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            var corsOrigins = "mini-CRM origins";

            builder.Services.AddCors(options => {
                options.AddPolicy(name: corsOrigins,
                                  policy => {
                                      policy //.WithOrigins("https://localhost:5173",
                                      //                    "http://localhost:5173",
                                      //                    "https://localhost:4200",
                                      //                    "http://localhost:4200")
                                      .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                                  });
            });

            builder.Services.AddApiVersioning(options => {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"),
                    new MediaTypeApiVersionReader("v")
                );
            })
            .AddApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            // for russian characters
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // In Program.cs or Startup.cs
            builder.Services.AddDbContext<crmDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperProfile>());
            // my DI
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));


            // avoid cyclic issues
            builder.Services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddOpenApiDocument();

            var app = builder.Build();

            if (app.Environment.IsDevelopment()) {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseCors(corsOrigins);

            app.Run();
        }
    }
}