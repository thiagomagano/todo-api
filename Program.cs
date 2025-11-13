using Microsoft.JSInterop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/hello", () =>
{
    var message = new WelcomeMessage("Hello Watch");
    return message;
})
.WithName("hello");

app.MapGet("/tasks", () =>
{
    var t1 = new TasksDTO(1, "Estudar DotNet 10", false);
    var t2 = new TasksDTO(2, "Estudar Angular 21", false);
    var t3 = new TasksDTO(3, "Estudar Linux Bash", false);
    var t4 = new TasksDTO(4, "Estudar Payment wayre", false);
    var t5 = new TasksDTO(5, "Estudar Inglês técnico", true);

    var tasks = new[] { t1, t2, t3, t4, t5 };

    return tasks;
})
.WithName("GetTasks");

app.MapPost("/tasks", () =>
{
    return "TASK CRIADA";
})
.WithName("PostTasks");

app.Run();

record WelcomeMessage(string Message) { }
record TasksDTO(int Id, string Tittle, bool Done) { }

