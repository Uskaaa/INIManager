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
│   ├── Layout/           # MainLayout, NavMenu, CSS
│   ├── Pages/            # Home, Configurator, Settings, Dialogs
│   │   ├── Components/   # Blazor Komponenten
│   │   └── Dialogs/
│   ├── Models/           # Datenmodelle (Configuration, Workstation, etc.)
│   ├── Services/         # Backend-Services (Konfiguration, Export, Locking, etc.)
│   │   └── Interfaces/   # Interfaces für Klassenimplementierungen
│   └── Database/
│
├── wwwroot/
│   ├── js/               # JavaScript für Interaktivität (Drag & Drop, Navbar, etc.)
│   ├── css/              # Statische CSS-Dateien
│   └── lib/              # Drittanbieter-Bibliotheken
│
├── Program.cs            # Einstiegspunkt, Service-Konfiguration
├── INIManager.csproj     # Projektdatei, NuGet-Abhängigkeiten
└── appsettings.json      # Konfiguration (z.B. DB-Connection)
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

# 🐳 Installation & Setup (Docker)

## 1. Voraussetzungen
* Docker ist installiert.
* Zugriff auf GitHub Packages ist vorhanden (für das Docker-Image).

## 2. Release herunterladen und entpacken

1. Gehe zu den [Releases](https://github.com/Uskaaa/INIManager/releases) auf GitHub
2. Lade die neueste Release-Version herunter (z.B. `INIManager-v1.0.0.zip`)
3. Entpacke das Archiv in einen Ordner deiner Wahl
4. Navigiere in den entpackten Ordner:

```bash
cd INIManager-v1.0.0
```

## 3. `.env`-Datei anpassen
Passe die `.env`-Datei an, welche sich im selben Verzeichnis wie die `docker-compose.yml` befindet.

**Beispiel `.env`:**

```env
MYSQL_DATABASE=inimanager_db
MYSQL_ROOT_PASSWORD=CHANGEME
```

🔒 **Hinweis:** Verwende niemals echte Passwörter in einem öffentlichen Repository. Diese `.env`-Datei dient nur als Beispiel.

## 4. Docker-Container starten
Führe den folgenden Befehl aus, um die Container im Hintergrund zu starten:

```bash
docker compose up -d
```

Dieser Befehl startet zwei Dienste:
* Eine **MySQL-Datenbank** (`mysql:8`)
* Den **INIManager** aus der GitHub Container Registry (`ghcr.io/uskaaa/inimanager:latest`)

## 5. Anwendung im Browser öffnen
Die Anwendung ist standardmäßig unter der folgenden Adresse erreichbar: http://localhost:8080
