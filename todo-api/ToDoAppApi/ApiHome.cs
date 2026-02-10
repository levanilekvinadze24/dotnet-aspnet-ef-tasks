namespace ToDoAppApi;

internal static class ApiHome
{
    internal static readonly object Payload = new
    {
        name = "Todo Minimal API",
        version = "v1",
        endpoints = new[]
        {
            new { method = "GET",    path = "/todoitems",          description = "Get all items" },
            new { method = "GET",    path = "/todoitems/complete", description = "Get completed items" },
            new { method = "GET",    path = "/todoitems/{id}",     description = "Get item by id" },
            new { method = "POST",   path = "/todoitems",          description = "Create item" },
            new { method = "PUT",    path = "/todoitems/{id}",     description = "Update item" },
            new { method = "DELETE", path = "/todoitems/{id}",     description = "Delete item" },
        }
    };
}
