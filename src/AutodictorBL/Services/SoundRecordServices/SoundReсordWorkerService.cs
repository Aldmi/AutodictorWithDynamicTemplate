using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DAL.Abstract.Entitys;
using Force.DeepCloner;

namespace AutodictorBL.Services.SoundRecordServices
{
    public class SoundReсordWorkerService : ISoundReсordWorkerService
    {
        /// <summary>
        /// Вычислить время сработки шаблона с учетом смещения.
        /// Для нештатного шаблона смещение применяеться на "время фикс. нешатаки"
        /// Для обычного шаблона смещение применяется "Время относительно которого заданн шаблон"
        /// </summary>
        public DateTime CalcTimeWithShift(ref SoundRecord rec, ActionTrainDynamic actionTrainDyn)
        {
            var timeSift = actionTrainDyn.ActionTrain.Time.IsDeltaTimes
                ? actionTrainDyn.ActionTrain.Time.DeltaTimes[0]
                : actionTrainDyn.ActionTrain.Time.CycleTime.Value;

            DateTime activationTime;
            if (actionTrainDyn.ActionTrain.Emergency == Emergency.None)
            {
                var manualTemplate= actionTrainDyn.ActionTrain.Name.StartsWith("@");
                var arrivalTime= (rec.ФиксированноеВремяПрибытия == null || !manualTemplate) ? rec.ВремяПрибытия : rec.ФиксированноеВремяПрибытия.Value;
                var departTime= (rec.ФиксированноеВремяОтправления == null || !manualTemplate) ? rec.ВремяОтправления : rec.ФиксированноеВремяОтправления.Value;

                activationTime= (actionTrainDyn.ActionTrain.ActionType == ActionType.Arrival)
                    ? arrivalTime.AddMinutes(timeSift)
                    : departTime.AddMinutes(timeSift);
            }
            else
            {
                activationTime = actionTrainDyn.ActionTrain.FixedTimeEmergencyEvent.AddMinutes(timeSift);
            }
            return activationTime;
        }


        /// <summary>
        /// Возвращает список дианмических шаблонов на базе ActionTrain.
        /// Учитывается Время активации шаблона.
        /// Для Нештатных шаблонов "время фикс. нешатаки" ... "часы до конца суток +3 часа" с шагом указанном в шаблоне
        /// Для Обычных шаблонов "Время относительно которого заданн шаблон" ... "часы до конца суток +3 часа" с шагом указанном в шаблоне
        /// </summary>
        public List<ActionTrainDynamic> СreateActionTrainDynamic(SoundRecord record, IEnumerable<ActionTrain> actions, double lowDelta4Cycle=-60, double hightDelta4Cycle=60)
        {
            var dynamiсLists = new List<ActionTrainDynamic>();
            var idActDyn = 1;

            foreach (var action in actions)
            {
                if (action.Time.IsDeltaTimes)      //Указанны временные смещения
                {
                    foreach (var time in action.Time.DeltaTimes) //копируем шаблон для каждого временного смещения
                    {
                        var newActionTrain = action.DeepClone();
                        newActionTrain.Time.DeltaTimes = new List<int> { time };
                        var actDyn = new ActionTrainDynamic
                        {
                            Id = idActDyn++,
                            SoundRecordId = record.Id,
                            Activity = true,
                            PriorityMain = Priority.Midlle,
                            SoundRecordStatus = SoundRecordStatus.ОжиданиеВоспроизведения,
                            ActionTrain = newActionTrain
                        };
                        dynamiсLists.Add(actDyn);
                    }
                }
                else                                   //Указан циклический повтор
                {
                    DateTime eventTime = (action.ActionType == ActionType.Arrival) ? record.ВремяПрибытия : record.ВремяОтправления;
                    var startDate4Cycle = eventTime.AddMinutes(lowDelta4Cycle);
                    var endDate4Cycle = eventTime.AddMinutes(hightDelta4Cycle);
                    var interval = action.Time.CycleTime.Value;
                    for (var date = startDate4Cycle; date < endDate4Cycle; date += new TimeSpan(0, interval, 0))
                    {
                        var time = (((eventTime - date).Hours * 60) + (eventTime - date).Minutes) * -1;
                        var newActionTrain= action.DeepClone();
                        newActionTrain.Time.DeltaTimes = new List<int> { time };
                        var actDyn = new ActionTrainDynamic
                        {
                            Id = idActDyn++,
                            SoundRecordId = record.Id,
                            Activity = true,
                            PriorityMain = Priority.Midlle,
                            SoundRecordStatus = SoundRecordStatus.ОжиданиеВоспроизведения,
                            ActionTrain = newActionTrain
                        };
                        dynamiсLists.Add(actDyn);
                    }
                }
            }
            return dynamiсLists;
        }



        public TextFragment CalcTextFragment(ref SoundRecord rec, ActionTrain actionTrain)
        {
            string[] названиеФайловНумерацииПутей = { "", "Нумерация поезда с головы состава", "Нумерация поезда с хвоста состава" };
            Color color= Color.Red;
            string str = string.Empty;
            TextFragment txtFrag = new TextFragment();

            var langRutemplate= actionTrain.Langs.FirstOrDefault(lang => lang.Name == "Rus");
            if (langRutemplate == null)
                return null;

            var fullTemplate= langRutemplate.TemplateSoundStart.Concat(langRutemplate.TemplateSoundBody).Concat(langRutemplate.TemplateSoundEnd).ToList();
            foreach (string шаблон in fullTemplate)
            {
                string текстПодстановки = string.Empty;
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
                            if(rec.Route == null || rec.Route.RouteType == RouteType.None)
                               break;

                            str = "Электропоезд движется ";
                            txtFrag.AddWord(str, Color.Black);
                            текстПодстановки = rec.Route.ToString();
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


        /// <summary>
        /// Возвращает список элементов шаблона
        /// </summary>
        /// <param name="actionTrain"></param>
        /// <param name="allowedLang">список разрещенных языков</param>
        public List<TemplateItem> CalcTemplateItems(ActionTrain actionTrain, List<string> allowedLang)
        {
            var templateItems= new List<TemplateItem>();
            foreach (var lang in actionTrain.Langs)
            {
                if(!allowedLang.Contains(lang.Name))
                continue;

                if (!lang.IsEnable)
                continue;

                templateItems.AddRange(lang.TemplateSoundStart.Select(tmp=> new TemplateItem {Template = tmp, NameLang = lang.Name}));
                for (int i = 0; i < lang.RepeatSoundBody; i++)
                {
                    templateItems.AddRange(lang.TemplateSoundBody.Select(tmp=> new TemplateItem { Template = tmp, NameLang = lang.Name }));
                }
                templateItems.AddRange(lang.TemplateSoundEnd.Select(tmp=> new TemplateItem { Template = tmp, NameLang = lang.Name }));
            }

            return templateItems;
        }


        /// <summary>
        /// Возвращает список элементов шаблона для указанного языка
        /// </summary>
        /// <param name="lang">язык</param>
        /// <returns></returns>
        public List<TemplateItem> CalcTemplateItemsByLang(Lang lang)
        {
            if (!lang.IsEnable)
                return null;

            var templateItems = new List<TemplateItem>();
            if (lang.TemplateSoundStart != null && lang.TemplateSoundStart.Any())
            {
                templateItems.AddRange(lang.TemplateSoundStart.Select(tmp => new TemplateItem { Template = tmp, NameLang = lang.Name }));
            }
            if (lang.TemplateSoundBody != null && lang.TemplateSoundBody.Any())
            {
                for (int i = 0; i < lang.RepeatSoundBody; i++)
                {
                    templateItems.AddRange(lang.TemplateSoundBody.Select(tmp => new TemplateItem { Template = tmp, NameLang = lang.Name }));
                }
            }
            if (lang.TemplateSoundEnd != null && lang.TemplateSoundEnd.Any())
            {
                templateItems.AddRange(lang.TemplateSoundEnd.Select(tmp => new TemplateItem { Template = tmp, NameLang = lang.Name }));
            }

            return templateItems;
        }
    }


    /// <summary>
    /// хранит строку текста в которой выделенны цветом разные слова.
    /// </summary>
    public class TextFragment
    {
        public StringBuilder StringBuilder { get; } = new StringBuilder();
        public List<FragmentOption> FragmentOptions { get; set; } = new List<FragmentOption>();


        public void AddWord(string text, Color color = default(Color))
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


    /// <summary>
    /// Элемент шаблона с указанием языка
    /// </summary>
    public class TemplateItem
    {
        public string Template { get; set; }
        public string NameLang { get; set; }
    }
}