namespace OrganizeMe.API.Models;

public class ToDo
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    //public bool IsDone { get; set; }
    public DateTime? CompletedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}