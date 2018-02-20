using System.Collections.Generic;
using DAL.Abstract.Entitys.Authentication;

namespace AutodictorBL.Services
{
    public interface IAuthentificationService
    {
         bool IsAuthentication { get;  }
         User CurrentUser { get; }
         User OldUser { get; }

        bool LogIn(User user);
        void LogOut();
        void SetOldUser();
        void SetObserver();
        bool CheckRoleAcsess(IEnumerable<Role> roles);
    }
}