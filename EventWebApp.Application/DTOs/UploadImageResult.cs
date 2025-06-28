namespace EventWebApp.Application.DTOs
{
  public class UploadImageResult
  {
    public bool IsSuccess { get; set; }
    public string? ImageUrl { get; set; }
    public string? ErrorMessage { get; set; }

    public static UploadImageResult Success(string imageUrl)
    {
      return new UploadImageResult
      {
        IsSuccess = true,
        ImageUrl = imageUrl
      };
    }

    public static UploadImageResult Failure(string errorMessage)
    {
      return new UploadImageResult
      {
        IsSuccess = false,
        ErrorMessage = errorMessage
      };
    }

    public (int statusCode, object data) ToHttpResponse()
    {
      if (IsSuccess)
      {
        return (200, new { imageUrl = ImageUrl });
      }

      return (400, ErrorMessage!);
    }
  }
}