namespace ToDoAppApi.Models;

// DTO used for input/output to avoid exposing Secret.
public sealed class TodoItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public TodoItemDto() { }

    public TodoItemDto(Todo todo)
    {
        this.Id = todo.Id;
        this.Name = todo.Name;
        this.IsComplete = todo.IsComplete;
    }
}
