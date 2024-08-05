using GraphQL;
using GraphQL.Types;
using WebApplication1.GraphQL.Entity;
using WebApplication1.GraphQL.Exceptions;

namespace WebApplication1.GraphQL.Query;

public class Query: ObjectGraphType
{
    public Query()
    {
        try
        {
            Field<ListGraphType<UserType>>("users")
                .Resolve(x => new List<User>
                {
                    new User() { Name = "barak1", Age = 1 },
                    new User() { Name = "barak2", Age = 3 }
                });
        }
        catch (Exception e)
        {
            throw new GraphQLException(e.Message);
        }

        try
        {
            Field<UserType>("user")
                .Arguments(new QueryArguments(
                    new QueryArgument<IntGraphType>(){Name = "age"},
                    new QueryArgument<StringGraphType>(){Name = "name"}
                ))
                .Resolve(x =>
                {
                    var age = x.GetArgument<int>("age");
                    var name = x.GetArgument<string>("name");
                    return new User() { Name = name, Age = age };
                });
        }
        catch (Exception e)
        {
            throw new GraphQLException(e.Message);
        }
    }
}