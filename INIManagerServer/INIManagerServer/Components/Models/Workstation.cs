namespace INIManagerServer.Components.Models;

public class Workstation
{
    public Workstation() {}
    
    public Workstation(int id, string name, string descriptioin, bool isSaved)
    {
        Id = id;
        Name = name;
        Description = descriptioin;
        IsSaved = isSaved;
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsSaved { get; set; }
}