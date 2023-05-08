namespace recipe_core_dotnet.common.Services.users;

public class UserServiceException : Exception
{
    public UserServiceException(string message) : base(message)
    {
    }
}