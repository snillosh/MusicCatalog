using MusicCatalog.Application.Common.Errors;

namespace MusicCatalog.Application.Common.Results;

public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error? Error { get; }

    public static Result Success() => new(true, null);
    public static Result Fail(string code, string message) => new(false, new Error(code, message));
}

public sealed class Result<T> : Result
{
    private Result(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(true, value, null);
    public new static Result<T> Fail(string code, string message) => new(false, default, new Error(code, message));
}
