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

var apiTasks = app.MapGroup("/api/tasks");

apiTasks.MapGet("/", GetAllTasks);
apiTasks.MapGet("/done", GetDoneTasks);
apiTasks.MapGet("/{id}", GetTask);
apiTasks.MapPost("/", CreateTask);
apiTasks.MapPut("/{id}", UpdateTask);
apiTasks.MapDelete("/{id}", DeleteTask);

app.Run();

static async Task<IResult> GetAllTasks(TaskDb db)
{
    return TypedResults.Ok(await db.Tasks.ToArrayAsync());
}

static async Task<IResult> GetDoneTasks(TaskDb db)
{
    return TypedResults.Ok(await db.Tasks.Where(t => t.Done).ToArrayAsync());
}

static async Task<IResult> GetTask(int id, TaskDb db)
{
    return await db.Tasks.FindAsync(id)
        is Task task
        ? TypedResults.Ok(task)
        : TypedResults.NotFound();
}

static async Task<IResult> CreateTask(Task task, TaskDb db)
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/tasks/{task.Id}", task);
}

static async Task<IResult> UpdateTask(int id, Task inputTask, TaskDb db)
{
    var task = await db.Tasks.FindAsync(id);

    if (task is null) return Results.NotFound();

    task.Title = inputTask.Title;
    task.Done = inputTask.Done;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTask(int id, TaskDb db)
{
    if (await db.Tasks.FindAsync(id) is Task task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}