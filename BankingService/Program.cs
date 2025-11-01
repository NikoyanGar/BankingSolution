using BankingService.Clients;
using BankingService.Middlewares;
using BankingService.Services;
using BankingService.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BankingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidAudience = configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Services
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IBankingService>();

            builder.Services.AddHttpClient<IScoringClient, ScoringHttpClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001");
            });

            builder.Services.AddHttpClient<IClientLoanClient, ClientLoanHttpClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5002");
            });

            builder.Services.AddControllers();
            builder.Services.AddValidatorsFromAssemblyContaining<EvaluateLoanValidator>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
