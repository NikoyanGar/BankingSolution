using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserService.Auth;
using UserService.Data;
using UserService.Middlewares;
using UserService.Options;
using UserService.Repositories;
using UserService.Services;
using UserService.Validators;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("DbConnection"));
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddOptions<DbConnectionOptions>().Bind(builder.Configuration.GetSection("DbConnectionOptions"));

            //Read from configuration to build connection string
            var dbOptions = builder.Configuration.GetSection("DbConnection").Get<DbConnectionOptions>();
            var connectionString =
                $"Host={dbOptions?.Host};Port={dbOptions?.Port};Database={dbOptions?.Database};Username={dbOptions?.Username};Password={dbOptions?.Password}";

            // Add services to the container.
            builder.Services.AddDbContext<UserDbContext>(optionsAction: options => options.UseNpgsql(connectionString));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserCrudService, UserCrudService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddControllers();

            builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationValidator>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
