using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizeMe.API.Models;
using OrganizeMe.API.Models.Dto;
namespace OrganizeMe.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;
    private readonly IMapper _mapper;

    public TodoController(ITodoRepository todoRepository, IMapper mapper)
    {
        _todoRepository = todoRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var todos = await _todoRepository.GetTodos(userId);
        var todosToReturn = _mapper.Map<IEnumerable<TodoItemDto>>(todos);

        return Ok(todosToReturn);
    }

    [HttpGet("{itemId}", Name = "GetTodoItem")]
    public async Task<IActionResult> GetTodoItem(Guid itemId)
    {
        var todo = await _todoRepository.GetTodoItem(itemId);
        if (todo == null)
            return NotFound();

        var todoToReturn = _mapper.Map<TodoItemDto>(todo);
        return Ok(todoToReturn);
    }
    
    [HttpGet]
    [Route("get-deleted-todos")]
    public async Task<IActionResult> GetDeletedTodoItem()
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var todos = await _todoRepository.GetDeletedTodoItem(userId);
        var todosToReturn = _mapper.Map<IEnumerable<TodoItemDto>>(todos);

        return Ok(todosToReturn);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoItem(TodoItemDto todoItemDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        var userId = new Guid(userIdClaim.Value);
        var todoItem = _mapper.Map<TodoItem>(todoItemDto);
        todoItem.UserId = userId;

        todoItem.CreatedDate = DateTime.Now;
        
        var createdTodoItem = await _todoRepository.AddTodoItem(todoItem);
        var todoToReturn = _mapper.Map<TodoItemDto>(createdTodoItem);

        return CreatedAtRoute("GetTodoItem", new { itemId = createdTodoItem.itemId }, todoToReturn);
    }


    [HttpPut("{itemId}")]
    public async Task<IActionResult> UpdateTodoItem(Guid itemId, TodoItemDto todoItemDto)
    {
        var todoItemFromRepo = await _todoRepository.GetTodoItem(itemId);
        if (todoItemFromRepo == null)
            return NotFound();

        _mapper.Map(todoItemDto, todoItemFromRepo);
        
        todoItemFromRepo.CompletedDate = DateTime.Now;

        if (await _todoRepository.SaveAll())
            return NoContent();

        throw new Exception($"Updating item {itemId} failed on save");
    }
    
    [HttpPut]
    [Route("undo-deleted-todo/{itemId}")]
    public async Task<IActionResult> UndoDeletedTodo(Guid itemId, TodoItemDto todoItemDto)
    {
        var todoItemFromRepo = await _todoRepository.UndoDeletedTdo(itemId);
        if (todoItemFromRepo == null)
            return NotFound();

        _mapper.Map(todoItemDto, todoItemFromRepo);

        todoItemFromRepo.DeletedDate = null;
        todoItemFromRepo.IsDeleted = false;
        
        if (await _todoRepository.SaveAll())
            return NoContent();

        throw new Exception($"Updating item {itemId} failed on save");
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> DeleteTodoItem(Guid itemId)
    {
        var todoItemFromRepo = await _todoRepository.GetTodoItem(itemId);
        if (todoItemFromRepo == null)
            return NotFound();

        if (await _todoRepository.DeleteTodoItem(itemId))
            return NoContent();

        throw new Exception($"Deleting item {itemId} failed on save");
    }
}