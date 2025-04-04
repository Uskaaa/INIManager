using INIManagerServer.Components;
using INIManagerServer.Components.Database;
using INIManagerServer.Components.Services;
using INIManagerServer.Components.Services.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MudBlazor.Services;
using Radzen;

namespace INIManagerServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents().AddCircuitOptions(options =>
            {
                options.DetailedErrors = true;
            });
        
        //Add Radzen and MudBlazor Service
        builder.Services.AddRadzenComponents();
        builder.Services.AddMudServices();

        string connectionString = "Server=localhost;Port=3306;Database=inimanager_db;Uid=root;Pwd=;";
        builder.Services.AddSingleton(new DbConnector(connectionString));
        builder.Services.AddSingleton<DbManager>();
        
        builder.Services.AddScoped<ProtectedLocalStorage>();
        
        builder.Services.AddScoped<AdoService>();
        builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
        builder.Services.AddScoped<WorkstationService>();
        builder.Services.AddScoped<ConfigurationDraftService>();
        
        

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

        app.Run();
    }
}