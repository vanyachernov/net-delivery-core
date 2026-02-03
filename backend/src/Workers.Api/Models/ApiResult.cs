namespace Workers.Api.Models;

/// <summary>
/// A unified envelope for all API responses.
/// </summary>
public record ApiResult<T>
{
    /// <summary>
    /// Indicates whether the request was successful.
    /// </summary>
    public bool Status { get; init; }

    /// <summary>
    /// The actual data returned by the API.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Detailed information about errors, if any.
    /// </summary>
    public object? Errors { get; init; }

    /// <summary>
    /// Timestamp when the response was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; init; }

    public ApiResult(bool status, T? data, object? errors)
    {
        Status = status;
        Data = data;
        Errors = errors;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a successful result with data.
    /// </summary>
    public static ApiResult<T> Success(T data) 
        => new(true, data, null);
    
    /// <summary>
    /// Creates a failed result with error details.
    /// </summary>
    public static ApiResult<T> Failure(object? errors) 
        => new(false, default, errors);
}

/// <summary>
/// Helper for non-generic or object-based responses.
/// </summary>
public static class ApiResult
{
    /// <summary>
    /// Creates a successful result with generic data.
    /// </summary>
    public static ApiResult<T> Success<T>(T data) 
        => ApiResult<T>.Success(data);

    /// <summary>
    /// Creates a successful result with an empty object.
    /// </summary>
    public static ApiResult<object> Success() 
        => new(true, new { }, null);

    /// <summary>
    /// Creates a failed result with error details.
    /// </summary>
    public static ApiResult<object> Failure(object? errors) 
        => new(false, null, errors);
}
