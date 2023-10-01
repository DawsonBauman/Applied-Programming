using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class HomeworkAssignment
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Subject { get; set; }
    public AssignmentTimer Timer { get; } = new AssignmentTimer();

    public HomeworkAssignment(string title, string description, DateTime dueDate, string subject)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Subject = subject;
    }

    public string GetSummary()
    {
        return $"{Title}\nDue Date: {DueDate.ToString("MM/dd/yyyy")}\nDescription: {Description}\nSubject: {Subject}\n";
    }
}

public class AssignmentTimer
{
    private Stopwatch stopwatch = new Stopwatch();

    public void Start()
    {
        stopwatch.Start();
    }

    public void Stop()
    {
        stopwatch.Stop();
    }

    public void Reset()
    {
        stopwatch.Reset();
    }

    public TimeSpan ElapsedTime => stopwatch.Elapsed;
}

class Tracker
{
    static void Main(string[] args)
    {
        var assignments = new List<HomeworkAssignment>();

        // Prompt the user to create homework assignments
        while (true)
        {
            Console.WriteLine("Options: create, remove, view, quit");
            string option = Console.ReadLine().ToLower();

            switch (option)
            {
                case "create":
                    CreateAssignment(assignments);
                    break;
                case "remove":
                    RemoveAssignment(assignments);
                    break;
                case "view":
                    ViewAssignments(assignments);
                    break;
                case "quit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void CreateAssignment(List<HomeworkAssignment> assignments)
    {
        Console.WriteLine("Enter the title of the homework assignment:");
        string title = Console.ReadLine() ?? "";

        Console.WriteLine("Enter the description of the homework assignment:");
        string description = Console.ReadLine() ?? "";

        Console.WriteLine("Enter the due date of the homework assignment (MM/dd/yyyy):");
        string dueDateString = Console.ReadLine() ?? "";

        DateTime dueDate;

        if (string.IsNullOrEmpty(dueDateString) || !DateTime.TryParse(dueDateString, out dueDate))
        {
            Console.WriteLine("Invalid date format. Using the current date as the due date.");
            dueDate = DateTime.Now;
        }

        Console.WriteLine("Enter the subject of the homework assignment:");
        string subject = Console.ReadLine() ?? "";

        var assignment = new HomeworkAssignment(title, description, dueDate, subject);
        assignments.Add(assignment);

        Console.WriteLine("Assignment created.");
    }

    static void RemoveAssignment(List<HomeworkAssignment> assignments)
    {
        Console.WriteLine("Enter the title of the assignment to remove:");
        string titleToRemove = Console.ReadLine();

        var assignmentToRemove = assignments.FirstOrDefault(a => a.Title.Equals(titleToRemove, StringComparison.OrdinalIgnoreCase));

        if (assignmentToRemove != null)
        {
            assignments.Remove(assignmentToRemove);
            Console.WriteLine($"Assignment '{titleToRemove}' removed.");
        }
        else
        {
            Console.WriteLine($"Assignment '{titleToRemove}' not found.");
        }
    }

    static void ViewAssignments(List<HomeworkAssignment> assignments)
{
    // Display assignment summaries and handle timers
    Console.WriteLine("\nHomework Assignments:");

    foreach (var assignment in assignments)
    {
        // Check if assignment is not null
        if (assignment != null)
        {
            Console.WriteLine(assignment.GetSummary());

            // Handle timers
            while (true)
            {
                Console.WriteLine("Commands: start, stop, next (to move to the next assignment), quit");
                string command = Console.ReadLine().ToLower();

                if (command == "start")
                {
                    Console.WriteLine("Timer started.");
                    assignment.Timer.Start();
                }
                else if (command == "stop")
                {
                    assignment.Timer.Stop();
                    Console.WriteLine("Timer stopped.");
                }
                else if (command == "next")
                {
                    break;
                }
                else if (command == "quit")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                }
            }

            // Use null-conditional operator to access ElapsedTime property safely
            TimeSpan? timeSpentOnAssignment = assignment.Timer.ElapsedTime;
            if (timeSpentOnAssignment.HasValue)
            {
                Console.WriteLine($"Time Spent on Assignment '{assignment.Title}': {timeSpentOnAssignment}\n");
            }
            else
            {
                Console.WriteLine($"Timer information not available for assignment '{assignment.Title}'.\n");
            }
        }
        else
        {
            Console.WriteLine("Assignment is null.");
        }
    }

    // Display time summaries
    DisplayTimeSummaries(assignments);
}

    static void DisplayTimeSummaries(List<HomeworkAssignment> assignments)
    {
        var totalTimeSpentOnSubjects = new Dictionary<string, TimeSpan>();
        var totalTimeSpent = TimeSpan.Zero;

        foreach (var assignment in assignments)
        {
            var subject = assignment.Subject;

            if (!totalTimeSpentOnSubjects.ContainsKey(subject))
            {
                totalTimeSpentOnSubjects[subject] = TimeSpan.Zero;
            }

            totalTimeSpentOnSubjects[subject] += assignment.Timer.ElapsedTime;
            totalTimeSpent += assignment.Timer.ElapsedTime;
        }

        Console.WriteLine("\nTime Summaries:");
        Console.WriteLine($"Total Time Spent on All Assignments: {totalTimeSpent}");

        foreach (var subject in totalTimeSpentOnSubjects.Keys)
        {
            Console.WriteLine($"Time Spent on {subject}: {totalTimeSpentOnSubjects[subject]}");
        }
    }
}
