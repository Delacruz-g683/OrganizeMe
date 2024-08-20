using Microsoft.EntityFrameworkCore;
using OrganizeMe.API.Models;

namespace OrganizeMe.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<ToDo> ToDo { get; set; }
}