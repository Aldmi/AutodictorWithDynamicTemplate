using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainExample
{
    public partial class СписокСтанций : Form
    {
        private readonly string[] _станцииВыбранногоНаправления;



        public СписокСтанций(string списокСтанций, string[] станцииВыбранногоНаправления)
        {
            _станцииВыбранногоНаправления = станцииВыбранногоНаправления;
            InitializeComponent();

            string[] выбранныеСтанции = списокСтанций.Split(',');

            foreach (var станция in станцииВыбранногоНаправления)
                if (выбранныеСтанции.Contains(станция))
                    lVВыбранныеСтанции.Items.Add(станция);
                else
                    lVОбщийСписок.Items.Add(станция);
        }



        private void btnВыбратьВсе_Click(object sender, EventArgs e)
        {
            lVОбщийСписок.Items.Clear();

            lVВыбранныеСтанции.Items.Clear();
            foreach (var Станция in _станцииВыбранногоНаправления)
                lVВыбранныеСтанции.Items.Add(Станция);
        }



        private void btnУдалитьВсе_Click(object sender, EventArgs e)
        {
            lVВыбранныеСтанции.Items.Clear();

            lVОбщийСписок.Items.Clear();
            foreach (var Станция in _станцииВыбранногоНаправления)
                lVОбщийСписок.Items.Add(Станция);
        }



        private void btnВыбратьВыделенные_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.lVОбщийСписок.SelectedIndices;

            int DeletedCounter = 0;
            foreach (int item in sic)
            {
                lVВыбранныеСтанции.Items.Add(this.lVОбщийСписок.Items[item - DeletedCounter].SubItems[0].Text);
                this.lVОбщийСписок.Items[item - DeletedCounter].Remove();
                DeletedCounter++;
            }
        }

        private void btnУдалитьВыбранные_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.lVВыбранныеСтанции.SelectedIndices;

            int DeletedCounter = 0;
            foreach (int item in sic)
            {
                lVОбщийСписок.Items.Add(this.lVВыбранныеСтанции.Items[item - DeletedCounter].SubItems[0].Text);
                this.lVВыбранныеСтанции.Items[item - DeletedCounter].Remove();
                DeletedCounter++;
            }
        }

        public List<string> ПолучитьСписокВыбранныхСтанций()
        {
            List<string> TempList = new List<string>();
            List<string> Result = new List<string>();

            for (int i = 0; i < lVВыбранныеСтанции.Items.Count; i++)
                TempList.Add(lVВыбранныеСтанции.Items[i].SubItems[0].Text);

            foreach (var станция in _станцииВыбранногоНаправления)
                if (TempList.Contains(станция))
                    Result.Add(станция);

            return Result;
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
