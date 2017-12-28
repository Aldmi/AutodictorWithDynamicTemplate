using System;
using System.Drawing;
using System.Windows.Forms;





namespace MainExample
{

    public partial class Расписание : Form
    {
        private byte ИндексТекущегоМесяца = 0;
        private byte ПредыдущийИндексТекущегоМесяца = 0;
        private ПланРасписанияПоезда РасписаниеПоезда;
        private DateTime ВремяПоследнегоНажатияКлавиатуры = DateTime.Now;
        private bool ПризнакИзмененияСтроки = false;
        private РежимРасписанияДвиженияПоезда СтарыйРежимРасписания = РежимРасписанияДвиженияПоезда.Отсутствует;
        private bool ИнициализацияЗавершена = false;

        //string[] Месяцы = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь", "Январь", "Февраль" };
        private string[] stringMonths = new string[14];

        public int[] intMonths = new int[14];
        public enum Months { Январь = 1, Февраль, Март, Апрель, Май, Июнь, Июль, Август, Сентябрь, Октябрь, Ноябрь, Декабрь }
        // byte[] КоличествоДнейВМесяце = new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31, 28 };
        // Назначить английское название функции
        private UInt32 СтепеньЧисла(int number, int степень)
        {
            if (степень == 0) return 1;

            int i = 0;
            if (степень > 0)
                i = степень - 1;

            if (степень < 0)
                i = степень + 1;
            return (UInt32)number * СтепеньЧисла(number, i);
        }



        public Расписание(ПланРасписанияПоезда РасписаниеПоезда)
        {
            InitializeComponent();

            this.РасписаниеПоезда = РасписаниеПоезда;

            for (int i = 0; i < intMonths.Length; i++) // Цикл по количеству прописанных месяцев в календаре
            {
                int monthIndex = i + DateTime.Now.Month - 1;
                int currentYear = DateTime.Now.Year + monthIndex / 12;
                intMonths[i] = monthIndex % 12 + 1;
                int currentMonth = intMonths[i];
                stringMonths[i] = (Months)currentMonth + " " + currentYear;
             
                string[] ШаблонСтроки = new string[38]; // Массив в 38 строк (максимальное количество элементов в длину)
                for (int j = 0; j < ШаблонСтроки.Length; j++)
                    ШаблонСтроки[j] = " ";

                ШаблонСтроки[0] = stringMonths[i]; // Заполняем колонку месяцев
                int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
                
                int dayOfWeek = ((byte) ((new DateTime(currentYear, currentMonth, 1)).DayOfWeek) + 6) % 7;
                for (int j = 1; j <= daysInMonth; j++) // от 1 до количества дней
                    ШаблонСтроки[j + dayOfWeek] = j.ToString(); // Пишем в клетки числа дней месяца в зависимости от того, на какой день недели выпадает его начало

                ListViewItem item = new ListViewItem(ШаблонСтроки, 0); // Добавляем колонки на ListView
                item.UseItemStyleForSubItems = false;

                for (int j = 1; j <= daysInMonth; j++)
                    item.SubItems[j + dayOfWeek].Tag = j + dayOfWeek; // тэгу задаем положение первого дня месяца в строке

                
                listView1.Items.Add(item); // Рисуем дни месяца на форме
            }

            this.Text = "Расписание движения поезда: " + РасписаниеПоезда.ПолучитьНомерПоезда() + " - " + РасписаниеПоезда.ПолучитьНазваниеПоезда(); // Заголовок
            
            int АктивностьДняДвижения = РасписаниеПоезда.ПолучитьАктивностьПоездаВКрайниеДни();
            if ((АктивностьДняДвижения & 0x40000) != 0x00000) cBНаГрМес.Checked = true; // 0x40000
            if ((АктивностьДняДвижения & 0x00001) != 0x00000) cBГр26.Checked = true; // 0x00001
            if ((АктивностьДняДвижения & 0x00002) != 0x00000) cBГр27.Checked = true; // 0x00002
            if ((АктивностьДняДвижения & 0x00004) != 0x00000) cBГр28.Checked = true; // 0x00004
            if ((АктивностьДняДвижения & 0x00008) != 0x00000) cBГр29.Checked = true; // 0x00008
            if ((АктивностьДняДвижения & 0x00010) != 0x00000) cBГр30.Checked = true; // 0x00010
            if ((АктивностьДняДвижения & 0x00020) != 0x00000) cBГр31.Checked = true; // 0x00020
            if ((АктивностьДняДвижения & 0x00040) != 0x00000) cBГр1.Checked = true; // 0x00040
            if ((АктивностьДняДвижения & 0x00080) != 0x00000) cBГр2.Checked = true; // 0x00080
            if ((АктивностьДняДвижения & 0x00100) != 0x00000) cBГр3.Checked = true; // 0x00100
            if ((АктивностьДняДвижения & 0x00200) != 0x00000) cBГр4.Checked = true; // 0x00200
            if ((АктивностьДняДвижения & 0x00400) != 0x00000) cBГр5.Checked = true; // 0x00400
            if ((АктивностьДняДвижения & 0x00800) != 0x00000) cBГр6.Checked = true; // 0x00800
            if ((АктивностьДняДвижения & 0x01000) != 0x00000) cBГр7.Checked = true; // 0x01000
            if ((АктивностьДняДвижения & 0x02000) != 0x00000) cBГр8.Checked = true; // 0x02000
            if ((АктивностьДняДвижения & 0x04000) != 0x00000) cBГр9.Checked = true; // 0x04000
            if ((АктивностьДняДвижения & 0x08000) != 0x00000) cBГр10.Checked = true; // 0x08000
            if ((АктивностьДняДвижения & 0x10000) != 0x00000) cBГр11.Checked = true; // 0x10000
            if ((АктивностьДняДвижения & 0x20000) != 0x00000) cBГр12.Checked = true; // 0x20000

            UInt16 ДниНедели = РасписаниеПоезда.ПолучитьАктивностьПоДнямНедели();
            if ((ДниНедели & 0x0001) != 0x0000) cBПн.Checked = true;
            if ((ДниНедели & 0x0002) != 0x0000) cBВт.Checked = true;
            if ((ДниНедели & 0x0004) != 0x0000) cBСр.Checked = true;
            if ((ДниНедели & 0x0008) != 0x0000) cBЧт.Checked = true;
            if ((ДниНедели & 0x0010) != 0x0000) cBПт.Checked = true;
            if ((ДниНедели & 0x0020) != 0x0000) cBСб.Checked = true;
            if ((ДниНедели & 0x0040) != 0x0000) cBВск.Checked = true;
            if ((ДниНедели & 0x0100) != 0x0000) cBКрПн.Checked = true;
            if ((ДниНедели & 0x0200) != 0x0000) cBКрВт.Checked = true;
            if ((ДниНедели & 0x0400) != 0x0000) cBКрСр.Checked = true;
            if ((ДниНедели & 0x0800) != 0x0000) cBКрЧт.Checked = true;
            if ((ДниНедели & 0x1000) != 0x0000) cBКрПт.Checked = true;
            if ((ДниНедели & 0x2000) != 0x0000) cBКрСб.Checked = true;
            if ((ДниНедели & 0x4000) != 0x0000) cBКрВск.Checked = true;

            ЗадатьКодПереключателейРежима(РасписаниеПоезда.ПолучитьРежимРасписания());

            ИнициализацияЗавершена = true;

            ОбновитьЦветовуюМаркировкуРасписания();
        }

        public void УстановитьВремяДействия(string ВремяДействия)
        {
            lblВремяДействия.Text = ВремяДействия;
        }

        public ПланРасписанияПоезда ПолучитьПланРасписанияПоезда()
        {
            return РасписаниеПоезда;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void РежимРасписания_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == false)
                return;

            bool[] МассивАктивныхМесяцев = new bool[14];
            // bool РаботаПоВыбраннымМесяцам = false;

            CheckBox[] МассивЭлементовАктивныхМесяцев = new CheckBox[12] { cBЯнв, cBФев, cBМар, cBАпр, cBМай, cBИюн, cBИюл, cBАвг, cBСен, cBОкт, cBНоя, cBДек };

            int count = 0;
            for (int i = 0; i < intMonths.Length; i++)
            {
                int currentMonth = intMonths[i];
                if (cBРаспространитьНаВсе.Checked || МассивЭлементовАктивныхМесяцев[intMonths[i] - 1].Checked)
                {
                    МассивАктивныхМесяцев[currentMonth - 1] = true;
                    count++;
                }
            }
            // РаботаПоВыбраннымМесяцам
            if (count == 0)
                for (int i = 0; i < intMonths.Length; i++)
                    МассивАктивныхМесяцев[intMonths[i] - 1] = true; // Необходимо сделать выборку активных месяцев раздельно для заданных и раздельно для остальных
            
            for (byte i = 0; i < intMonths.Length; i++)
            {
                int currentMonth = intMonths[i];
                if (МассивАктивныхМесяцев[currentMonth - 1] == false) // И здесь
                    continue;
                int monthIndex = i + DateTime.Now.Month - 1;
                int currentYear = DateTime.Now.Year + monthIndex / 12;
                int lastMonth = currentMonth - 1;
                int lastMonthYear = currentYear;
                if (lastMonth == 0)
                {
                    lastMonthYear -= 1;
                    lastMonth = 12;
                }
                int daysInLastMonth = DateTime.DaysInMonth(lastMonthYear, lastMonth);

                РежимРасписанияДвиженияПоезда новыйРежимРасписания = ПолучитьКодПереключателейРежима();


                // if (СтарыйРежимРасписания != новыйРежимРасписания)
                //if (daysInLastMonth % 2 != 0)
                //{
                    
                    //{
                        switch (новыйРежимРасписания)
                        {
                            case РежимРасписанияДвиженияПоезда.Отсутствует:
                                for (byte j = 0; j < 32; j++)
                                    РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, j, false);
                                break;

                            case РежимРасписанияДвиженияПоезда.Ежедневно:
                                for (byte j = 0; j < 32; j++)
                                    РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, j, true);
                                break;

                            case РежимРасписанияДвиженияПоезда.ПоЧетным:
                                for (byte j = 0; j < 32; j++)
                                    РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, j, ((j % 2) == 1) ? true : false);
                                //if (daysInLastMonth % 2 == 0)
                                //{
                                //    МассивАктивныхМесяцев[currentMonth - 1] = false;
                                //    //continue;
                                //}
                                break;

                            case РежимРасписанияДвиженияПоезда.ПоНечетным:
                                for (byte j = 0; j < 32; j++)
                                    РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, j, ((j % 2) == 0) ? true : false);
                                //if (daysInLastMonth % 2 == 0)
                                //{
                                //    МассивАктивныхМесяцев[currentMonth - 1] = false;
                                //    //continue;
                                //}
                                break;

                            case РежимРасписанияДвиженияПоезда.Выборочно:
                                break;

                            case РежимРасписанияДвиженияПоезда.ПоДням:
                                ЗадатьАктивностьРасписанияПоДням();
                                break;
                        }

                        
                        if (cBНаГрМес.Checked && (daysInLastMonth % 2 != 0) && (новыйРежимРасписания == РежимРасписанияДвиженияПоезда.ПоНечетным || новыйРежимРасписания == РежимРасписанияДвиженияПоезда.ПоЧетным))
                        {
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 25, cBГр26.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 26, cBГр27.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 27, cBГр28.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 28, cBГр29.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 29, cBГр30.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 30, cBГр31.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 0, cBГр1.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 1, cBГр2.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 2, cBГр3.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 3, cBГр4.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 4, cBГр5.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 5, cBГр6.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 6, cBГр7.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 7, cBГр8.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 8, cBГр9.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 9, cBГр10.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 10, cBГр11.Checked);
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, 11, cBГр12.Checked);
                        }

                        РасписаниеПоезда.ЗадатьРежимРасписания(новыйРежимРасписания);
                        СтарыйРежимРасписания = новыйРежимРасписания;
                    //}
                //}
            }

            ОбновитьЦветовуюМаркировкуРасписания();
        }

        private РежимРасписанияДвиженияПоезда ПолучитьКодПереключателейРежима()
        {
            if (radioButton1.Checked == true)
                return РежимРасписанияДвиженияПоезда.Ежедневно;
            else if (radioButton2.Checked == true)
                return РежимРасписанияДвиженияПоезда.ПоЧетным;
            else if (radioButton3.Checked == true)
                return РежимРасписанияДвиженияПоезда.ПоНечетным;
            else if (radioButton4.Checked == true)
                return РежимРасписанияДвиженияПоезда.Выборочно;
            else if (radioButton6.Checked == true)
                return РежимРасписанияДвиженияПоезда.ПоДням;

            return РежимРасписанияДвиженияПоезда.Отсутствует;
        }

        private void ЗадатьКодПереключателейРежима(РежимРасписанияДвиженияПоезда КодПереключателейРежима)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            switch (КодПереключателейРежима)
            {
                case РежимРасписанияДвиженияПоезда.Ежедневно:
                    radioButton1.Checked = true; break;
                case РежимРасписанияДвиженияПоезда.ПоЧетным:
                    radioButton2.Checked = true; break;
                case РежимРасписанияДвиженияПоезда.ПоНечетным:
                    radioButton3.Checked = true; break;
                case РежимРасписанияДвиженияПоезда.Выборочно:
                    radioButton4.Checked = true; break;
                case РежимРасписанияДвиженияПоезда.ПоДням:
                    radioButton6.Checked = true; break;
                case РежимРасписанияДвиженияПоезда.Отсутствует:
                    radioButton5.Checked = true; break;
            }
            
        }

        private void ОбновитьЦветовуюМаркировкуРасписания()
        {
            for (byte i = 0; i < intMonths.Length; i++)
            {
                int monthIndex = i + DateTime.Now.Month - 1;
                int currentYear = DateTime.Now.Year + monthIndex / 12;
                intMonths[i] = monthIndex % 12 + 1;
                int currentMonth = intMonths[i];
                //string[] ШаблонСтроки = new string[36];
                
                int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
                //byte КоличествоДнейВТекущемМесяце = ((i == 1) && ((DateTime.Now.Year % 4) == 0)) || ((i == 13) && (((DateTime.Now.Year + 1) % 4) == 0)) ? (byte)29 : КоличествоДнейВМесяце[i];

                int dayOfWeek = ((byte)((new DateTime(currentYear, currentMonth, 1)).DayOfWeek) + 6) % 7;
                //DateTime НачалоМесяца = new DateTime(DateTime.Now.Year + (i / 12), (i % 12) + 1, 1);
                for (byte j = 1; j <= daysInMonth; j++)
                {
                    byte НомерСтолбца = (byte)(j + dayOfWeek);
                    bool АктивностьВТекущийДень = РасписаниеПоезда.ПолучитьАктивностьДняДвижения((byte)monthIndex, (byte)(j - 1));

                    ListViewItem.ListViewSubItem SubItem = listView1.Items[i].SubItems[НомерСтолбца];
                    SubItem.BackColor = АктивностьВТекущийДень ? Color.LightGreen : Color.White;
                    SubItem.ForeColor = ((НомерСтолбца % 7) == 6) || ((НомерСтолбца % 7) == 0) ? Color.Red : Color.Black;
                    listView1.Items[i].SubItems[НомерСтолбца] = SubItem;
                }
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y >= 24)
            {
                int rowIndex = (e.Y - 24) / 24;

                if (rowIndex < intMonths.Length)
                {
                    lblВыбранМесяц.Text = stringMonths[rowIndex];
                    ИндексТекущегоМесяца = (byte)rowIndex;
                    РежимРасписанияДвиженияПоезда РежимРасписанияПоезда = РасписаниеПоезда.ПолучитьРежимРасписания();
                    ЗадатьКодПереключателейРежима(РежимРасписанияПоезда);

                    if (ПредыдущийИндексТекущегоМесяца != ИндексТекущегоМесяца)
                    {
                        ПредыдущийИндексТекущегоМесяца = ИндексТекущегоМесяца;
                        ПризнакИзмененияСтроки = true;
                    }
                    else
                        ПризнакИзмененияСтроки = false;

                    int monthIndex = ИндексТекущегоМесяца + DateTime.Now.Month - 1;
                    // && ((DateTime.Now - ВремяПоследнегоНажатияКлавиатуры).Seconds < 1)
                    if ((ПризнакИзмененияСтроки == false) && (РежимРасписанияПоезда == РежимРасписанияДвиженияПоезда.Выборочно))
                    {
                        ListViewHitTestInfo lvhti;
                        lvhti = listView1.HitTest(new Point(e.X, e.Y));
                        ListViewItem.ListViewSubItem my4 = lvhti.SubItem;

                        int Число;
                        if (int.TryParse(my4.Text, out Число) == true)
                        {
                            int Tag = (int)my4.Tag;

                            if ((Tag >= 1) && (Tag <= 38) && (Число > 0) && (Число < 32))
                            {
                                bool ТекущаяАктивность = РасписаниеПоезда.ПолучитьАктивностьДняДвижения((byte)monthIndex, (byte)(Число - 1));
                                РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, (byte)(Число - 1), !ТекущаяАктивность);
                                //ОбновитьЦветовуюМаркировкуРасписания();
                            }
                        }
                    }

                    ВремяПоследнегоНажатияКлавиатуры = DateTime.Now;
                }
            }
            ОбновитьЦветовуюМаркировкуРасписания();
        }

        private void ИзмененоПоДням(object sender, EventArgs e)
        {
            if (ИнициализацияЗавершена == false)
                return; 

            CheckBox cb = sender as CheckBox;

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = true;

            if (cb.Checked == true)
                switch (cb.Name)
                {
                    case "cBПн": 
                    case "cBВт":
                    case "cBСр":
                    case "cBЧт":
                    case "cBПт":
                    case "cBСб":
                    case "cBВск":
                        cBКрПн.Checked = false;
                        cBКрВт.Checked = false;
                        cBКрСр.Checked = false;
                        cBКрЧт.Checked = false;
                        cBКрПт.Checked = false;
                        cBКрСб.Checked = false;
                        cBКрВск.Checked = false;
                        cBКрВ.Checked = false;
                        break;

                    case "cBКрПн":
                    case "cBКрВт":
                    case "cBКрСр":
                    case "cBКрЧт":
                    case "cBКрПт":
                    case "cBКрСб":
                    case "cBКрВск":
                        cBПн.Checked = false;
                        cBВт.Checked = false;
                        cBСр.Checked = false;
                        cBЧт.Checked = false;
                        cBПт.Checked = false;
                        cBСб.Checked = false;
                        cBВск.Checked = false;
                        cBВ.Checked = false;
                        break;
                }

            if ((cBСб.Checked == true) && (cBВск.Checked == true))
                cBВ.Checked = true;

            if ((cBКрСб.Checked == true) && (cBКрВск.Checked == true))
                cBКрВ.Checked = true;

            ЗадатьАктивностьРасписанияПоДням();
        }

        private void ЗадатьАктивностьРасписанияПоДням()
        {
            if (ИнициализацияЗавершена == false)
                return;

            bool[] МассивАктивныхДней = new bool[32];
            bool[] МассивАктивныхМесяцев = new bool[14];
            bool РаботаПоВыбраннымМесяцам = false;

            CheckBox[] МассивЭлементовАктивныхМесяцев = new CheckBox[12] { cBЯнв, cBФев, cBМар, cBАпр, cBМай, cBИюн, cBИюл, cBАвг, cBСен, cBОкт, cBНоя, cBДек };

            int count = 0;
            for (int i = 0; i < intMonths.Length; i++)
            {
                int currentMonth = intMonths[i];
                if (cBРаспространитьНаВсе.Checked || МассивЭлементовАктивныхМесяцев[intMonths[i] - 1].Checked)
                {
                    МассивАктивныхМесяцев[currentMonth - 1] = true;
                    count++;
                }
            }
            // РаботаПоВыбраннымМесяцам
            if (count == 0)
                for (int i = 0; i < intMonths.Length; i++)
                    МассивАктивныхМесяцев[intMonths[i] - 1] = true; // Заменить ИндексТекущегоМесяца

            //for (byte i = 0; i < intMonths.Length; i++)
            
            //    int currentMonth = intMonths[i];
            //    if (МассивАктивныхМесяцев[currentMonth - 1] == false) // И здесь
            //        continue;
                
                


            byte ПоДням = 0x00;
            if (cBПн.Checked) ПоДням |= 0x01;
            if (cBВт.Checked) ПоДням |= 0x02;
            if (cBСр.Checked) ПоДням |= 0x04;
            if (cBЧт.Checked) ПоДням |= 0x08;
            if (cBПт.Checked) ПоДням |= 0x10;
            if (cBСб.Checked) ПоДням |= 0x20;
            if (cBВск.Checked) ПоДням |= 0x40;
            if (cBВ.Checked) ПоДням |= 0x60;

            byte КромеДней = 0x00;
            if (cBКрПн.Checked) КромеДней |= 0x01;
            if (cBКрВт.Checked) КромеДней |= 0x02;
            if (cBКрСр.Checked) КромеДней |= 0x04;
            if (cBКрЧт.Checked) КромеДней |= 0x08;
            if (cBКрПт.Checked) КромеДней |= 0x10;
            if (cBКрСб.Checked) КромеДней |= 0x20;
            if (cBКрВск.Checked) КромеДней |= 0x40;
            if (cBКрВ.Checked) КромеДней |= 0x60;

            РасписаниеПоезда.ЗадатьАктивностьПоДнямНедели((ushort)((КромеДней << 8) | ПоДням));
            РасписаниеПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.ПоДням);

            for (int i = 0; i < intMonths.Length; i++)
            {

                int monthIndex = i + DateTime.Now.Month - 1;
                int currentYear = DateTime.Now.Year + monthIndex / 12;
                int currentMonth = intMonths[i];
                
                int dayOfWeek = (byte)((new DateTime(currentYear, currentMonth, 1)).DayOfWeek) + 6;

                if (МассивАктивныхМесяцев[currentMonth - 1])
                {
                    byte НомерПервогоДня = 0;
                    byte НомерПоследнегоДня = 30;

                    for (int j = 0; j < 32; j++)
                        МассивАктивныхДней[j] = false;

                    int lastMonth = currentMonth - 1;
                    int lastMonthYear = currentYear;
                    if (lastMonth == 0)
                    {
                        lastMonthYear -= 1;
                        lastMonth = 12;
                    }
                    int daysInLastMonth = DateTime.DaysInMonth(lastMonthYear, lastMonth);
                    
                    if ((cBНаГрМес.Checked == true) && (daysInLastMonth % 2 != 0))
                    {
                        МассивАктивныхДней[25] = cBГр26.Checked;
                        МассивАктивныхДней[26] = cBГр27.Checked;
                        МассивАктивныхДней[27] = cBГр28.Checked;
                        МассивАктивныхДней[28] = cBГр29.Checked;
                        if (daysInLastMonth > 29)
                        {
                            МассивАктивныхДней[29] = cBГр30.Checked;
                            МассивАктивныхДней[30] = cBГр31.Checked;
                        }
                        else
                        {
                            МассивАктивныхДней[29] = false;
                            МассивАктивныхДней[30] = false;
                        }
                        МассивАктивныхДней[0] = cBГр1.Checked;
                        МассивАктивныхДней[1] = cBГр2.Checked;
                        МассивАктивныхДней[2] = cBГр3.Checked;
                        МассивАктивныхДней[3] = cBГр4.Checked;
                        МассивАктивныхДней[4] = cBГр5.Checked;
                        МассивАктивныхДней[5] = cBГр6.Checked;
                        МассивАктивныхДней[6] = cBГр7.Checked;
                        МассивАктивныхДней[7] = cBГр8.Checked;
                        МассивАктивныхДней[8] = cBГр9.Checked;
                        МассивАктивныхДней[9] = cBГр10.Checked;
                        МассивАктивныхДней[10] = cBГр11.Checked;
                        МассивАктивныхДней[11] = cBГр12.Checked;
                        //НомерПервогоДня = 12;
                        //НомерПоследнегоДня = 24;
                    }
                    //else if ((cBНаГрМес.Checked == true) && (daysInLastMonth % 2 == 0))
                    //{
                      //  НомерПервогоДня = 0;
                        //НомерПоследнегоДня = (byte)(DateTime.DaysInMonth(currentYear, currentMonth) - 1);
                        //for (int j = НомерПервогоДня; j <= НомерПоследнегоДня; j++)
                        //{
                        //    МассивАктивныхДней[j] = ; // Найти откуда брать значения на основе типа расписания
                        //}
                    //}

                    if (ПоДням != 0x00)
                    {
                        for (int j = НомерПервогоДня; j <= НомерПоследнегоДня; j++)
                            if ((ПоДням & (0x01 << ((j + dayOfWeek) % 7))) != 0x00)
                                МассивАктивныхДней[j] = true;
                    }
                    else if (КромеДней != 0x00)
                    {
                        for (int j = НомерПервогоДня; j <= НомерПоследнегоДня; j++)
                            if ((КромеДней & (0x01 << ((j + dayOfWeek) % 7))) == 0x00)
                                МассивАктивныхДней[j] = true;
                    }


                    for (byte j = 0; j < 31; j++)
                        РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)monthIndex, j, МассивАктивныхДней[j]);
                }
            }

            ОбновитьЦветовуюМаркировкуРасписания();
        }

        private void ИзмененоНаГраницеМесяца(object sender, EventArgs e)
        {
            int АктивностьРасписанияВКрайниеДни = 0x00000;
            if (cBНаГрМес.Checked) АктивностьРасписанияВКрайниеДни |= 0x40000;
            if (cBГр26.Checked) АктивностьРасписанияВКрайниеДни |= 0x00001;
            if (cBГр27.Checked) АктивностьРасписанияВКрайниеДни |= 0x00002;
            if (cBГр28.Checked) АктивностьРасписанияВКрайниеДни |= 0x00004;
            if (cBГр29.Checked) АктивностьРасписанияВКрайниеДни |= 0x00008;
            if (cBГр30.Checked) АктивностьРасписанияВКрайниеДни |= 0x00010;
            if (cBГр31.Checked) АктивностьРасписанияВКрайниеДни |= 0x00020;
            if (cBГр1.Checked) АктивностьРасписанияВКрайниеДни |= 0x00040;
            if (cBГр2.Checked) АктивностьРасписанияВКрайниеДни |= 0x00080;
            if (cBГр3.Checked) АктивностьРасписанияВКрайниеДни |= 0x00100;
            if (cBГр4.Checked) АктивностьРасписанияВКрайниеДни |= 0x00200;
            if (cBГр5.Checked) АктивностьРасписанияВКрайниеДни |= 0x00400;
            if (cBГр6.Checked) АктивностьРасписанияВКрайниеДни |= 0x00800;
            if (cBГр7.Checked) АктивностьРасписанияВКрайниеДни |= 0x01000;
            if (cBГр8.Checked) АктивностьРасписанияВКрайниеДни |= 0x02000;
            if (cBГр9.Checked) АктивностьРасписанияВКрайниеДни |= 0x04000;
            if (cBГр10.Checked) АктивностьРасписанияВКрайниеДни |= 0x08000;
            if (cBГр11.Checked) АктивностьРасписанияВКрайниеДни |= 0x10000;
            if (cBГр12.Checked) АктивностьРасписанияВКрайниеДни |= 0x20000;
            РасписаниеПоезда.УстановитьАктивностьПоездаВКрайниеДни(АктивностьРасписанияВКрайниеДни);

            if (radioButton6.Checked)
                ЗадатьАктивностьРасписанияПоДням();
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
