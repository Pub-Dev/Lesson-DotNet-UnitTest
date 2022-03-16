using PubDev.Store.API.Enums;
using System.Collections.ObjectModel;

namespace PubDev.Store.API;

public class NotificationContext
{
    private readonly IList<ErrorMessage> _errors = new List<ErrorMessage>();
    public IReadOnlyCollection<ErrorMessage> ErrorMessages { get => new ReadOnlyCollection<ErrorMessage>(_errors); }
    public bool IsValid { get => _errors.Count == 0; }

    public void AddNotification(string errorCode, string message, ErrorType errorType)
    {
        _errors.Add(new()
        {
            ErrorType = errorType,
            ErrorCode = errorCode,
            Message = message
        });
    }

    public void AddNotFound(string errorCode, string message)
    {
        AddNotification(errorCode, message, ErrorType.NotFound);
    }

    public void AddValidationError(string errorCode, string message)
    {
        AddNotification(errorCode, message, ErrorType.Validation);
    }
}
