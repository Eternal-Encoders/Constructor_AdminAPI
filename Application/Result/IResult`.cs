namespace Constructor_API.Application.Result
{
    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}
