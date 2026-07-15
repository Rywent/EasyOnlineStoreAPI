using Microsoft.Extensions.Logging;

namespace EasyOnlineStore.Application.Services;

public partial class CategoryService
{

    [LoggerMessage(
        EventId = 401,
        Level = LogLevel.Warning,
        Message = "Category with ID {CategoryId} was not found")]
    static partial void LogCategoryNotFound(ILogger logger, Guid categoryId);


    [LoggerMessage(
        EventId = 402,
        Level = LogLevel.Warning,
        Message = "Validation failed: Category name cannot be empty. Provided name: '{CategoryName}'")]
    static partial void LogInvalidCategoryName(ILogger logger, string categoryName);


    [LoggerMessage(
        EventId = 403,
        Level = LogLevel.Information,
        Message = "Category {CategoryId} was successfully created. Name: '{CategoryName}', Code: '{CategoryCode}'")]
    static partial void LogCategoryCreated(ILogger logger, Guid categoryId, string categoryName, string categoryCode);


    [LoggerMessage(
        EventId = 404,
        Level = LogLevel.Information,
        Message = "Category {CategoryId} was successfully updated. New Name: '{CategoryName}', New Code: '{CategoryCode}'")]
    static partial void LogCategoryUpdated(ILogger logger, Guid categoryId, string categoryName, string categoryCode);


    [LoggerMessage(
        EventId = 405,
        Level = LogLevel.Information,
        Message = "Category with ID {CategoryId} was successfully deleted from the system")]
    static partial void LogCategoryDeleted(ILogger logger, Guid categoryId);
}