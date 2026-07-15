using Microsoft.Extensions.Logging;

namespace EasyOnlineStore.Application.Services;

public partial class UserService
{
    #region Logger Messages 
    
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "User with ID {UserId} not found")]
    static partial void LogUserNotFound(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "User with email {Email} not found")]
    static partial void LogUserByEmailNotFound(ILogger logger, string email);


    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Warning,
        Message = "Login failed: User with email {Email} not found")]
    static partial void LogLoginFailedUserNotFound(ILogger logger, string email);


    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Warning,
        Message = "Login failed: Invalid password provided for user {Email} (ID: {UserId})")]
    static partial void LogLoginFailedInvalidPassword(ILogger logger, string email, Guid userId);


    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Information,
        Message = "User {Email} (ID: {UserId}) successfully logged in")]
    static partial void LogUserLoggedIn(ILogger logger, string email, Guid userId);


    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Warning,
        Message = "User with email {Email} already exists (ID: {UserId})")]
    static partial void LogUserAlreadyExists(ILogger logger, string email, Guid userId);


    [LoggerMessage(
        EventId = 7,
        Level = LogLevel.Warning,
        Message = "Registration failed: invalid role name {RoleName}")]
    static partial void LogRegistrationFailedInvalidRole(ILogger logger, string roleName);


    [LoggerMessage(
        EventId = 8,
        Level = LogLevel.Warning,
        Message = "Registration failed: {Errors} (ID: {UserId})")]
    static partial void LogRegistrationFailed(ILogger logger, string errors, Guid userId);


    [LoggerMessage(
        EventId = 9,
        Level = LogLevel.Information,
        Message = "New user {Email} (ID: {UserId}) successfully registered")]
    static partial void LogUserRegistered(ILogger logger, string? email, Guid userId);


    [LoggerMessage(
        EventId = 10,
        Level = LogLevel.Warning,
        Message = "Profile update failed: {Errors} (ID: {UserId})")]
    static partial void LogProfileUpdateFailed(ILogger logger, string errors, Guid userId);


    [LoggerMessage(
        EventId = 11,
        Level = LogLevel.Information,
        Message = "Profile successfully updated (ID: {UserId})")]
    static partial void LogProfileUpdated(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 12,
        Level = LogLevel.Warning,
        Message = "Password reset failed: {Errors} (ID: {UserId})")]
    static partial void LogPasswordResetFailed(ILogger logger, string errors, Guid userId);


    [LoggerMessage(
        EventId = 13,
        Level = LogLevel.Information,
        Message = "Password successfully changed (ID: {UserId})")]
    static partial void LogPasswordChanged(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 14,
        Level = LogLevel.Warning,
        Message = "Role update failed: invalid role name {RoleName} (ID: {UserId})")]
    static partial void LogRoleUpdateFailedInvalidRole(ILogger logger, string roleName, Guid userId);

    
    [LoggerMessage(
        EventId = 15,
        Level = LogLevel.Warning,
        Message = "Role update failed: {Errors} (ID: {UserId})")]
    static partial void LogRoleUpdateFailed(ILogger logger, string errors, Guid userId);


    [LoggerMessage(
        EventId = 16,
        Level = LogLevel.Information,
        Message = "User roles successfully updated (ID: {UserId})")]
    static partial void LogUserRolesUpdated(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 17,
        Level = LogLevel.Warning,
        Message = "Locking user failed: {Errors} (ID: {UserId})")]
    static partial void LogLockUserFailed(ILogger logger, string errors, Guid userId);


    [LoggerMessage(
        EventId = 18,
        Level = LogLevel.Information,
        Message = "User successfully locked (ID: {UserId})")]
    static partial void LogUserLocked(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 19,
        Level = LogLevel.Warning,
        Message = "Unlocking user failed: {Errors} (ID: {UserId})")]
    static partial void LogUnlockUserFailed(ILogger logger, string errors, Guid userId);


    [LoggerMessage(
        EventId = 20,
        Level = LogLevel.Information,
        Message = "User successfully unlocked (ID: {UserId})")]
    static partial void LogUserUnlocked(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 21,
        Level = LogLevel.Warning,
        Message = "User deletion failed: {Errors} (ID: {UserId})")]
    static partial void LogUserDeletionFailed(ILogger logger, string errors, Guid userId);
    
    
    [LoggerMessage(
        EventId = 22,
        Level = LogLevel.Information,
        Message = "User successfully deleted (ID: {UserId})")]
    static partial void LogUserDeleted(ILogger logger, Guid userId);

    #endregion
}