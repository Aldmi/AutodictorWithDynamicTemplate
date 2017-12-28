using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entitys.Authentication;

namespace MainExample.Services
{
    public class AuthenticationService
    {
        #region prop

        public bool IsAuthentication { get; private set; }
        public User CurrentUser { get; private set; }

        public User OldUser { get; private set; }

        #endregion





        #region Methode

        /// <summary>
        /// Инициализация NoSql БД
        /// </summary>
        public async Task UsersDbInitialize()
        {
            await Task.Factory.StartNew(() =>
            {
                string adminLogin = "Админ";
                string adminPassword = "123456";

                var admin = Program.UsersDbRepository.List(user => (user.Role == Role.Администратор) &&
                                                                   (user.Login == adminLogin)).FirstOrDefault();
                if (admin == null)
                {
                    Program.UsersDbRepository.Add(new User { Login = adminLogin, Password = adminPassword, Role = Role.Администратор });
                }
            }
             );
        }


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

            var existUser = Program.UsersDbRepository.List(u => (u.Role == user.Role) &&
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
            CurrentUser = new User {Login = "Наблюдатель", Role = Role.Наблюдатель};
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
