namespace INIManager.Components.Models;

public class Workstation
{
    public int Id { get; set; }
    public string Bezeichnung { get; set; } = null!;
    public WorkstationType WorkstationType { get; set; } = null!;
    public float MinGuiSampleTimeInMs { get; set; }
    public int HighActive { get; set; }
    public string TcAdsDiAddress { get; set; } = null!;
    public string TcAdsDiAdsPort { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Sequence { get; set; }
    public bool IsSaved { get; set; }
}