using Microsoft.EntityFrameworkCore;
using ToDoAppApi.Models;

namespace ToDoAppApi.Data;

public sealed class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

    public DbSet<Todo> Todos => this.Set<Todo>();
}
