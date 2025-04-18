using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces;  

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTaskItems()
        {
            return await _taskRepository.GetAllTaskItems();
        }

        public async Task<TaskItem> GetTaskItemById(int id)
        {
            return await _taskRepository.GetTaskItemById(id);
        }

        public async Task<TaskItem> CreateTaskItem(TaskItem task)
        {
            return await _taskRepository.CreateTaskItem(task);
        }

        public async Task<TaskItem> UpdateTaskItem(TaskItem task)
        {
            return await _taskRepository.UpdateTaskItem(task);
        }

        public async Task<bool> UpdateTaskItemStatus(int id, TaskItemStatus newStatus)
        {
            return await _taskRepository.UpdateTaskItemStatus(id, newStatus);
        }

        public async Task<bool> DeleteTaskItem(int id)
        {
            return await _taskRepository.DeleteTaskItem(id);
        }
    }
}