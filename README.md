# INIManager

**INIManager** ist eine moderne, interaktive Webanwendung zur Verwaltung, Konfiguration und Organisation von Workstations und deren INI-Konfigurationsdateien. Die Anwendung richtet sich an Administratoren und Entwickler, die komplexe Konfigurationsszenarien effizient und übersichtlich abbilden möchten.

---

## Features

- **Konfigurationsverwaltung:**  
  Erstellen, Bearbeiten, Löschen und Exportieren von Konfigurationen für verschiedene Workstations.
- **Drag & Drop Konfigurator:**  
  Intuitive Oberfläche zum Zuweisen und Anordnen von Workstations in Konfigurationen.
- **Live-Preview:**  
  Direkte Vorschau der generierten INI-Dateien (Hardware.ini, Params.ini, Defaults.ini, M2kSys.ini) für jede Konfiguration.
- **Drafts & Versionierung:**  
  Entwürfe (Drafts) und fertige Konfigurationen werden getrennt verwaltet.
- **Azure DevOps Integration:**  
  (Optional) Automatisches Einlesen von Workstations aus einem Azure DevOps Repository.
- **Benutzerfreundliches UI:**  
  Moderne Oberfläche mit MudBlazor und Radzen, responsiv und übersichtlich.
- **Echtzeit-Updates:**  
  SignalR sorgt für Live-Aktualisierung der Konfigurationslisten bei Änderungen.
- **Settings:**  
  Anpassbare Einstellungen wie Sprache, Zeitzone, Datenschutz, Benachrichtigungen und Erscheinungsbild (Dark Mode).

---

## Technologie-Stack

- **Frontend & UI:**  
  - [Blazor (ASP.NET Core, .NET 9)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
  - [MudBlazor](https://mudblazor.com/) (UI-Komponenten)
  - [Radzen.Blazor](https://blazor.radzen.com/) (UI-Komponenten)
- **Backend & Services:**  
  - ASP.NET Core Web API
  - SignalR (Echtzeit-Kommunikation)
  - MySQL (Datenbank, via MySqlConnector)
  - Azure DevOps (optional, für Workstation-Import)
- **Sonstiges:**  
  - C#
  - JavaScript (für Drag & Drop, Interaktivität)
  - CSS (Custom Styles)

---

## Projektstruktur

```
INIManager/
│
├── Components/
│   ├── Layout/         # MainLayout, NavMenu, CSS
│   ├── Pages/          # Home, Configurator, Settings, Dialogs
│   ├── Models/         # Datenmodelle (Configuration, Workstation, etc.)
│   ├── Services/       # Backend-Services (Konfiguration, Export, Locking, etc.)
│   └── Database/       # Datenbankzugriff
│
├── wwwroot/
│   ├── js/             # JavaScript für Interaktivität (Drag & Drop, Navbar, etc.)
│   ├── css/            # Statische CSS-Dateien
│   └── lib/            # Drittanbieter-Bibliotheken
│
├── Program.cs          # Einstiegspunkt, Service-Konfiguration
├── INIManager.csproj   # Projektdatei, NuGet-Abhängigkeiten
└── appsettings.json    # Konfiguration (z.B. DB-Connection)
```

---

## Hauptseiten & Funktionen

### Home

- Übersicht über alle Draft- und fertigen Konfigurationen
- Tabellenansicht mit Aktionen: Editieren, Exportieren, Löschen
- Auswahl und Anzeige der zugehörigen Workstations

### Configurator

- Drag & Drop Oberfläche zum Zuweisen von Workstations zu einer Konfiguration
- Live-Preview der resultierenden INI-Dateien in Tabs
- Auto-Save und Statusanzeige
- Aktionen: Speichern als Draft, Final speichern, Exportieren

### Settings

- Allgemeine Einstellungen (Sprache, Zeitzone)
- Datenschutzoptionen
- Benachrichtigungen (E-Mail, Push)
- Erscheinungsbild (Dark Mode, Schriftgröße)

---

## Datenmodell (Beispiel)

```csharp
class Configuration {
    int Id;
    string Bezeichnung;
    string Timestamp;
    List<Workstation> Workstations;
}

class Workstation {
    int Id;
    string Bezeichnung;
    string Description;
    int Sequence;
    // ...
}
```

---

## Installation & Setup

1. **Voraussetzungen:**
   - [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
   - MySQL-Datenbank (Standard: `localhost`, DB: `inimanager_db`)
   - (Optional) Azure DevOps Repo für Workstations

2. **Projekt klonen:**
   ```bash
   git clone https://github.com/dein-benutzername/INIManager.git
   cd INIManager
   ```

3. **Datenbank anpassen:**  
   Passe ggf. die Verbindungsdaten in `Program.cs` oder `appsettings.json` an.

4. **Abhängigkeiten installieren & starten:**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

5. **Im Browser öffnen:**  
   Standardmäßig unter [http://localhost:5000](http://localhost:5000) erreichbar.

---

## Screenshots & Mockups

- Siehe `/docs` oder die SVG-Mockups im Repository für Beispielansichten.

---

## Weiterentwicklung & Mitwirken

Pull Requests, Bug Reports und Feature Requests sind willkommen!  
Bitte beachte die [CONTRIBUTING.md](CONTRIBUTING.md) (sofern vorhanden).

---

## Lizenz

Dieses Projekt steht unter der MIT-Lizenz. Siehe [LICENSE](LICENSE) für Details.

---

**Kontakt:**  
Für Fragen oder Support: [dein.email@domain.de] 