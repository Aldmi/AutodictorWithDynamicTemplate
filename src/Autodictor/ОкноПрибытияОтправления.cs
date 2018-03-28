using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace MainExample
{
    public partial class ОкноПрибытияОтправления : Form
    {
        public int СчетчикСтроки = 0;

        public Font drawFont = new Font("Arial", 8, FontStyle.Bold);
        public SolidBrush drawBrushWhite = new SolidBrush(Color.White);
        public SolidBrush drawBrushRed = new SolidBrush(Color.DarkRed);
        public SolidBrush drawBrushBlue = new SolidBrush(Color.Black);
        public bool ОбновитьЧасы = false;
        public bool ОбноыитьРасписание = false;
        public bool ОбновитьСтрокуПрокрутки = false;
        private DateTime ВремяПоследнегоОбновления = DateTime.Now;

        const int КоличествоСтрокДляПоездовДальнегоСледования = 6;
        const int КоличествоСтрокДляЭлектропоездов = 4;


        public ОкноПрибытияОтправления()
        {
            InitializeComponent();
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Minute != ВремяПоследнегоОбновления.Minute)
            {
                ВремяПоследнегоОбновления = DateTime.Now;
                ОбновитьЧасы = true;
                ОбноыитьРасписание = true;
            }


            Graphics g = Graphics.FromHwnd(panel1.Handle);

            if (ОбноыитьРасписание)
            {
                g.FillRectangle(drawBrushBlue, new Rectangle(0, 40, 390, 250));
                g.FillRectangle(drawBrushRed, new Rectangle(0, 0, 390, 40));
                g.FillRectangle(drawBrushRed, new Rectangle(0, 150, 390, 40));

                g.DrawString("Прибытие отправление поездов                   Московское время", drawFont, drawBrushWhite, new Point(6, 6));
                g.DrawString("№ Поезда| Маршрут следования             Приб. |Отпр. | Путь | Стоянка", drawFont, drawBrushWhite, new Point(6, 25));

                g.DrawString("Расписание движения эл. поездов                   Местное время:", drawFont, drawBrushWhite, new Point(6, 156));
                g.DrawString("№ Поезда| Маршрут следования             Приб. |Отпр. | Путь | Стоянка", drawFont, drawBrushWhite, new Point(6, 175));

                List<string> Поезда = new List<string>();
                for (int i = 0, j = 0, k = 0; i < MainWindowForm.SoundRecords.Count; i++)
                {
                    var Данные = MainWindowForm.SoundRecords.ElementAt(i);
                    //if (((Данные.Value.ТипСообщения == SoundRecordType.ДвижениеПоезда) || (Данные.Value.ТипСообщения == SoundRecordType.ДвижениеПоездаНеПодтвержденное)) && (j < КоличествоСтрокДляПоездовДальнегоСледования) && (Данные.Value.ТипПоезда == ТипПоезда.Пассажирский))
                    {
                        if (Поезда.Contains(Данные.Value.НомерПоезда) == true)
                            continue;

                        if ((Данные.Value.БитыАктивностиПолей & 0x10) != 0x00)
                        {
                            if (DateTime.Now > Данные.Value.ВремяОтправления)
                                continue;
                        }
                        else
                        {
                            if ((Данные.Value.БитыАктивностиПолей & 0x04) != 0x00)
                                if (DateTime.Now > Данные.Value.ВремяПрибытия)
                                    continue;
                        }

                        string[] Массив = Данные.Value.НомерПоезда.Split(':');
                        g.DrawString(Массив[0], drawFont, drawBrushWhite, new Point(6, 45 + j * 18));
                        g.DrawString(Данные.Value.НазваниеПоезда, drawFont, drawBrushWhite, new Point(40, 45 + j * 18));
                        if ((Данные.Value.БитыАктивностиПолей & 0x04) != 0x00) g.DrawString(Данные.Value.ВремяПрибытия.ToString("HH:mm"), drawFont, drawBrushWhite, new Point(228, 45 + j * 18));
                        if ((Данные.Value.БитыАктивностиПолей & 0x10) != 0x00) g.DrawString(Данные.Value.ВремяОтправления.ToString("HH:mm"), drawFont, drawBrushWhite, new Point(268, 45 + j * 18));
                        if (Данные.Value.НомерПути != "") g.DrawString(Данные.Value.НомерПути.ToString(), drawFont, drawBrushWhite, new Point(308, 45 + j * 18));
                        if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x14) g.DrawString(Данные.Value.ВремяСтоянки.ToString(), drawFont, drawBrushWhite, new Point(353, 45 + j * 18));
                        Поезда.Add(Данные.Value.НомерПоезда);
                        j++;
                    }

                    //if (((Данные.Value.ТипСообщения == SoundRecordType.ДвижениеПоезда) || (Данные.Value.ТипСообщения == SoundRecordType.ДвижениеПоездаНеПодтвержденное)) && (k < КоличествоСтрокДляЭлектропоездов) && (Данные.Value.ТипПоезда == ТипПоезда.Пригородный))
                    {
                        if (Поезда.Contains(Данные.Value.НомерПоезда) == true)
                            continue;

                        if ((Данные.Value.БитыАктивностиПолей & 0x10) != 0x00)
                        {
                            if (DateTime.Now > Данные.Value.ВремяОтправления)
                                continue;
                        }
                        else
                        {
                            if ((Данные.Value.БитыАктивностиПолей & 0x04) != 0x00)
                                if (DateTime.Now > Данные.Value.ВремяПрибытия)
                                    continue;
                        }

                        string[] Массив = Данные.Value.НомерПоезда.Split(':');
                        g.DrawString(Массив[0], drawFont, drawBrushWhite, new Point(6, 190 + k * 18));
                        g.DrawString(Данные.Value.НазваниеПоезда, drawFont, drawBrushWhite, new Point(40, 190 + k * 18));
                        if ((Данные.Value.БитыАктивностиПолей & 0x04) != 0x00) g.DrawString(Данные.Value.ВремяПрибытия.AddHours(4).ToString("HH:mm"), drawFont, drawBrushWhite, new Point(228, 190 + k * 18));
                        if ((Данные.Value.БитыАктивностиПолей & 0x10) != 0x00) g.DrawString(Данные.Value.ВремяОтправления.AddHours(4).ToString("HH:mm"), drawFont, drawBrushWhite, new Point(268, 190 + k * 18));
                        if (Данные.Value.НомерПути != "") g.DrawString(Данные.Value.НомерПути.ToString(), drawFont, drawBrushWhite, new Point(308, 190 + k * 18));
                        if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x14) g.DrawString(Данные.Value.ВремяСтоянки.ToString(), drawFont, drawBrushWhite, new Point(353, 190 + k * 18));
                        Поезда.Add(Данные.Value.НомерПоезда);
                        k++;
                    }

                    ListViewItem lvi = new ListViewItem(new string[] { Данные.Value.Id.ToString(), Данные.Value.Время.ToString("HH:mm:ss"), "00:00", Данные.Value.НомерПоезда + Данные.Value.Описание });
                }


                ОбноыитьРасписание = false;
            }

            if (ОбновитьЧасы)
            {
                g.FillRectangle(drawBrushRed, new Rectangle(353, 5, 34, 18));
                g.DrawString(DateTime.Now.ToString("HH:mm"), drawFont, drawBrushWhite, new Point(353, 6));

                g.FillRectangle(drawBrushRed, new Rectangle(353, 155, 34, 18));
                g.DrawString(DateTime.Now.AddHours(4).ToString("HH:mm"), drawFont, drawBrushWhite, new Point(353, 156));

                ОбновитьЧасы = false;
            }
        }

        private void ОкноРасписания_Load(object sender, EventArgs e)
        {
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
            panel1.Invalidate();
        }

        private void ОкноРасписания_Activated(object sender, EventArgs e)
        {
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
            panel1.Invalidate();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
            panel1.Invalidate();
        }
    }
}
