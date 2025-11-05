using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AbpSolution1.Data;
using Serilog;
using Volo.Abp;
using Volo.Abp.Data;

namespace AbpSolution1.DbMigrator;

public class DbMigratorHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfiguration _configuration;

    public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Validate connection string early and give a clear message
        var defaultConn = _configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(defaultConn))
        {
            // Try fallback: read appsettings.json from current directory
            try
            {
                var basePath = Directory.GetCurrentDirectory();
                var jsonPath = Path.Combine(basePath, "appsettings.json");
                if (File.Exists(jsonPath))
                {
                    using var doc = JsonDocument.Parse(File.ReadAllText(jsonPath));
                    if (doc.RootElement.TryGetProperty("ConnectionStrings", out var cs) && cs.TryGetProperty("Default", out var def))
                    {
                        defaultConn = def.GetString();
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        if (string.IsNullOrWhiteSpace(defaultConn))
        {
            Log.Logger.Fatal("Connection string 'Default' is not configured in DbMigrator appsettings. Please set ConnectionStrings:Default.");
            // Stop application to avoid confusing runtime exception later
            _hostApplicationLifetime.StopApplication();
            return;
        }

        using (var application = await AbpApplicationFactory.CreateAsync<AbpSolution1DbMigratorModule>(options =>
        {
           options.Services.ReplaceConfiguration(_configuration);
           options.UseAutofac();
           options.Services.AddLogging(c => c.AddSerilog());
           options.AddDataMigrationEnvironment();
        }))
        {
            await application.InitializeAsync();

            await application
                .ServiceProvider
                .GetRequiredService<AbpSolution1DbMigrationService>()
                .MigrateAsync();

            await application.ShutdownAsync();

            _hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
