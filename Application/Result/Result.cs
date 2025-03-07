
namespace Constructor_API.Application.Result
{
    public class Result : IResult
    {
        public bool IsSuccessfull => _errors.Count < 1;
        private readonly List<Error> _errors = [];

        protected void AddError(Error error)
        {
            ArgumentNullException.ThrowIfNull(error);
            _errors.Add(error);
        }

        public IReadOnlyList<Error> GetErrors() => _errors;

        private static readonly Result _success = new Result();
        public static Result Success() => _success;

        public static Result Error(Error error)
        {
            var result = new Result();
            result.AddError(error);
            return result;
        }
    }
}
