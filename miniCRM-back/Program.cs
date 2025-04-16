
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models.Auth;
using miniCRM_back.Services;
using miniCRM_back.Services.Contracts;
using System.Text;
using System.Text.Json;
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

            // authentication
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
                options.Events = new JwtBearerEvents {
                    OnChallenge = async context => {
                        // Предотвращаем выполнение стандартного обработчика
                        context.HandleResponse();

                        // Устанавливаем код 401
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = ApiResponse<object>.ErrorResponse(
                            "AUTH_TOKEN_INVALID",
                            "Токен авторизации недействителен или истёк"
                        );

                        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                    }
                };
            });

            builder.Services.AddScoped<IAuthService, AuthService>();

            // In Program.cs or Startup.cs
            builder.Services.AddDbContext<crmDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperProfile>());
            // my DI
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(ITaskRepository), typeof(TaskRepository));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            // services
            builder.Services.AddScoped(typeof(IBaseService<,,,>), typeof(BaseService<,,,>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskItemService, TaskItemService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IReportService, ReportService>();

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