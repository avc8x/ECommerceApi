using ECommerceApi.Application.Common.Interfaces;
using ECommerceApi.Infrastructure.Caching;
using ECommerceApi.Infrastructure.Persistence;
using ECommerceApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ECommerceApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // SQL Server
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());

        // Redis - optional, falls back to no-op cache if unavailable
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";

        try
        {
            var configOptions = ConfigurationOptions.Parse(redisConnectionString);
            configOptions.AbortOnConnectFail = false;
            configOptions.ConnectTimeout = 3000;
            configOptions.ConnectRetry = 1;

            var multiplexer = ConnectionMultiplexer.Connect(configOptions);

            if (multiplexer.IsConnected)
            {
                services.AddSingleton<IConnectionMultiplexer>(multiplexer);
                services.AddScoped<ICacheService, RedisCacheService>();
            }
            else
            {
                services.AddSingleton<ICacheService, NoOpCacheService>();
                Console.WriteLine("[Cache] Redis not available - using no-op cache (data will not be cached).");
            }
        }
        catch
        {
            services.AddSingleton<ICacheService, NoOpCacheService>();
            Console.WriteLine("[Cache] Redis not available - using no-op cache (data will not be cached).");
        }

        // Slug service
        services.AddScoped<ISlugService, SlugService>();

        return services;
    }
}
