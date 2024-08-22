using Microsoft.EntityFrameworkCore;
using OrganizeMe.API.Data;
using OrganizeMe.API.Models;

namespace OrganizeMe.API.Repository.Implementation;

public class TodoRepository(ApplicationDbContext context) : ITodoRepository
{
    public async Task<IEnumerable<TodoItem>> GetTodos(Guid userId)
    {
        return await context.TodoItems
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<TodoItem> GetTodoItem(Guid itemId)
    {
        return await context.TodoItems.FirstOrDefaultAsync(t => t.ItemId == itemId) ?? throw new InvalidOperationException();
    }

    public async Task<TodoItem> AddTodoItem(TodoItem todoItem)
    {
        todoItem.ItemId = Guid.NewGuid();
        await context.TodoItems.AddAsync(todoItem);
        await context.SaveChangesAsync();
        return todoItem;
    }

    public async Task<bool> DeleteTodoItem(Guid itemId)
    {
        var todoItem = await context.TodoItems.FirstOrDefaultAsync(t => t.ItemId == itemId);
        if (todoItem == null)
            return false;

        context.TodoItems.Remove(todoItem);
        return await SaveAll();
    }

    public async Task<bool> SaveAll()
    {
        return await context.SaveChangesAsync() > 0;
    }
}