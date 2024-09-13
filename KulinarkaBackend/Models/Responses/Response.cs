
namespace Kulinarka.Models.Responses
{
    public enum StatusCode{
        OK=200,
        Created=201,
        BadRequest=400,
        Unauthorized=401,
        NotFound=404,
        InternalServerError=500
    }
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }

        public static Response<T> Success(T data, StatusCode statusCode)
        {
            return new Response<T> { IsSuccess = true, Data = data,StatusCode=statusCode };
        }

        public static Response<T> Failure(string errorMessage, StatusCode statusCode)
        {
            return new Response<T> { IsSuccess = false, ErrorMessage = errorMessage, StatusCode=statusCode};
        }
    }
}
