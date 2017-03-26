
using System.Linq;
using SportsStore.Domain.Abstract;

namespace SportsStore.Domain.Concrete
{
    public class Authentication : IAuthentication
    {
        readonly IUserRepository _userRepository;

        public Authentication(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool CheckLogin(string email, string password)
        {
            var isCorrect = _userRepository.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return isCorrect != null;
        }

        public bool Logout()
        {
            return true;
        }
    }
}
