using System.IO;
using System.Text;
using PricklyApp.Models;

namespace PricklyApp.Utils;

public static class CsvManager
{
    public static async void Export(List<Project> projects, string filePath)
    {
        await using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
        await writer.WriteLineAsync("Project,Task,Created,Start,End,Duration");
        foreach (var project in projects)
        {
            if (project.Tasks.Count == 0)
            {
                await writer.WriteLineAsync($"{project.Name},,,,");
                continue;
            }
            foreach (var task in project.Tasks)
            {
                if (task.WorkIntervals.Count == 0)
                {
                    await writer.WriteLineAsync($"{project.Name},{task.Name},,,");
                    continue;
                }
                foreach (var interval in task.WorkIntervals)
                {
                    await writer.WriteLineAsync($"{project.Name},{task.Name},{interval.CreatedAt},{interval.Start},{interval.End},{interval.Duration}");
                }
            }
        }
    }
}