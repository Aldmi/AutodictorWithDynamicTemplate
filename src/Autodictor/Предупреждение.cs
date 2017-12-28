using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MainExample
{
    public partial class Предупреждение : Form
    {
        public int Таймер = 20;
        private ListViewItem myItem;

        public Предупреждение(ListViewItem item)
        {
            myItem = item;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            //MainWindowForm.myMainForm.ОтключитьСообщение(myItem);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Таймер > 0)
            {
                Таймер--;
                lbl_Timer.Text = "00:00:" + Таймер.ToString("00");
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.No;
                this.Close();
            }
        }
    }
}
