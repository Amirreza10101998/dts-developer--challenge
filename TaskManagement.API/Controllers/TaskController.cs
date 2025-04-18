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
        /// <response code="200">Returns all task items</response>
        [HttpGet]
        public async Task<IActionResult> GetAllTaskItems()
        {
            try
            {
                var taskItems = await _taskService.GetAllTaskItems();
                return Ok(taskItems ?? new List<TaskItem>()); // Always return a list
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
        /// <response code="200">Returns the requested task item</response>
        /// <response code="400">Invalid ID format</response>
        /// <response code="404">Task item not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskItemById(int id)
        {
            try 
            {
                if (id <= 0) return BadRequest("Invalid ID format");

                var taskItem = await _taskService.GetTaskItemById(id);

                return taskItem != null ? Ok(taskItem) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting task item {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new task item
        /// </summary>
        /// <param name="task"></param>
        /// <response code="201">Returns the created task item</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        public async Task<IActionResult> CreateTaskItem([FromBody] TaskItem task)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdTask = await _taskService.CreateTaskItem(task);
                return CreatedAtAction(nameof(GetTaskItemById), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task item");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update a task item status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <response code="200">Task item updated</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Task item not found</response>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskItem(int id, [FromBody] TaskItem task)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid ID format");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _taskService.UpdateTaskItemStatus(id, task.Status);
                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating status for task {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a task item
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Task item deleted</response>
        /// <response code="400">Invalid ID format</response>
        /// <response code="404">Task item not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid ID format");
                
                var result = await _taskService.DeleteTaskItem(id);
                return result ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting task item with ID {id}");
                return StatusCode(500, "Internal server error");
            }   
        }
    }
}