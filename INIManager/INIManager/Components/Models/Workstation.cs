namespace INIManager.Components.Models;

public class Workstation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Sequence { get; set; }
    public bool IsSaved { get; set; }
}