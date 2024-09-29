using Kulinarka.Models.Responses;

namespace Kulinarka.Services
{
    public static class UploadFilesService
    {
        public static async Task<Response<byte[]>> UploadFileAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return Response<byte[]>.Failure("File is required", StatusCode.BadRequest);
            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream);
                return Response<byte[]>.Success(stream.ToArray(),StatusCode.OK);
            }
        }
    }
}
