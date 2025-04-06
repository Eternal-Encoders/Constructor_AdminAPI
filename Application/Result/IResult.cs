namespace Constructor_API.Application.Result
{
    public interface IResult
    {
        bool IsSuccessfull { get; }
        IReadOnlyList<Error> GetErrors();
    }
}
