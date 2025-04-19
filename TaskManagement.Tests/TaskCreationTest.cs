using Moq;
using TaskManagement.Application.Interfaces;
using TaskManagement.API.Controllers;
using Microsoft.Extensions.Logging;
using Castle.Core.Logging;
using TaskManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace TaskManagement.Tests;

public class TaskControllerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly Mock<ILogger<TaskController>> _mockLogger;
    private readonly TaskController _controller;

    public TaskControllerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _mockLogger = new Mock<ILogger<TaskController>>();
        _controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllTaskItems_ReturnsOkResult_WithListOfTasks()
    {
        // Arrange
        var mockTasks = new List<TaskItem>
        { 
            new TaskItem { Id = 1, Title = "Task 1" },
            new TaskItem { Id = 2, Title = "Task 2"}
        };
        
        _mockTaskService.Setup(service => service.GetAllTaskItems())
            .ReturnsAsync(mockTasks);
        
        // Act
        var result = await _controller.GetAllTaskItems();
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(mockTasks);
    }
    
    [Fact]
    public async Task GetAllTaskItems_ReturnsEmptyList_WhenNoTasksExist()
    {
        // Arrange
        _mockTaskService.Setup(service => service.GetAllTaskItems())
            .ReturnsAsync(new List<TaskItem>());
        
        // Act
        var result = await _controller.GetAllTaskItems();
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        (okResult.Value as IEnumerable<TaskItem>).Should().BeEmpty();
    }

    [Fact]
    public async Task GetTaskItemById_ReturnsOkResult_WhenTaskExists()
    {
        // Arrange
        var taskId = 1;
        var mockTask = new TaskItem { Id = taskId, Title = "Task 1" };
        
        _mockTaskService.Setup(service => service.GetTaskItemById(taskId))
            .ReturnsAsync(mockTask);
        
        // Act
        var result = await _controller.GetTaskItemById(taskId);
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(mockTask);
    }

    [Fact]
    public async Task GetTaskItemById_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var testId = 1;
        _mockTaskService.Setup(service => service.GetTaskItemById(testId))
        .ReturnsAsync((TaskItem)null);
        
        // Act
        var result = await _controller.GetTaskItemById(testId);
        
        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Fact]
    public async Task GetTaskItemById_ReturnsBadRequest_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = 0;
        
        // Act
        var result = await _controller.GetTaskItemById(invalidId);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().Be("Invalid ID format");
    }

    [Fact]
    public async Task CreateTaskItem_ReturnsCreatedResult_WithTask()
    {
        // Arrange
        var newTask = new TaskItem { Title = "New Task", Description = "Description" };
        var createdTask = new TaskItem { Id = 1, Title = "New Task", Description = "Description" };
        
        _mockTaskService.Setup(service => service.CreateTaskItem(newTask))
            .ReturnsAsync(createdTask);
            
        // Act
        var result = await _controller.CreateTaskItem(newTask);
        
        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Value.Should().BeEquivalentTo(createdTask);
        createdResult.ActionName.Should().Be(nameof(TaskController.GetTaskItemById));
    }
    
    [Fact]
    public async Task CreateTaskItem_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var invalidTask = new TaskItem { Title = "" }; // Invalid because title is empty
        _controller.ModelState.AddModelError("Title", "Title is required");
        
        // Act
        var result = await _controller.CreateTaskItem(invalidTask);
        
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
public async Task UpdateTaskItem_ReturnsOkResult_WhenUpdateIsSuccessful()
{
    // Arrange
    var testId = 1;
    var updatedTask = new TaskItem { Id = testId, Title = "Updated Task" };
    
    _mockTaskService.Setup(service => service.UpdateTaskItem(testId, updatedTask))
        .ReturnsAsync(true);

    // Act
    var result = await _controller.UpdateTaskItem(testId, updatedTask);

    // Assert
    result.Should().BeOfType<OkResult>();
}

[Fact]
public async Task UpdateTaskItem_ReturnsNotFound_WhenTaskDoesNotExist()
{
    // Arrange
    var testId = 1;
    var updatedTask = new TaskItem { Id = testId, Title = "Updated Task" };
    
    _mockTaskService.Setup(service => service.UpdateTaskItem(testId, updatedTask))
        .ReturnsAsync(false);

    // Act
    var result = await _controller.UpdateTaskItem(testId, updatedTask);

    // Assert
    result.Should().BeOfType<NotFoundResult>();
}

[Fact]
public async Task UpdateTaskItem_ReturnsBadRequest_WhenIdIsInvalid()
{
    // Arrange
    var invalidId = 0;
    var updatedTask = new TaskItem { Id = 1, Title = "Updated Task" };

    // Act
    var result = await _controller.UpdateTaskItem(invalidId, updatedTask);

    // Assert
    result.Should().BeOfType<BadRequestObjectResult>();
}

[Fact]
public async Task DeleteTaskItem_ReturnsNoContent_WhenDeleteIsSuccessful()
{
    // Arrange
    var testId = 1;
    _mockTaskService.Setup(service => service.DeleteTaskItem(testId))
        .ReturnsAsync(true);

    // Act
    var result = await _controller.DeleteTaskItem(testId);

    // Assert
    result.Should().BeOfType<NoContentResult>();
}

[Fact]
public async Task DeleteTaskItem_ReturnsNotFound_WhenTaskDoesNotExist()
{
    // Arrange
    var testId = 1;
    _mockTaskService.Setup(service => service.DeleteTaskItem(testId))
        .ReturnsAsync(false);

    // Act
    var result = await _controller.DeleteTaskItem(testId);

    // Assert
    result.Should().BeOfType<NotFoundResult>();
}

[Fact]
public async Task DeleteTaskItem_ReturnsBadRequest_WhenIdIsInvalid()
{
    // Arrange
    var invalidId = 0;

    // Act
    var result = await _controller.DeleteTaskItem(invalidId);

    // Assert
    result.Should().BeOfType<BadRequestObjectResult>();
}

[Fact]
public async Task GetAllTaskItems_ReturnsStatusCode500_WhenExceptionOccurs()
{
    // Arrange
    _mockTaskService.Setup(service => service.GetAllTaskItems())
        .ThrowsAsync(new Exception("Test exception"));

    // Act
    var result = await _controller.GetAllTaskItems();

    // Assert
    result.Should().BeOfType<ObjectResult>();
    var objectResult = result as ObjectResult;
    objectResult.StatusCode.Should().Be(500);
}





}