using System;
using System.Windows.Forms;
using CommunicationDevices.Verification;

namespace MainExample
{
    public partial class AboutForm : Form
    {
        private readonly IVerificationActivation _verificationActivation;




        #region ctor

        public AboutForm(IVerificationActivation verificationActivation)
        {
            _verificationActivation = verificationActivation;
            InitializeComponent();
        }

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            tb_ActivationInfo.Text = $@"До блокировки программы осталось {_verificationActivation.GetDeltaDayBeforeBlocking()} дней";
            base.OnLoad(e);
        }


        private void btn_activationWindow_Click(object sender, EventArgs e)
        {
            if (BlockingForm.MyMainForm == null)
            {
                var blockingForm = new BlockingForm(_verificationActivation);
                blockingForm.ShowDialog();

                tb_ActivationInfo.Text = $@"До блокировки программы осталось {_verificationActivation.GetDeltaDayBeforeBlocking()} дней";
            }
        }
    }
}
