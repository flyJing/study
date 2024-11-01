using GraphQL.Types;


namespace WebApplication.GraphQL.Entity;

public class User
{
    public string Name { get; set; }
    
    public int Age { get; set; }
    
}

public class UserType : ObjectGraphType<User>
{
    public UserType()
    {
        Field(x => x.Name).Description("The name of the user.");
        Field(x => x.Age).Description("The age of the user.");
    }
}