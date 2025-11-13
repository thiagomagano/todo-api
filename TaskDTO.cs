public class TaskDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsDone { get; set; }

    public TaskDTO() { }
    public TaskDTO(Task todoItem) =>
    (Id, Title, IsDone) = (todoItem.Id, todoItem.Title, todoItem.IsDone);
}