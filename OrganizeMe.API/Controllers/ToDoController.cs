using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizeMe.API.Data;
using OrganizeMe.API.Models;

namespace OrganizeMe.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ToDoController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodos()
    {
        var todos = await _applicationDbContext.ToDo.ToListAsync();

        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> AddTodo(ToDo toDo)
    {
        toDo.Id = Guid.NewGuid();

        _applicationDbContext.ToDo.Add(toDo);
        await _applicationDbContext.SaveChangesAsync();
        
        return Ok(toDo); 
    }
}