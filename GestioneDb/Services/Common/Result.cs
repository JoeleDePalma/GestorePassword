namespace GestioneDb.Services.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public ErrorCode Error { get; set; }
        public T Data { get; set; }

        public static Result<T> Ok(T data) =>
            new Result<T> { Success = true, Error = ErrorCode.None, Data = data };

        public static Result<T> Fail(ErrorCode error) =>
            new Result<T> { Success = false, Error = error };
    }
}
