using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoringServiceAPI.Data;
using ScoringServiceAPI.Middlewares;
using ScoringServiceAPI.Options;
using ScoringServiceAPI.Services;
using ScoringServiceAPI.Validators;

namespace ScoringServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("DbConnection"));

            builder.Services.AddOptions<DbConnectionOptions>().Bind(builder.Configuration.GetSection("DbConnectionOptions"));

            //Read from configuration to build connection string
            var dbOptions = builder.Configuration.GetSection("DbConnection").Get<DbConnectionOptions>();
            var connectionString =
                $"Host={dbOptions?.Host};Port={dbOptions?.Port};Database={dbOptions?.Database};Username={dbOptions?.Username};Password={dbOptions?.Password}";

            // Add services to the container.
            builder.Services.AddDbContext<ScoringDbContext>(optionsAction: options =>
            {
                options.UseNpgsql(connectionString);
            });

            // Repos & Services
            builder.Services.AddScoped<ScoreService>();

            builder.Services.AddControllers();
            builder.Services.AddValidatorsFromAssemblyContaining<AddScoreValidator>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.UseRouting();
            app.MapControllers();

            app.Run();
        }

    }
}
