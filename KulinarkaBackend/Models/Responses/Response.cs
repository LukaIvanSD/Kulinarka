namespace Kulinarka.Models.Responses
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }

        public static Response<T> Success(T data)
        {
            return new Response<T> { IsSuccess = true, Data = data };
        }

        public static Response<T> Failure(string errorMessage)
        {
            return new Response<T> { IsSuccess = false, ErrorMessage = errorMessage };
        }
    }
}
