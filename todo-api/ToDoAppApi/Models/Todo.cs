using System.Text.Json.Serialization;

namespace ToDoAppApi.Models;

public sealed class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    // Never serialize this, even if someone returns the entity by mistake
    [JsonIgnore]
    public string? Secret { get; set; }
}
