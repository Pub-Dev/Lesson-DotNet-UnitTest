namespace PubDev.Store.API.Responses;

public class ErrorResponse
{
    public Guid? Id { get; set; }
    public IEnumerable<ErrorMessageResponse> Messages { get; private set; }

    public ErrorResponse(Guid id, IEnumerable<ErrorMessageResponse> messages)
    {
        Id = id;
        Messages = messages;
    }

    public ErrorResponse(IEnumerable<ErrorMessageResponse> messages)
    {
        Messages = messages;
    }
}
