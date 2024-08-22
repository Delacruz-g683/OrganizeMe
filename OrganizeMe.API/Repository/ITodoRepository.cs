using OrganizeMe.API.Models;

namespace OrganizeMe.API;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetTodos(Guid userId);
    Task<TodoItem> GetTodoItem(Guid id);
    Task<TodoItem> AddTodoItem(TodoItem todoItem);
    Task<bool> DeleteTodoItem(Guid id);
    Task<bool> SaveAll();
}