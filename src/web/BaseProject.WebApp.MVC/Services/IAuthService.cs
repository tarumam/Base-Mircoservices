using System.Threading.Tasks;
using BaseProject.WebApp.MVC.Models;

namespace BaseProject.WebApp.MVC.Services
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);

        Task<UserResponseLogin> SignUp(UserSignUp userSignUp);
    }

}
