namespace PubDev.Store.API.Responses;

public class ErrorMessageResponse
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }

    public static explicit operator ErrorMessageResponse(ErrorMessage errorMessage)
    {
        return new()
        {
            ErrorCode = errorMessage.ErrorCode,
            Message = errorMessage.Message,
        };
    }
}
