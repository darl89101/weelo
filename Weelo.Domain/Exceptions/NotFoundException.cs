namespace Weelo.Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message, IEnumerable<string>? errors = null)
        : base(404, message, errors)
    {
    }
}
