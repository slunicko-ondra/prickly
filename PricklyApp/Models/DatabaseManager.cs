using LiteDB;

namespace PricklyApp.Models;

public class DatabaseManager
{
    private string _path;
    
    public DatabaseManager(string path)
    {
        _path = path;
        using var db = new LiteDatabase(_path);
        db.GetCollection<Project>("projects").EnsureIndex(p => p.Name, true);
    }

    public bool AddProject(string name, List<string>? taskNames = null)
    {
        Project project;
        if (taskNames == null)
        {
            project = new Project(name);
        }
        else
        {
            var tasks = taskNames.Select(t => new ProjectTask(t)).ToList();
            project = new Project(name, tasks);
        }
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        if (projects.Exists(p => p.Name == name))
        {
            return false;
        }
        projects.Insert(project);
        return true;
    }
    
    public bool AddTask(string projectName, string taskName)
    {
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        var project = projects.FindOne(p => p.Name == projectName);
        if (project == null)
        {
            throw new ArgumentException("Project not found.");
        }
        try
        {
            project.AddTask(taskName);
        }
        catch (ArgumentException)
        {
            return false;
        }
        projects.Update(project);
        return true;
    }
    
    public bool AddTasks(string projectName, List<string> taskNames)
    {
        using var db = new LiteDatabase(_path);
        var projects = db.GetCollection<Project>("projects");
        var project = projects.FindOne(p => p.Name == projectName);
        if (project == null)
        {
            throw new ArgumentException("Project not found.");
        }
        if (project.Tasks.Select(t => t.Name).Concat(taskNames).GroupBy(name => name).Any(g => g.Count() > 1))
        {
            return false;
        }
        foreach (var taskName in taskNames)
        {
            project.Tasks.Add(new ProjectTask(taskName));
        }
        projects.Update(project);
        return true;
    }
    
    public bool AddInterval(string projectName, string taskName, WorkInterval interval)
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
        return true;
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
    
    public bool StopInterval(string projectName, string taskName, DateTime endTime)
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
        var interval = task.WorkIntervals.LastOrDefault(i => i.End == null);
        if (interval == null)
        {
            return false;
        }
        interval.Stop(endTime);
        projects.Update(project);
        return true;
    }
}