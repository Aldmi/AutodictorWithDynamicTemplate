using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AutodictorBL.Services.SoundFileServices;


namespace MainExample
{
    public partial class CheckSoundFilesExistsForm : Form
    {
        private readonly IEnumerable<SoundFileError> _errors;




        #region ctor

        public CheckSoundFilesExistsForm(IEnumerable<SoundFileError> errors)
        {
            _errors = errors;
            InitializeComponent();
        }

        #endregion




        #region EventHandler

        protected override void OnLoad(EventArgs e)
        {
            dgv_Errors.DataSource = _errors;

        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }


}
