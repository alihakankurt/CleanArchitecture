using Application.Persistence.Repositories;
using Core.Application.Persistence;
using Core.Application.Services;
using Core.Infrastructure.Persistence;
using Core.Infrastructure.Persistence.EntityFramework.Interceptors;
using Core.Infrastructure.Services;
using Infrastructure.Persistence.EntityFramework;
using Infrastructure.Persistence.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistrar
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EfDatabaseContext>((sp, options) =>
            options.UseSqlite(configuration.GetConnectionString("SqliteConnectionString"),
                static (options) => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .AddInterceptors(sp.GetServices<SaveChangesInterceptor>()));

        services.AddScoped<SaveChangesInterceptor, CreationAuditInterceptor>();
        services.AddScoped<SaveChangesInterceptor, UpdateAuditInterceptor>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork<EfDatabaseContext>>();

        services.AddScoped<IUserRepository, EfUserRepository>();

        services.AddScoped<IDateTimeService, UtcDateTimeService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}
