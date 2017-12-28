using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.Verification;

namespace MainExample
{
    public partial class BlockingForm : Form
    {
        private readonly IVerificationActivation _verificationActivation;
        public static BlockingForm MyMainForm = null;




        #region ctor

        public BlockingForm(IVerificationActivation verificationActivation)
        {
            if (MyMainForm != null)
                return;
            MyMainForm = this;

            _verificationActivation = verificationActivation;

            InitializeComponent();
        }

        #endregion





        protected override void OnLoad(EventArgs e)
        {
            if (_verificationActivation.IsBlock)   //форма блокировки
            {
                tb_message.Text = 
                    $@"Внимание!! 
 БЛОКИРОВКА
 При закрытии этого окна будет закрыта программа";
            }
            else                                  //форма предупреждения
            {
                tb_message.Text =
                    $@"Внимание!! 
 Последняя активация выполнялась {_verificationActivation.GetDeltaDay()} дней назад.
 до блокировки программы осталось {_verificationActivation.GetDeltaDayBeforeBlocking()} дней ";
            }

            base.OnLoad(e);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyMainForm == this)
                MyMainForm = null;

            base.OnClosing(e);
            if (_verificationActivation.IsBlock) //закрыть программу
            {
                Environment.Exit(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_verificationActivation.ResetActivation(tb_password.Text))
            {
                tb_StatusActivation.Text = @"Активация продленна успешно еще на 90 дней.";
                tb_StatusActivation.BackColor=Color.Green;
            }
            else
            {
                tb_StatusActivation.Text = @"КОД АКТИВАЦИИ НЕ ВЕРЕН";
                tb_StatusActivation.BackColor = Color.Red;
            }
        }
    }
}
