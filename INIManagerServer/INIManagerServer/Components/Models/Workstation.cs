namespace INIManagerServer.Components.Models;

public class Workstation
{
    public Workstation() {}
    
    public Workstation(int id, string name, string descriptioin)
    {
        Id = id;
        Name = name;
        Description = descriptioin;
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}