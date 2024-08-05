using GraphQLParser.Exceptions;
using Serilog;
using WebApplication1.GraphQL.Exceptions;

namespace WebApplication1.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (GraphQLParserException ex)
        {
            context.Response.StatusCode = 500;
            Log.Error("GraphQLParser error message : {message}", ex);
        }
        catch (GraphQLException ex)
        {
            context.Response.StatusCode = 500;
            Log.Error("GraphQL error message : {message}", ex);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            Log.Error("server error message : {message}", ex);
        }
        
    }
}