using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using AutodictorBL.DataAccess;
using DAL.Abstract.Entitys;
using DAL.NoSqlLiteDb.Entityes;


namespace MainExample
{
    public partial class ArchiveChangesForm : Form
    {
        private readonly SoundRecChangesService _soundRecChangesService;
        public static ArchiveChangesForm myMainForm = null;
        public List<SoundRecordChangesDb> RecordChanges { get; set; }




        #region ctor

        public ArchiveChangesForm(SoundRecChangesService soundRecChangesService)
        {
            if (myMainForm != null)
                return;
            myMainForm = this;

            _soundRecChangesService = soundRecChangesService;
            InitializeComponent();
        }

        #endregion




        #region EventHandlers

        private void btn_Поиск_Click(object sender, EventArgs e)
        {
            var startDate = dtp_Начало.Value;
            var endDate = dtp_Конец.Value;
            var db= _soundRecChangesService.GetByDateRange(startDate, endDate, changesDb => true)?.ToList();
            if(db == null)
                return;


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
                dgv_архив.Rows.Clear();
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
