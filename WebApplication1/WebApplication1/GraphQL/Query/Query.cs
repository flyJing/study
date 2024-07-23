using GraphQL;
using GraphQL.Types;
using WebApplication1.GraphQL.Entity;

namespace WebApplication1.GraphQL.Query;

public class Query: ObjectGraphType
{
    public Query()
    {
        Field<ListGraphType<UserType>>("users")
            .Resolve(x => new List<User>
            {
                new User() { Name = "barak1", Age = 1 },
                new User() { Name = "barak2", Age = 3 }
            });

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
}