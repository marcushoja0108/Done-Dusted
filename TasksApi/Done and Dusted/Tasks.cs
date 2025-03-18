namespace WebApplication1;

public class Tasks
{
    public int id { get; set; }
    public DateTime doDate { get; set; }
    public TimeSpan doTime { get; set; }
    public string repeats { get; set; }
    public string title { get; set; }
    public string summary { get; set; }
    public bool done { get; set; }
    public DateTime? doneDate { get; set; }
    public TimeSpan? doneTime { get; set; }
}