
using Asp.Versioning;
using miniCRM_back.Configs;

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

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperProfile>());


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApiDocument();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                //app.MapOpenApi();
                app.UseOpenApi();
                // Add web UIs to interact with the document
                // Available at: http://localhost:<port>/swagger
                app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseCors(corsOrigins);

            app.Run();
        }
    }
}