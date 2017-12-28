using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Entitys.Authentication;
using MainExample.Services;

namespace MainExample
{
    public partial class AuthenticationForm : Form
    {
        #region ctor

        public AuthenticationForm()
        {
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
            tb_password.MaxLength = 8;
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
                var role= (Role)cb.SelectedItem;
                if (role == Role.Наблюдатель)
                {
                    cb_Users.Enabled = false;
                    cb_Users.DataSource = null;
                    tb_password.Enabled = false;
                    return;
                }

               cb_Users.Enabled = true;
               tb_password.Enabled = true;
               var users = Program.UsersDbRepository.List(user => user.Role == role).ToList();
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
            var loginUser= (User)cb_Users.SelectedItem ?? new User {Login = "НАБЛЮДАТЕЛЬ", Role = Role.Наблюдатель};
            loginUser.Password = tb_password.Text;

            if (!Program.AuthenticationService.LogIn(loginUser))
            {
                MessageBox.Show(@"НЕ ВЕРНЫЙ ПАРОЛЬ!!!");
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion
    }
}
