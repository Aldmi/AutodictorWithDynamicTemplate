using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using AutodictorBL.Entites;
using AutodictorBL.Sound;
using DAL.Abstract.Entitys;
using MainExample.Extension;


namespace MainExample
{
    public partial class SoundPlayersForm : Form
    {
        public static SoundPlayersForm MyMainForm = null;



        #region prop

        private ISoundPlayer SoundPlayer { get; set; }
        IDisposable DisposeIsConnectChangeRx { get; }
        IDisposable DisposeStatusStringChangeRx { get; }

        #endregion




        #region ctor

        public SoundPlayersForm()
        {
            if (MyMainForm != null)
                return;
            MyMainForm = this;

            SoundPlayer = Program.AutodictorModel.SoundPlayer;

            DisposeIsConnectChangeRx = SoundPlayer.IsConnectChangeRx.Subscribe(isConnect =>
            {
                tb_IsConnect.InvokeIfNeeded(() =>
                {
                    tb_IsConnect.Text = isConnect ? "Да" : "Нет";
                    tb_IsConnect.BackColor = isConnect ? Color.Green : Color.Red;

                    btn_GetInfo.Enabled = isConnect;
                    btn_Play.Enabled = isConnect;
                });
            });

            DisposeStatusStringChangeRx = SoundPlayer.StatusStringChangeRx.Subscribe(statusStr =>
            {
                tb_Status.InvokeIfNeeded(() =>
                {
                    tb_Status.Text = statusStr;
                });
            });

            InitializeComponent();
        }

        #endregion





        #region EventHandler

        protected override void OnLoad(EventArgs e)
        {
            tb_PlayerType.Text = SoundPlayer.PlayerType.ToString();

            tb_IsConnect.Text = SoundPlayer.IsConnect ? "Да" : "Нет";
            tb_IsConnect.BackColor = SoundPlayer.IsConnect ? Color.Green : Color.Red;
            btn_GetInfo.Enabled = SoundPlayer.IsConnect;
            btn_Play.Enabled = SoundPlayer.IsConnect;

            tb_Status.Text = SoundPlayer.StatusString;

            base.OnLoad(e);
        }


        private async void btn_Reconnect_Click(object sender, EventArgs e)
        {
            await SoundPlayer.ReConnect();
        }


        private void btn_GetInfo_Click(object sender, EventArgs e)
        {
            rtb_GetInfo.Text = SoundPlayer.GetInfo();
        }


        private void btn_Play_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_FileName.Text))
            {
                MessageBox.Show(@"Введите имя файла!!!");
                return;
            }

            var soundMessage = new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = tb_FileName.Text, Язык = NotificationLanguage.Rus };
            SoundPlayer.PlayFile(soundMessage);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyMainForm == this)
                MyMainForm = null;

            DisposeIsConnectChangeRx?.Dispose();
            DisposeStatusStringChangeRx?.Dispose();

            base.OnClosing(e);
        }

        #endregion
    }
}
