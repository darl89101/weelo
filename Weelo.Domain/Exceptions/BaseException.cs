namespace Weelo.Domain.Exceptions;

public abstract class BaseException : Exception
{
    public int CodeError { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public BaseException(int codeError, string message, IEnumerable<string>? errors)
        : base(message)
    {
        CodeError = codeError;
        Errors = errors;
    }
}
