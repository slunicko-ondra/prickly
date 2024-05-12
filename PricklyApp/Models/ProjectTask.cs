using LiteDB;

namespace PricklyApp.Models;

public class ProjectTask
{
    public ObjectId Id { get; }
    public string Name { get; set; }
    public List<WorkInterval> WorkIntervals { get; }
    
    public ProjectTask(string name)
    {
        Id = ObjectId.NewObjectId();
        Name = name;
        WorkIntervals = new List<WorkInterval>();
    }
}