namespace WebApplication.GraphQL.Exceptions;

public class GraphQlException: Exception
{
    public int Code { get; set; }
    public string Message { get; set; }

    public GraphQlException(string message,int code = 500)
    {
        Code = code;
        Message = message;
    }
}