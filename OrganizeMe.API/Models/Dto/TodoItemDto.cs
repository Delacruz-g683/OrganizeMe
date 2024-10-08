namespace OrganizeMe.API.Models.Dto;

public class TodoItemDto
{
    public Guid itemId { get; set; }
    public required string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}