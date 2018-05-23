using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Abstract.Entitys.Authentication;

namespace AutodictorBL.Services.AuthenticationServices
{
    public interface IAuthentificationService
    {
         bool IsAuthentification { get;  }
         User CurrentUser { get; }
         User OldUser { get; }

        bool LogIn(User user);
        void LogOut();
        void SetOldUser();
        void SetObserver();
        bool CheckRoleAcsess(IEnumerable<Role> roles);

        User CreateUser(string login, string password, Role role);
        User CreateObserver();

        Task UsersDbInitialize();
    }
}