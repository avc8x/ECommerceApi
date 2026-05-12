namespace ECommerceApi.Application.Common.Results;

/// <summary>
/// Result pattern: wraps success/failure without throwing exceptions across handler boundaries.
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? Error { get; private set; }
    public int StatusCode { get; private set; }

    private Result(bool isSuccess, T? value, string? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T value) => new(true, value, null, 200);
    public static Result<T> Created(T value) => new(true, value, null, 201);
    public static Result<T> Failure(string error, int statusCode = 400) => new(false, default, error, statusCode);
    public static Result<T> NotFound(string error = "Resource not found") => new(false, default, error, 404);
    public static Result<T> Conflict(string error) => new(false, default, error, 409);
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public string? Error { get; private set; }
    public int StatusCode { get; private set; }

    private Result(bool isSuccess, string? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result Success() => new(true, null, 200);
    public static Result Failure(string error, int statusCode = 400) => new(false, error, statusCode);
    public static Result NotFound(string error = "Resource not found") => new(false, error, 404);
}
