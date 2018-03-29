using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.SoundRecordServices
{
    public class SoundReсordWorkerService : ISoundReсordWorkerService
    {
        /// <summary>
        /// Вычислить время сработки шаблона с учетом смещения.
        /// </summary>
        public DateTime CalcTimeWithShift(ref SoundRecord rec, ActionTrainDynamic actionTrainDyn)
        {
            var timeSift = actionTrainDyn.ActionTrain.Time.IsDeltaTimes
                ? actionTrainDyn.ActionTrain.Time.DeltaTimes[0]
                : actionTrainDyn.ActionTrain.Time.CycleTime.Value;


            var manualTemplate = actionTrainDyn.ActionTrain.Name.StartsWith("@");
            var arrivalTime = (rec.ФиксированноеВремяПрибытия == null || !manualTemplate) ? rec.ВремяПрибытия : rec.ФиксированноеВремяПрибытия.Value;
            var departTime = (rec.ФиксированноеВремяОтправления == null || !manualTemplate) ? rec.ВремяОтправления : rec.ФиксированноеВремяОтправления.Value;

            var activationTime = actionTrainDyn.ActionTrain.ActionType == ActionType.Arrival
                ? arrivalTime.AddMinutes(timeSift)
                : departTime.AddMinutes(timeSift);

            return activationTime;
        }



        public TextFragment ОтобразитьШаблонОповещенияНаRichTb2(ref SoundRecord rec, ActionTrain actionTrain)
        {
            string[] названиеФайловНумерацииПутей = { "", "Нумерация поезда с головы состава", "Нумерация поезда с хвоста состава" };
            Color color= Color.Red;
            string str = String.Empty;
            TextFragment txtFrag = new TextFragment();

            var langRutemplate= actionTrain.Langs.FirstOrDefault(lang => lang.Name == "Rus");
            if (langRutemplate == null)
                return null;

            var fullTemplate= langRutemplate.TemplateSoundStart.Concat(langRutemplate.TemplateSoundBody).Concat(langRutemplate.TemplateSoundEnd).ToList();
            foreach (string шаблон in fullTemplate)
            {
                string текстПодстановки = String.Empty;
                switch (шаблон)
                {
                    case "НА НОМЕР ПУТЬ":
                    case "НА НОМЕРом ПУТИ":
                    case "С НОМЕРого ПУТИ":
                        var pathway = rec.Pathway;
                        if (pathway == null)
                            break;
                        if (шаблон == "НА НОМЕР ПУТЬ") текстПодстановки = pathway.НаНомерПуть;
                        if (шаблон == "НА НОМЕРом ПУТИ") текстПодстановки = pathway.НаНомерОмПути;
                        if (шаблон == "С НОМЕРого ПУТИ") текстПодстановки = pathway.СНомерОгоПути;

                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ПУТЬ ДОПОЛНЕНИЕ":
                        pathway= rec.Pathway;
                        if (pathway == null)
                            break;
                        текстПодстановки= pathway.Addition ?? string.Empty;
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "НОМЕР ПОЕЗДА":
                        текстПодстановки = rec.НомерПоезда ?? string.Empty;
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "НОМЕР ПОЕЗДА ТРАНЗИТ ОТПР":
                        текстПодстановки = rec.НомерПоезда2 ?? string.Empty;
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ДОПОЛНЕНИЕ":
                        if (string.IsNullOrEmpty(rec.Дополнение))
                            break;
                        текстПодстановки = rec.Дополнение ?? string.Empty;
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "СТ.ПРИБЫТИЯ":
                        текстПодстановки = rec.СтанцияНазначения ?? string.Empty;
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "СТ.ОТПРАВЛЕНИЯ":
                        текстПодстановки = rec.СтанцияОтправления ?? string.Empty;
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ВРЕМЯ ПРИБЫТИЯ":
                        str = "Время прибытия: ";
                        txtFrag.AddWord(str, Color.Black);
                        текстПодстановки = rec.ВремяПрибытия.ToString("HH:mm");
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ВРЕМЯ ПРИБЫТИЯ UTC":
                        str = "Время прибытия UTC: ";
                        txtFrag.AddWord(str, Color.Black);
                        var времяUtc= rec.ВремяПрибытия.AddMinutes(0); //Program.Настройки.UTC
                        текстПодстановки = времяUtc.ToString("HH:mm");
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ВРЕМЯ СТОЯНКИ":
                        str = "Стоянка: ";
                        txtFrag.AddWord(str, Color.Black);
                        if (rec.ВремяСтоянки.HasValue)
                        {
                            текстПодстановки= (rec.ВремяСтоянки.Value.Hours.ToString("D2") + ":" + rec.ВремяСтоянки.Value.Minutes.ToString("D2"));
                        }
                        else
                        if (rec.БитыАктивностиПолей == 31)
                        {
                            текстПодстановки= "Время стоянки будет измененно";
                        }
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                        str = "Время отправления: ";
                        txtFrag.AddWord(str, Color.Black);
                        текстПодстановки = rec.ВремяОтправления.ToString("HH:mm");
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ВРЕМЯ ОТПРАВЛЕНИЯ UTC":
                        str = "Время отправления UTC: ";
                        txtFrag.AddWord(str, Color.Black);
                        времяUtc = rec.ВремяОтправления.AddMinutes(0); //Program.Настройки.UTC
                        текстПодстановки = времяUtc.ToString("HH:mm");
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ВРЕМЯ ЗАДЕРЖКИ":
                        str = "Время задержки: ";
                        txtFrag.AddWord(str, Color.Black);
                        текстПодстановки = (rec.ВремяЗадержки == null) ? "00:00" : rec.ВремяЗадержки.Value.ToString("HH:mm");
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "ОЖИДАЕМОЕ ВРЕМЯ":
                        str = "Ожидаемое время: ";
                        txtFrag.AddWord(str, Color.Black);
                        текстПодстановки = rec.ОжидаемоеВремя.ToString("HH:mm");
                        txtFrag.AddWord(текстПодстановки, color);
                        break;

                    case "НУМЕРАЦИЯ СОСТАВА":
                        if ((rec.НумерацияПоезда > 0) && (rec.НумерацияПоезда <= 2))
                        {
                            текстПодстановки = названиеФайловНумерацииПутей[rec.НумерацияПоезда];
                            txtFrag.AddWord(текстПодстановки, color);
                        }
                        break;

                    case "СТАНЦИИ":
                        if (rec.ТипПоезда.CategoryTrain == CategoryTrain.Suburb)
                        {
                            if (string.IsNullOrEmpty(rec.Примечание))
                               break;

                            str = "Электропоезд движется ";
                            txtFrag.AddWord(str, Color.Black);
                            текстПодстановки = rec.Примечание;
                            txtFrag.AddWord(текстПодстановки, color);
                        }
                        break;

                    default:
                        txtFrag.AddWord(шаблон, Color.Black);
                        break;
                }
            }
            return txtFrag;
        }
    }

    /// <summary>
    /// хранит строку текста в которой выделенны цветом разные слова.
    /// </summary>
    public class TextFragment
    {
        public StringBuilder StringBuilder { get; } = new StringBuilder();
        public List<FragmentOption> FragmentOptions { get; set; } = new List<FragmentOption>();


        public void AddWord(string text, Color color)
        {
            FragmentOptions.Add(new FragmentOption
            {
                StartIndex= StringBuilder.Length,
                Lenght= text.Length,
                Color= color
            });
            StringBuilder.Append(text + " ");
        }
    }


    public class FragmentOption
    {
        public int StartIndex { get; set; }
        public int Lenght { get; set; }
        public Color Color { get; set; }
    }
}