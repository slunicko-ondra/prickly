using LiteDB;

namespace PricklyApp.Models;

public class DatabaseManager
{
    private string _path;
    
    public DatabaseManager(string path)
    {
        _path = path;
    }

    public void AddProject(string name)
    {
        var project = new Project(name);
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        projects.Insert(project);
        projects.EnsureIndex(p => p.Name, true);
    }
    
    public void AddTask(string projectName, string taskName)
    {
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        var project = projects.FindOne(p => p.Name == projectName);
        if (project == null)
        {
            throw new ArgumentException("Project not found.");
        }
        project.Tasks.Add(new ProjectTask(taskName));
        projects.Update(project);
    }
    
    public void AddInterval(string projectName, string taskName, WorkInterval interval)
    {
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        var project = projects.FindOne(p => p.Name == projectName);
        if (project == null)
        {
            throw new ArgumentException("Project not found.");
        }
        var task = project.Tasks.Find(t => t.Name == taskName);
        if (task == null)
        {
            throw new ArgumentException("Task not found.");
        }
        task.WorkIntervals.Add(interval);
        projects.Update(project);
    }
    
    public List<Project> GetProjects()
    {
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        return projects.FindAll().ToList();
    }
    
    public List<ProjectTask> GetTasks(string projectName)
    {
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        var project = projects.FindOne(p => p.Name == projectName);
        if (project == null)
        {
            throw new ArgumentException("Project not found.");
        }
        return project.Tasks;
    }
    
    public List<WorkInterval> GetIntervals(string projectName, string taskName)
    {
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        var project = projects.FindOne(p => p.Name == projectName);
        if (project == null)
        {
            throw new ArgumentException("Project not found.");
        }
        var task = project.Tasks.Find(t => t.Name == taskName);
        if (task == null)
        {
            throw new ArgumentException("Task not found.");
        }
        return task.WorkIntervals;
    }
}