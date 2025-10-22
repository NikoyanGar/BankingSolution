using ClientLoanServiceAPI.Middlewares.Extensions;
using ClientLoanServiceAPI.Repositories;
using ClientLoanServiceAPI.Services;
using ClientLoanServiceAPI.Validators;
using FluentValidation;

namespace ClientLoanServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("RedisConfiguration:Host").Value;
                options.InstanceName = builder.Configuration.GetSection("RedisConfiguration:InstanceName").Value;
            });

            builder.Services.AddScoped<ILoanHistoryRepository, LoanHistoryRepository>();
            builder.Services.AddScoped<ILoanHistoryService, LoanHistoryService>();

            builder.Services.AddControllers();
            builder.Services.AddValidatorsFromAssemblyContaining<LoanHistoryValidator>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseGlobalExceptionHandler();
            app.UseRequestLogging();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
