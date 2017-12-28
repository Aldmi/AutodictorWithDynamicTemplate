using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Entitys;


namespace MainExample
{
    public partial class ОкноАрхиваИзменений : Form
    {
        public static ОкноАрхиваИзменений myMainForm = null;


        public List<SoundRecordChangesDb> RecordChanges { get; set; }



        #region ctor

        public ОкноАрхиваИзменений()
        {
            if (myMainForm != null)
                return;

            myMainForm = this;

            InitializeComponent();
        }

        #endregion






        #region EventHandlers


        private void btn_Поиск_Click(object sender, EventArgs e)
        {
            var startDate = dtp_Начало.Value;
            var endDate = dtp_Конец.Value;

            var db=  Program.SoundRecordChangesDbRepository.List();
            var query= db.Where(rec => (rec.TimeStamp >= startDate) && (rec.TimeStamp <= endDate));

            if (cb_ПоменялиПуть.Checked)
            {
                query = query.Where(rec => rec.Rec.НомерПути != rec.NewRec.НомерПути);
            }

            if (cb_ПоменялиВремя.Checked)
            {
                query = query.Where(rec => rec.Rec.Время != rec.NewRec.Время);
            }

            if (!string.IsNullOrWhiteSpace(tb_НомерПоезда.Text))
            {
                query = query.Where(rec => rec.Rec.НомерПоезда == tb_НомерПоезда.Text);
            }

            RecordChanges?.Clear();
            RecordChanges = query.ToList();

            ShowRecords();
        }

        #endregion





        private void ShowRecords()
        {
            if (!RecordChanges.Any())
            {
                MessageBox.Show(@"Поиск не дал результатов");
                return;
            }

            List<string[]> rows= new List<string[]>();
            foreach (var ch in RecordChanges)
            {
                rows.Add(new string[] {ch.ScheduleId.ToString(), ch.TimeStamp.ToString("G"), ch.UserInfo, ch.CauseOfChange, ch.ToString() });
            }

            dgv_архив.Rows.Clear();
            foreach (var row in rows)
            {
               dgv_архив.Rows.Add(row);
            }
        }




        protected override void OnClosing(CancelEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;

            base.OnClosing(e);
        }
    }
}
