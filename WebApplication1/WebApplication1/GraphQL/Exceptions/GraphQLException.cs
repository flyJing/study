namespace WebApplication1.GraphQL.Exceptions;

public class GraphQLException: Exception
{
    public int Code { get; set; }
    public string Message { get; set; }

    public GraphQLException(string message,int code = 500)
    {
        Code = code;
        Message = message;
    }
}