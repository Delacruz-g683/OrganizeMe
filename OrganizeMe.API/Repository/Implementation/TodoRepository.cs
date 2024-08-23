using Microsoft.EntityFrameworkCore;
using OrganizeMe.API.Data;
using OrganizeMe.API.Models;

namespace OrganizeMe.API.Repository.Implementation;

public class TodoRepository(ApplicationDbContext context) : ITodoRepository
{
    public async Task<IEnumerable<TodoItem>> GetTodos(Guid userId)
    {
        return await context.TodoItems
            .Where(t => t.UserId == userId &&  t.IsDeleted == false)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<TodoItem> GetTodoItem(Guid itemId)
    {
        return await context.TodoItems.FindAsync(itemId) ?? throw new InvalidOperationException("TodoItem not found");
    }
    
    public async Task<TodoItem> UndoDeletedTdo(Guid itemId)
    {
        return await context.TodoItems.FindAsync(itemId) ?? throw new InvalidOperationException("TodoItem not found");
    }
    
    public async Task<IEnumerable<TodoItem>> GetDeletedTodoItem(Guid userId)
    {
        return await context.TodoItems
            .Where(t => t.IsDeleted == true)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<TodoItem> AddTodoItem(TodoItem todoItem)
    {
        todoItem.itemId = Guid.NewGuid();
        await context.TodoItems.AddAsync(todoItem);
        await context.SaveChangesAsync();
        return todoItem;
    }

    public async Task<bool> DeleteTodoItem(Guid itemId)
    {
        var todoItem = await context.TodoItems.FirstOrDefaultAsync(t => t.itemId == itemId);
        if (todoItem == null)
            return false;

        todoItem.IsDeleted = true;
        todoItem.DeletedDate = DateTime.Now;
        
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> SaveAll()
    {
        return await context.SaveChangesAsync() > 0;
    }
}