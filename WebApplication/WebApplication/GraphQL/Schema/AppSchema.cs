namespace WebApplication.GraphQL.Schema;

public class AppSchema: global::GraphQL.Types.Schema
{
    public AppSchema(IServiceProvider provider): base(provider)
    {
        Query = provider.GetRequiredService<Query.Query>();
    }
}