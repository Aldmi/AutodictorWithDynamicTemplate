using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys.Authentication;

namespace AutodictorBL.Services
{
    public class AuthenticationService : IAuthentificationService
    {
        #region field

        private readonly IUsersRepository _usersRepository;

        #endregion




        #region prop

        public bool IsAuthentication { get; private set; }
        public User CurrentUser { get; private set; }
        public User OldUser { get; private set; }

        #endregion




        #region ctor

        public AuthenticationService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Вход пользователя
        /// </summary>
        public bool LogIn(User user)
        {
            if (user.Role == Role.Наблюдатель)
            {
                SetObserver();
                return true;
            }

            var existUser = _usersRepository.List(u => (u.Role == user.Role) &&
                                                                (u.Login == user.Login) &&
                                                                (u.Password == user.Password)).FirstOrDefault();
            if (existUser == null)
            {
                LogOut();
                return false;
            }

            CurrentUser = existUser;
            IsAuthentication = true;
            return true;
        }



        /// <summary>
        /// Выход пользователя
        /// </summary>
        public void LogOut()
        {
            OldUser = CurrentUser;
            CurrentUser = null;
            IsAuthentication = false;
        }



        /// <summary>
        /// Установить пользователя с правами НАБЛЮДАТЕЛЬ
        /// </summary>
        public void SetOldUser()
        {
            IsAuthentication = true;
            CurrentUser = OldUser;
        }


        /// <summary>
        /// Установить пользователя с правами НАБЛЮДАТЕЛЬ
        /// </summary>
        public void SetObserver()
        {
            IsAuthentication = true;
            CurrentUser = new User { Login = "Наблюдатель", Role = Role.Наблюдатель };
        }



        /// <summary>
        /// Проверка доступа по ролям
        /// </summary>
        public bool CheckRoleAcsess(IEnumerable<Role> roles)
        {
            return roles.Contains(CurrentUser.Role);
        }


        #endregion
    }
}