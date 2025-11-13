using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();


app.MapGet("/", () =>
{
    return "Hello";
})
.WithName("hello");

app.MapGet("/tasks", async (TaskDb db) =>
    await db.Tasks.ToListAsync()
);

app.MapPost("/tasks", async (Task task, TaskDb db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPost("/tasks", () =>
{
    return "TASK CRIADA";
})
.WithName("PostTasks");

app.Run();

record TasksDTO(int Id, string Tittle, bool Done) { }

