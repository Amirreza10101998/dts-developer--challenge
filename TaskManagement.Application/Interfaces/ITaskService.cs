using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTaskItems();
        Task<TaskItem> GetTaskItemById(int id);
        Task<TaskItem> CreateTaskItem(TaskItem task);
        Task<TaskItem> UpdateTaskItem(TaskItem task);
        Task<bool> UpdateTaskItemStatus(int id, TaskItemStatus newStatus);
        Task<bool> DeleteTaskItem(int id);
    }
}