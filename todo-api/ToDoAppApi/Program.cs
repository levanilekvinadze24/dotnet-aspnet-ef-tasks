using Microsoft.EntityFrameworkCore;
using ToDoAppApi;
using ToDoAppApi.Data;
using ToDoAppApi.Models;

var builder = WebApplication.CreateBuilder(args);

// EF Core InMemory + EF developer exception filter (v8 packages)
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Simple root
app.MapGet("/", () => "Todo API is running. Try GET /todoitems");

// Group routes under /todoitems
RouteGroupBuilder todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", TodosEndpoints.GetAll);
todoItems.MapGet("/complete", TodosEndpoints.GetComplete);
todoItems.MapGet("/{id:int}", TodosEndpoints.GetById);
todoItems.MapPost("/", TodosEndpoints.Create);
todoItems.MapPut("/{id:int}", TodosEndpoints.Update);
todoItems.MapDelete("/{id:int}", TodosEndpoints.Delete);

app.Run();

namespace ToDoAppApi // S3903: keep the helper type in a named namespace
{
    internal static class TodosEndpoints
    {
        public static async Task<IResult> GetAll(TodoDb db, CancellationToken ct) =>
            TypedResults.Ok(
                await db.Todos
                        .Select(t => new TodoItemDto(t))
                        .ToArrayAsync(ct));

        public static async Task<IResult> GetComplete(TodoDb db, CancellationToken ct) =>
            TypedResults.Ok(
                await db.Todos
                        .Where(t => t.IsComplete)
                        .Select(t => new TodoItemDto(t))
                        .ToListAsync(ct));

        public static async Task<IResult> GetById(int id, TodoDb db, CancellationToken ct)
        {
            // CA1861: avoid params array allocation; use predicate instead of FindAsync([id], ct)
            var entity = await db.Todos.FirstOrDefaultAsync(t => t.Id == id, ct);
            return entity is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(new TodoItemDto(entity));
        }

        public static async Task<IResult> Create(TodoItemDto dto, TodoDb db, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["name"] = new[] { "Name is required." }
                });
            }

            var entity = new Todo
            {
                Name = dto.Name!.Trim(),
                IsComplete = dto.IsComplete
            };

            _ = db.Todos.Add(entity);           // IDE0058: explicitly discard the return value
            _ = await db.SaveChangesAsync(ct);  // IDE0058: explicitly discard

            return TypedResults.Created($"/todoitems/{entity.Id}", new TodoItemDto(entity));
        }

        public static async Task<IResult> Update(int id, TodoItemDto dto, TodoDb db, CancellationToken ct)
        {
            var entity = await db.Todos.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["name"] = new[] { "Name is required." }
                });
            }

            entity.Name = dto.Name!.Trim();
            entity.IsComplete = dto.IsComplete;

            _ = await db.SaveChangesAsync(ct);  // IDE0058: explicitly discard
            return TypedResults.NoContent();
        }

        public static async Task<IResult> Delete(int id, TodoDb db, CancellationToken ct)
        {
            var entity = await db.Todos.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            _ = db.Todos.Remove(entity);        // IDE0058: explicitly discard
            _ = await db.SaveChangesAsync(ct);  // IDE0058: explicitly discard
            return TypedResults.NoContent();
        }
    }
}
