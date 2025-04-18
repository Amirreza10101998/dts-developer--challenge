using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskService taskService,
            ILogger<TaskController> logger )
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new task item
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTaskItems()
        {
            try 
            {
                var taskItems = await _taskService.GetAllTaskItems();
                
                if (taskItems == null || !taskItems.Any())
                {
                    return NotFound("No task items found");
                }

                return Ok(taskItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all task items");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a task item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskItemById(int id)
        {
            try 
            {
                if(id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                var taskItem = await _taskService.GetTaskItemById(id);

                if(taskItem == null)
                {
                    return NotFound($"Task item with ID {id} not found");
                }

                return Ok(taskItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting task item with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new task item
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTaskItem([FromBody] TaskItem task)
        {
            await _taskService.CreateTaskItem(task);
            return Ok();
        }

        /// <summary>
        /// Update a task item status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskItem(int id, [FromBody] TaskItem task)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                var taskItem = await _taskService.GetTaskItemById(id);
                if (taskItem == null)
                {
                    return NotFound($"Task item with ID {id} not found");
                }

                taskItem.Status = task.Status;
                var result = await _taskService.UpdateTaskItem(taskItem);
                if (result == null)
                {
                    return BadRequest("Failed to update task item");
                }

                return Ok(taskItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating task item with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a task item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                var taskItem = await _taskService.GetTaskItemById(id);
                if (taskItem == null)
                {
                    return NotFound($"Task item with ID {id} not found");
                }

                var result = await _taskService.DeleteTaskItem(id);
                if (!result)
                {
                    return BadRequest("Failed to delete task item");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting task item with ID {id}");
                return StatusCode(500, "Internal server error");
            }   
        }
    }
}