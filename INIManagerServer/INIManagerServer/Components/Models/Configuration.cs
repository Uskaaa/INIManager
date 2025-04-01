namespace INIManagerServer.Components.Models;

public class Configuration
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public List<Workstation> AmountOfWS { get; set; }
    public string CreationDate { get; set; }
}