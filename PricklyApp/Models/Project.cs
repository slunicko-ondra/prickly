using LiteDB;

namespace PricklyApp.Models;

public class Project
{
    public ObjectId Id { get; }
    public string Name { get; set; }
    public List<ProjectTask> Tasks { get; }
    
    public Project(string name)
    {
        Id = ObjectId.NewObjectId();
        Name = name;
        Tasks = new List<ProjectTask>();
        Tasks.Add(new ProjectTask("default"));
    }
    
    public void AddTask(string name)
    {
        if (Tasks.Any(t => t.Name == name))
        {
            throw new ArgumentException("Task already exists.");
        }
        Tasks.Add(new ProjectTask(name));
    }
}