namespace MyFeed.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string username);
    }
}

