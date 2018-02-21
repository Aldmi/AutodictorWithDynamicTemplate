using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys.Authentication;
using BCrypt.Net;


namespace AutodictorBL.Services
{
    public class AuthenticationService : IAuthentificationService
    {
        #region field

        private const string HardAdminSalt = "AssIbir2018Super10987612345"; // дополнительная соль
        private const int Complexity = 12; // сложность вычисления хэш-функции

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

            DateTime today = DateTime.Today;
            var usr = _usersRepository.List(u => (u.Role == user.Role) &&
                                                          (u.Login == user.Login) &&
                                                          (u.IsEnabled) &&
                                                          (u.StartDate <= today && u.EndDate >= today)).FirstOrDefault(); // находим пользователя, у которого совпала и роль, и логин
            var existUser = IsCorrectPassword(user.Password, usr?.Password ?? string.Empty) ? usr : null; // верифицируем пароль

            if (existUser == null)
            {
                LogOut();
                return false;
            }

            CurrentUser = existUser;
            IsAuthentication = true;

            return true;
        }


        public string CreateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
        public string CreateSalt(int workFactor)
        {
            return BCrypt.Net.BCrypt.GenerateSalt(workFactor);
        }

        public string Crypt(string password)
        {
            //string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hash = BCrypt.Net.BCrypt.HashPassword(password + HardAdminSalt);
            return hash;
        }
        public string Crypt(string password, int workFactor)
        {
            //string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hash = BCrypt.Net.BCrypt.HashPassword(password + HardAdminSalt, workFactor);
            return hash;//hash;
        }

        public bool IsCorrectPassword(string password, string hash)
        {
            try
            {
                return hash.StartsWith("$2") ? BCrypt.Net.BCrypt.Verify(password + HardAdminSalt, hash) : password == hash; // совместимое условие проверки пароля для старый версий репозитория
            }
            catch (SaltParseException ex)
            {
                Console.WriteLine("Некорректная соль: " + ex.Message);
                return false;
            }
            catch (BcryptAuthenticationException ex)
            {
                Console.WriteLine("Исключение аутентификации: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Неизвестное исключение при проверке пароля: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Создание пользователя с заданным логином, паролем и ролью
        /// </summary>
        public User CreateUser(string login, string password, Role role)
        {
            return new User
            {
                Login = login,
                Password = password.StartsWith("$2") ? password : Crypt(password, Complexity),
                Role = role,
                StartDate = new DateTime(1900, 01, 01),
                EndDate = new DateTime(2100, 12, 31),
                FullName = login,
                IsEnabled = true
            };
        }
        public User CreateUser(string login, string password, Role role, string fullName)
        {
            return new User
            {
                Login = login,
                Password = password.StartsWith("$2") ? password : Crypt(password, Complexity),
                Role = role,
                StartDate = new DateTime(1900, 01, 01),
                EndDate = new DateTime(2100, 12, 31),
                FullName = fullName,
                IsEnabled = true
            };
        }
        public User CreateUser(string login, string password, Role role, DateTime startDate, DateTime endDate)
        {
            return new User
            {
                Login = login,
                Password = password.StartsWith("$2") ? password : Crypt(password, Complexity),
                Role = role,
                StartDate = startDate,
                EndDate = endDate,
                FullName = login,
                IsEnabled = true
            };
        }
        public User CreateUser(string login, string password, Role role, DateTime startDate, DateTime endDate, string fullName)
        {
            return new User
            {
                Login = login,
                Password = password.StartsWith("$2") ? password : Crypt(password, Complexity),
                Role = role,
                StartDate = startDate,
                EndDate = endDate,
                FullName = fullName,
                IsEnabled = true
            };
        }
        public User CreateUser(string login, string password, Role role, DateTime startDate, DateTime endDate, string fullName, bool IsEnabled)
        {
            return new User
            {
                Login = login,
                Password = password.StartsWith("$2") ? password : Crypt(password, Complexity),
                Role = role,
                StartDate = startDate,
                EndDate = endDate,
                FullName = fullName,
                IsEnabled = IsEnabled
            };
        }

        public User CreateObserver()
        {
            return CreateUser("Наблюдатель", string.Empty, Role.Наблюдатель);
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