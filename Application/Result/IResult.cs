namespace ConstructorAdminAPI.Application.Result
{
    public interface IResult
    {
        bool IsSuccessfull { get; }
        IReadOnlyList<Error> GetErrors();
    }
}
