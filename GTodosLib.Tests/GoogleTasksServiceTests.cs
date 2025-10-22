using GTodosLib;

namespace GTodosLib.Tests;

public class GoogleTasksServiceTests
{
    [Fact]
    public void Constructor_InitializesWithApplicationName()
    {
        // Arrange
        var applicationName = "Test Application";

        // Act
        var service = new GoogleTasksService(applicationName);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetTaskListsAsync_ThrowsInvalidOperationException_WhenNotAuthenticated()
    {
        // Arrange
        var service = new GoogleTasksService("Test Application");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetTaskListsAsync());
        
        Assert.Contains("サービスが初期化されていません", exception.Message);
    }

    [Fact]
    public async Task GetTasksAsync_ThrowsInvalidOperationException_WhenNotAuthenticated()
    {
        // Arrange
        var service = new GoogleTasksService("Test Application");
        var taskListId = "test-task-list-id";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetTasksAsync(taskListId));
        
        Assert.Contains("サービスが初期化されていません", exception.Message);
    }

    [Fact]
    public async Task CreateTaskAsync_ThrowsInvalidOperationException_WhenNotAuthenticated()
    {
        // Arrange
        var service = new GoogleTasksService("Test Application");
        var taskListId = "test-task-list-id";
        var title = "Test Task";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateTaskAsync(taskListId, title));
        
        Assert.Contains("サービスが初期化されていません", exception.Message);
    }

    [Fact]
    public async Task CreateTaskAsync_WithNotes_ThrowsInvalidOperationException_WhenNotAuthenticated()
    {
        // Arrange
        var service = new GoogleTasksService("Test Application");
        var taskListId = "test-task-list-id";
        var title = "Test Task";
        var notes = "Test notes";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateTaskAsync(taskListId, title, notes));
        
        Assert.Contains("サービスが初期化されていません", exception.Message);
    }

    [Fact]
    public async Task CreateTaskAsync_WithDueDate_ThrowsInvalidOperationException_WhenNotAuthenticated()
    {
        // Arrange
        var service = new GoogleTasksService("Test Application");
        var taskListId = "test-task-list-id";
        var title = "Test Task";
        var notes = "Test notes";
        var due = "2024-12-31T23:59:59.000Z";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateTaskAsync(taskListId, title, notes, due));
        
        Assert.Contains("サービスが初期化されていません", exception.Message);
    }

    [Fact]
    public async Task GetTaskListsAsync_WithCustomMaxResults_ThrowsInvalidOperationException_WhenNotAuthenticated()
    {
        // Arrange
        var service = new GoogleTasksService("Test Application");
        var maxResults = 5;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetTaskListsAsync(maxResults));
        
        Assert.Contains("サービスが初期化されていません", exception.Message);
    }

    [Fact]
    public async Task GetTasksAsync_WithCustomMaxResults_ThrowsInvalidOperationException_WhenNotAuthenticated()
    {
        // Arrange
        var service = new GoogleTasksService("Test Application");
        var taskListId = "test-task-list-id";
        var maxResults = 50;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.GetTasksAsync(taskListId, maxResults));
        
        Assert.Contains("サービスが初期化されていません", exception.Message);
    }
}
