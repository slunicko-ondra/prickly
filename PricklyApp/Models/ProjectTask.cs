using LiteDB;

namespace PricklyApp.Models;

public class ProjectTask
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public List<WorkInterval> WorkIntervals { get; set; }
    
    public ProjectTask(string name)
    {
        Id = ObjectId.NewObjectId();
        Name = name;
        WorkIntervals = new List<WorkInterval>();
    }
}