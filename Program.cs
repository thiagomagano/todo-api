using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TaskAPI";
    config.Title = "TaskAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/tasks", async (TaskDb db) =>
    await db.Tasks.ToListAsync()
);

app.MapGet("/tasks/done", async (TaskDb db) =>
    await db.Tasks.Where(t => t.Done).ToListAsync()
);

app.MapGet("/tasks/{id}", async (int id, TaskDb db) =>
    await db.Tasks.FindAsync(id)
    is Task task
    ? Results.Ok(task)
    : Results.NotFound()
);

app.MapPost("/tasks", async (Task task, TaskDb db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return Results.Created($"/tasks/{task.Id}", task);
});

app.Run();

