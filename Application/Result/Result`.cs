namespace Constructor_API.Application.Result
{
    public class Result<TValue> : Result, IResult<TValue>
    {
        public TValue Value { get; set; }

        private Result(TValue value)
        {
            Value = value;
        }
        private Result(Error error)
        {
            AddError(error);
        }

        public static Result<TValue> Success(TValue value)
        {
            return new Result<TValue>(value);
        }

        public new static Result<TValue> Error(Error error)
        {
            return new Result<TValue>(error);
        }
    }
}
