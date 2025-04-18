using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class TaskItem 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
}
