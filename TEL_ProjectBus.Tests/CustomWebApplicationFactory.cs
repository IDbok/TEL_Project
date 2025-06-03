using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TEL_ProjectBus.DAL.DbContext;
using MassTransit;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using TEL_ProjectBus.DAL.Entities;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TEL_ProjectBus.Tests;

public class CustomWebApplicationFactory
: WebApplicationFactory<Program> // Adjusted to reference the Program class directly  
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        /* ---------- 1. подменяем конфигурацию ---------- */
        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            // Переопределяем только то, что нужно для тестов
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["UseInMemoryTransport"] = "true",      // MassTransit → In-Memory
                ["ConnectionStrings:DefaultConnection"] = "InMemoryDb", // будет перехвачено ниже
                ["JwtSettings:Secret"] = "UnitTestSecretUnitTestSecret123!"
            });

            // <-- Хук, который выполняется после того, как контейнер собран
            builder.UseSetting("SeedDb", "true");
        });

        /* ---------- 2. подменяем сервисы ---------------- */
        builder.ConfigureServices(async services =>
        {
            /* --- EF Core: In-Memory -------------------- */
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(AppDbContext));
            //var dbDescriptor = services.Single(
            //    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            //services.Remove(dbDescriptor);


            ///* --- Запускаем тесты с одной базой --- */
            //services.AddDbContext<AppDbContext>(opt =>
            //    opt.UseInMemoryDatabase("UnitTestsDb"));

            /* --- Запускаем тесты паралельно (с уникальным именем БД) --- */
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase($"UnitTestsDb_{Guid.NewGuid()}"));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var um = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var rm = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            await DbInitializer.Seed(ctx, um, rm,
                recreateDb: true,   // чтобы каждый прогон начинался с чистой БД
                loadTestData: true);

            /* --- Встраиваем TestHarness (MassTransit.InMemory) ------------- */
            services.AddMassTransitTestHarness();
        });
    }
}
