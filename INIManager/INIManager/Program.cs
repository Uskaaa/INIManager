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

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents().AddCircuitOptions(options => { options.DetailedErrors = true; });

        //Add Radzen and MudBlazor Service
        builder.Services.AddRadzenComponents();
        builder.Services.AddMudServices();

        string connectionString = "Server=localhost;Port=3306;Database=inimanager_db;Uid=root;Pwd=;";
        builder.Services.AddScoped<DbConnector>(sp =>
            new DbConnector(connectionString));
        builder.Services.AddScoped<DbManager>();

        builder.Services.AddScoped<ProtectedLocalStorage>();
        builder.Services.AddSignalR();
        builder.Services.AddRazorPages();

        builder.Services.AddScoped<AdoService>();
        builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
        builder.Services.AddScoped<WorkstationService>();
        builder.Services.AddScoped<ConfigurationDraftService>();
        builder.Services.AddScoped<SetSavedService>();

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