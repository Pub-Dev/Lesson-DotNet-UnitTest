using PubDev.Store.API.Enums;

namespace PubDev.Store.API;

public struct ErrorMessage
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }
    public ErrorType ErrorType { get; set; }

    public ErrorMessage(string errorCode, string message, ErrorType errorType)
    {
        ErrorCode = errorCode;
        Message = message;
        ErrorType = errorType;
    }
}
