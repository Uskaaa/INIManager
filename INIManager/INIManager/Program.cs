using INIManager.Components.Database;
using INIManager.Components.Services;
using INIManager.Components.Services.Interfaces;
using INIManager.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MudBlazor.Services;
using Radzen;

namespace INIManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents().AddCircuitOptions(options => { options.DetailedErrors = true; });

        //Add Radzen and MudBlazor Service
        builder.Services.AddRadzenComponents();
        builder.Services.AddMudServices();

        var server = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
        var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "inimanager_db";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

        var connectionString = $"Server={server};Port={port};Database={database};Uid={user};Pwd={password};";
        
        builder.Services.AddScoped<DbConnector>(sp =>
            new DbConnector(connectionString));
        builder.Services.AddScoped<ProtectedLocalStorage>();

        // builder.Services.AddScoped<IAdoService>(provider =>
        // {
        //     var repoUrl = "https://dev.azure.com/yourorg/yourproject/_git/yourrepo";
        //     var pat = builder.Configuration["AzureDevOps:PersonalAccessToken"]!;
        //     var workstationService = provider.GetRequiredService<IWorkstationService>();
        //     return new AdoService(repoUrl, pat, workstationService);
        // });
        
        builder.Services.AddScoped<IAdoService, AdoService>();
        builder.Services.AddScoped<ConfigurationService>();
        builder.Services.AddScoped<IWorkstationService, WorkstationService>();
        builder.Services.AddScoped<IExportService, ExportService>();
        builder.Services.AddScoped<ConfigurationDraftService>();
        builder.Services.AddScoped<SetSavedService>();
        builder.Services.AddSingleton<ILockService, LockService>();
        builder.Services.AddMemoryCache();
        builder.Services.AddSignalR();
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapRazorPages();
        app.MapHub<ConfigurationHub>("/configurationHub");

        app.Run();
    }
}