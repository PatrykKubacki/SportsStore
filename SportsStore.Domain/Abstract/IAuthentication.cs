
namespace SportsStore.Domain.Abstract
{
    public interface IAuthentication
    {
        bool CheckLogin(string email, string password);
        bool Logout();
    }
}
