namespace Workers.Api.Models;

/// <summary>
/// Represents error information in API responses.
/// </summary>
public record ApiError
{
    /// <summary>
    /// Human-readable error message.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Optional error code for programmatic handling.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Field-specific validation errors (field name -> error messages).
    /// </summary>
    public IDictionary<string, string[]>? ValidationErrors { get; init; }

    /// <summary>
    /// Additional error details or metadata.
    /// </summary>
    public object? Details { get; init; }

    public ApiError(string message, string? code = null, IDictionary<string, string[]>? validationErrors = null, object? details = null)
    {
        Message = message;
        Code = code;
        ValidationErrors = validationErrors;
        Details = details;
    }

    /// <summary>
    /// Creates a simple error with just a message.
    /// </summary>
    public static ApiError Simple(string message, string? code = null)
        => new(message, code);

    /// <summary>
    /// Creates a validation error with field-specific messages.
    /// </summary>
    public static ApiError Validation(string message, IDictionary<string, string[]> validationErrors)
        => new(message, "VALIDATION_ERROR", validationErrors);

    /// <summary>
    /// Creates an error with additional details.
    /// </summary>
    public static ApiError WithDetails(string message, string code, object details)
        => new(message, code, null, details);
}
