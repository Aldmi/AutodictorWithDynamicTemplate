using System;
using System.Linq;
using System.Windows.Forms;
using AutodictorBL.DataAccess;
using AutodictorBL.Services;
using Autofac.Features.OwnedInstances;
using DAL.Abstract.Entitys.Authentication;


namespace MainExample
{
    public partial class AuthenticationForm : Form
    {
        private readonly IDisposable _userServiceOwner;
        private readonly UserService _userService;
        private readonly IAuthentificationService _authentificationService;



        #region ctor

        public AuthenticationForm(Owned<UserService> userService, IAuthentificationService authentificationService)
        {
            _userService = userService.Value;
            _userServiceOwner = userService;
            _authentificationService = authentificationService;

            InitializeComponent();
            CreateMyPasswordTextBox();

            //Установить фокус на кнопку
            btn_Enter.Focus();
            btn_Enter.Select();

            cb_Roles.DataSource = Enum.GetValues(typeof(Role));
            cb_Roles.SelectedItem = Role.Диктор;
        }

        #endregion





        #region Methode

        public void CreateMyPasswordTextBox()
        {
            tb_password.MaxLength = 32;
            tb_password.PasswordChar = '*';
            tb_password.TextAlign = HorizontalAlignment.Center;
        }

        #endregion





        #region EventHandler

        private void cb_Roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb != null)
            {
                var role = (Role)cb.SelectedItem;
                if (role == Role.Наблюдатель)
                {
                    cb_Users.Enabled = false;
                    cb_Users.DataSource = null;
                    tb_password.Enabled = false;
                    return;
                }

                cb_Users.Enabled = true;
                tb_password.Enabled = true;
                var users = _userService.GetAllUsersWithRole(role).ToList();
                if (!users.Any())
                {
                    cb_Users.DataSource = null;
                    return;
                }

                cb_Users.DataSource = users;
                cb_Users.DisplayMember = "Login";
            }
        }


        private void btn_Enter_Click(object sender, EventArgs e)
        {
            //Если пользователь не выбран из БД, логиним Наблюдателя.
            var loginUser = (User)cb_Users.SelectedItem ?? _authentificationService.CreateObserver(); // если выбрано какое-либо значение, пишем в переменную, иначе задаем нового пользователя с ролью Наблюдатель
            loginUser.Password = tb_password.Text;

            if (!_authentificationService.LogIn(loginUser))
            {
                MessageBox.Show(@"НЕ ВЕРНЫЙ ПАРОЛЬ!!!");
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }


        protected override void OnClosed(EventArgs e)
        {
            _userServiceOwner.Dispose();
            base.OnClosed(e);
        }

        #endregion
    }
}
