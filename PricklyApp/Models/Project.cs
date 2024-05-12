using LiteDB;

namespace PricklyApp.Models;

public class Project
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public List<ProjectTask> Tasks { get; set; }
    
    public Project(string name)
    {
        Id = ObjectId.NewObjectId();
        Name = name;
        Tasks = new List<ProjectTask>();
        Tasks.Add(new ProjectTask("default"));
    }
    
    public Project(string name, List<ProjectTask> tasks)
    {
        if (tasks.Distinct().Count() != tasks.Count)
        {
            throw new ArgumentException("Tasks must be unique.");
        }
        Id = ObjectId.NewObjectId();
        Name = name;
        Tasks = tasks;
        Tasks.Insert(0, new ProjectTask("default"));
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