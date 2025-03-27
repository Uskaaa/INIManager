using INIManager.Components.Database;
using Microsoft.Extensions.Logging;
using Radzen;

namespace INIManager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        //Add Radzen and MudBlazor services
        builder.Services.AddRadzenComponents();

        string connectionString = "Server=localhost;Port=3306;Database=inimanager_db;Uid=root;Pwd=;";
        builder.Services.AddSingleton(new DbConnector(connectionString));
        builder.Services.AddSingleton<DbManager>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}