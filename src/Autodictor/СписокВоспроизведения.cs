using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MainExample.Extension;



namespace MainExample
{
    public partial class СписокВоспроизведения : Form
    {
        #region prop

        private Timer Timer { get; set; }

        #endregion





        #region ctor

        public СписокВоспроизведения()
        {
            Timer= new Timer {Interval = 100};
            Timer.Tick += Timer_Tick;
            InitializeComponent();
        }

        #endregion





        protected override void OnLoad(EventArgs e)
        {
            Timer.Start();
            base.OnLoad(e);
        }


        protected override void OnClosed(EventArgs e)
        {
            if (Timer != null)
            {
                Timer.Tick -= Timer_Tick;
                Timer.Dispose();
            }

            base.OnClosed(e);
        }


        private int _oldCountElemQueue;
        private int _oldCountElemFiles;
        private bool _oldIsStaticSoundPlaying;
        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentCount = MainWindowForm.QueueSound.GetElementsWithFirstElem.Count();
            if (currentCount != _oldCountElemQueue)
            {
                _oldCountElemQueue = currentCount;
                //Сработка при изменении кол-ва элементов в очереди
                ОбновитьСодержимоеСпискаЭлементов();
                ВизуализироватьСписокЭлементов(lVСписокЭлементов);
                ОбновитьСодержимоеСпискаФайлов();
                return;
            }


            if(MainWindowForm.QueueSound.GetElementsOnTemplatePlaying != null && MainWindowForm.QueueSound.GetElementsOnTemplatePlaying.Any())
            {
                var currentCountElem = MainWindowForm.QueueSound.GetElementsOnTemplatePlaying.Count();
                if (currentCountElem != _oldCountElemFiles)
                {
                    _oldCountElemFiles = currentCountElem;
                    //Сработка при изменении кол-ва файлов в очереди проигрывания шаблона
                    ОбновитьСодержимоеСпискаФайлов();
                }   
            }


            if (MainWindowForm.QueueSound.IsStaticSoundPlaying != _oldIsStaticSoundPlaying)
            {
                _oldIsStaticSoundPlaying = MainWindowForm.QueueSound.IsStaticSoundPlaying;
                ОбновитьСодержимоеСпискаФайлов();
            }
        }



        private void btn_StartStopQueue_Click(object sender, EventArgs e)
        {
            if (MainWindowForm.QueueSound.IsWorking)
            {
                MainWindowForm.QueueSound.StopQueue();
                btn_StartStopQueue.Text = "Старт";
            }
            else
            {
                MainWindowForm.QueueSound.StartQueue();
                btn_StartStopQueue.Text = "Стоп";
            }
        }



        private void btn_СlearQueue_Click(object sender, EventArgs e)
        {
            var resultDialog= MessageBox.Show(this, @"Точно очистить?", @"Удаление...", MessageBoxButtons.OKCancel );
            if (resultDialog == DialogResult.OK)
            {
                MainWindowForm.QueueSound.Clear();
            }        
        }




        #region Methode

        public void ОбновитьСодержимоеСпискаЭлементов()
        {
            lVСписокЭлементов.InvokeIfNeeded(() =>
            {
                lVСписокЭлементов.Items.Clear();
                try
                {
                    foreach (var elem in MainWindowForm.QueueSound.GetElementsWithFirstElem)
                    {
                        ListViewItem lvi1 = new ListViewItem(new string[] {elem.ИмяВоспроизводимогоФайла});
                        this.lVСписокЭлементов.Items.Add(lvi1);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ОбновитьСодержимоеСпискаЭлементов = {ex.ToString()}");//DEBUG
                }
            });
        }



        public void ОбновитьСодержимоеСпискаФайлов()
        {
            lVСписокФайлов.InvokeIfNeeded(() =>
            {
                lVСписокФайлов.Items.Clear();
                try
                {
                    if (MainWindowForm.QueueSound.IsStaticSoundPlaying)
                    {
                        ListViewItem lvi1 = new ListViewItem(new string[] { MainWindowForm.QueueSound.CurrentSoundMessagePlaying.ИмяВоспроизводимогоФайла });
                        this.lVСписокФайлов.Items.Add(lvi1);
                    }
                    else
                    {
                        if(MainWindowForm.QueueSound.GetElementsOnTemplatePlaying == null)
                            return;

                        foreach (var elem in MainWindowForm.QueueSound.GetElementsOnTemplatePlaying)
                        {
                            ListViewItem lvi1 = new ListViewItem(new string[] { elem.ИмяВоспроизводимогоФайла });
                            this.lVСписокФайлов.Items.Add(lvi1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ОбновитьСодержимоеСпискаФайлов = {ex.ToString()}");//DEBUG
                }
            });
        }


        public void ВизуализироватьСписокЭлементов(ListView lv)
        {
            var listElem = MainWindowForm.QueueSound.GetElementsWithFirstElem.ToList();
            lVСписокЭлементов.InvokeIfNeeded(() =>
            {
                try
                {
                    for (int item = 0; item < lv.Items.Count; item++)
                    {
                        if (item == 0)
                        {
                            lv.Items[item].Font = new Font(FontFamily.GenericSansSerif, 18);
                            continue;
                        }

                        lv.Items[item].ForeColor = listElem[item].ОчередьШаблона == null
                            ? Color.Brown
                            : Color.DodgerBlue;

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ВизуализироватьСписокЭлементов = {ex.ToString()}");//DEBUG
                };
            });

        }


        #endregion


    }
}
