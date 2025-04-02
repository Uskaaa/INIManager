namespace INIManagerServer.Components.Models;

public class Configuration
{
    public int Id { get; set; }
    public string Bezeichnung { get; set; }
    public List<Workstation> Workstations { get; set; }
    public string DateOfCreation { get; set; }
    public bool IsDraft { get; set; }
}