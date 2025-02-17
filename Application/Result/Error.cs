namespace ConstructorAdminAPI.Application.Result
{
    public class Error
    {
        public readonly string _message;
        public readonly int _code;
        public Error(string message, int code)
        {
            _code = code;
            _message = message;
        }
    }
}
