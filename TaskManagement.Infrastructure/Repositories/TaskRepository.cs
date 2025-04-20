using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTaskItems()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem> GetTaskItemById(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task<TaskItem> CreateTaskItem(TaskItem task)
        {
            task.CreatedAt = DateTime.UtcNow;
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }
        
        public async Task<bool> UpdateTaskItem(TaskItem task)
        {
            _context.TaskItems.Update(task); // Mark entity as modified
            await _context.SaveChangesAsync();
            return true; // Or handle exceptions
        }

        public async Task<bool> UpdateTaskItemStatus(int id, TaskItemStatus newStatus)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            task.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskItem(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}