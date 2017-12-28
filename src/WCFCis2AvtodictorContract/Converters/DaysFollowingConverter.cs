using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WCFCis2AvtodictorContract.Converters;

namespace MainExample.Converters
{
    /// <summary>
    /// указанные дни в строке спецификации
    /// в формате пар ключ-значние, где
    /// ключ- номер месяца
    /// значение- список дней
    /// </summary>
    public struct DaysSpecification
    {
        public Dictionary<byte, List<byte>> IncludingDays;        // включая дни
        public Dictionary<byte, List<byte>> ExcludingDays;        // исключая дни
    }





    public class DaysFollowingConverter
    {
        public IEnumerable<string> DaysFollowingCis { get; } //входная коллекция
        private List<string> DaysFollowingAutodictor { get; } = new List<string>(); //выходная коллекция

        public Dictionary<byte, string> MathingNameMonth { get; } = new Dictionary<byte, string>
        {
            {1, "Январь"},
            {2, "Февраль"},
            {3, "Март"},
            {4, "Апрель"},
            {5, "Май"},
            {6, "Июнь"},
            {7, "Июль"},
            {8, "Август"},
            {9, "Сентябрь"},
            {10, "Октябрь"},
            {11, "Ноябрь"},
            {12, "Декабрь"},
        };

        public Dictionary<string, DayOfWeek> MathingDayWeek { get; } = new Dictionary<string, DayOfWeek>
        {
            {"ПН", DayOfWeek.Monday},
            {"ВТ", DayOfWeek.Tuesday},
            {"СР", DayOfWeek.Wednesday},
            {"ЧТ", DayOfWeek.Thursday},
            {"ПТ", DayOfWeek.Friday},
            {"СБ", DayOfWeek.Saturday},
            {"ВС", DayOfWeek.Sunday}
        };


        public Dictionary<DayOfWeek, byte> MathingDayWeekBinary { get; } = new Dictionary<DayOfWeek, byte>
        {
            {DayOfWeek.Monday, 0x01},
            {DayOfWeek.Tuesday, 0x02},
            {DayOfWeek.Wednesday, 0x04},
            {DayOfWeek.Thursday, 0x08},
            {DayOfWeek.Friday, 0x10},
            {DayOfWeek.Saturday, 0x20},
            {DayOfWeek.Sunday, 0x40}
        };




        public DaysFollowingConverter(IEnumerable<string> daysFollowingCis)
        {
            DaysFollowingCis = daysFollowingCis;
        }




        public async Task<IList<string>> Convert() //TODO: DaysFollowingCis передавать напрямую
        {
            foreach (var days in DaysFollowingCis)
            {
                DaysFollowingAutodictor.Add(await ConvertDays(days));
            }

            //DRBUG------------------------------------------------
            //var inStr =
            //    "Тип: \"ОН\"   Дни: \"Включая: 26.12, 22.12, 03.12, 29.12, 02.01, 04.01, 05.01, 06.05-20.06\"";


            //"Тип: \"ОН\"   Дни: \"Включая: 26.12, 22.12, 03.12, 29.12, 02.01, 04.01, 05.01, 06.05-20.06\"";
            //"Тип: \"Еж.\"   Дни: \"\"";
            //"Тип: \"Еж.\"   Дни: \"Кроме: 05.03, 03.08-06.08, 09.03\"";
            //"Тип: \"НЕЧЕТН\"   Дни: \"Кроме: 05.03, 03.08-20.08, 09.03, 27.12\"";
            // "Тип: \"В\"   Дни: \"Включая: 05.03, 09.03, 27.12, 21.03-24.03\"";
            //"Тип: \"Р\"   Дни: \"Кроме: 05.03, 09.03, 27.12, 21.03-24.03\"";
            // "Тип: \"кр.ПН СР ЧТ\"   Дни: \"Кроме: 26.12, 29.12, 02.01, 04.01, 05.01; Включая: 07.01\"";
            // "Тип: \"ПН СР ЧТ\"   Дни: \"Кроме: 26.12, 29.12, 02.01, 04.01, 05.01; Включая: 07.01\"";



            //DaysFollowingAutodictor.Add(await ConvertDays(inStr));
            //DRBUG------------------------------------------------

            return DaysFollowingAutodictor;
        }



        private async Task<string> ConvertDays(string days)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                //парсим информацию между кавычками
                Regex regex = new Regex("\"[^\"]*\"", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(days);

                string type = null;
                string specification = null;
                if (matches.Count == 2)
                {
                    type = matches[0].Value.Trim(new[] { '"' });
                    specification = matches[1].Value.Trim(new[] { '"' });
                }
                else
                {
                    return "НЕ КОРРЕКТНОЕ ПРЕОБРАЗОВАНИЕ";
                }

                var daysSpecification = new DaysSpecification();

                if (!string.IsNullOrEmpty(specification))
                {
                    var indexInclude = specification.IndexOf("Включая:", StringComparison.Ordinal);
                    var indexExclude = specification.IndexOf("Кроме:", StringComparison.Ordinal);

                    string strInclude = null;
                    string strExclude = null;
                    if (indexInclude >= 0 && indexExclude >= 0)
                    {
                        //вначале строки секция Кроме:.
                        if (indexExclude < indexInclude)
                        {
                            var indexSemicolon = specification.IndexOf(";", StringComparison.Ordinal);
                            if (indexSemicolon >= 0)
                            {
                                strExclude = specification.Substring((indexExclude + "Кроме:".Length),
                                    (indexSemicolon - "Кроме:".Length));
                                strInclude = specification.Substring((indexInclude + "Включая:".Length));
                            }
                        }

                        //вначале строки секция Включая:.
                        if (indexInclude < indexExclude)
                        {
                            var indexSemicolon = specification.IndexOf(";", StringComparison.Ordinal);
                            if (indexSemicolon >= 0)
                            {
                                strInclude = specification.Substring((indexInclude + "Включая:".Length),
                                    (indexSemicolon - "Включая:".Length));
                                strExclude = specification.Substring((indexExclude + "Кроме:".Length));
                            }
                        }
                    }
                    else if (indexInclude >= 0)
                    {
                        strInclude = specification.Substring(indexInclude + "Включая:".Length);
                    }
                    else if (indexExclude >= 0)
                    {
                        strExclude = specification.Substring(indexExclude + "Кроме:".Length);
                    }


                    if (!string.IsNullOrEmpty(strInclude))
                    {
                        daysSpecification.IncludingDays = new Dictionary<byte, List<byte>>();
                        var dateCollection = strInclude.Split(',');
                        foreach (var date in dateCollection)
                        {
                            //Выделим  диапазон дат
                            if (date.Contains("-"))
                            {
                                var range = date.Split('-');
                                if (range.Length == 2)
                                {
                                    RangeDaysConverter(range[0], range[1], daysSpecification.IncludingDays);
                                }

                                continue;
                            }

                            //Выделим конкретные даты
                            var monthDay = date.Split('.');
                            if (monthDay.Length == 2)
                            {
                                byte key = Byte.Parse(monthDay[1]);
                                byte value = Byte.Parse(monthDay[0]);

                                if (!daysSpecification.IncludingDays.ContainsKey(key))
                                    daysSpecification.IncludingDays[key] = new List<byte>();

                                daysSpecification.IncludingDays[key].Add(value);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(strExclude))
                    {
                        daysSpecification.ExcludingDays = new Dictionary<byte, List<byte>>();
                        var dateCollection = strExclude.Split(',');
                        foreach (var date in dateCollection)
                        {
                            //Выделим  диапазон дат
                            if (date.Contains("-"))
                            {
                                var range = date.Split('-');
                                if (range.Length == 2)
                                {
                                    RangeDaysConverter(range[0], range[1], daysSpecification.ExcludingDays);
                                }

                                continue;
                            }

                            var monthDay = date.Split('.');
                            if (monthDay.Length == 2)
                            {
                                byte key = Byte.Parse(monthDay[1]);
                                byte value = Byte.Parse(monthDay[0]);

                                if (!daysSpecification.ExcludingDays.ContainsKey(key))
                                    daysSpecification.ExcludingDays[key] = new List<byte>();

                                daysSpecification.ExcludingDays[key].Add(value);
                            }
                        }
                    }
                }


                //сортировка по возростанию месяцев (ключей) и дней (значений)
                if (daysSpecification.IncludingDays != null && daysSpecification.IncludingDays.Any())
                {
                    daysSpecification.IncludingDays =
                        daysSpecification.IncludingDays.OrderBy(key => key.Key)
                            .ToDictionary(k => k.Key, pair => pair.Value);
                    foreach (var key in daysSpecification.IncludingDays.Keys)
                    {
                        var daysCurrent = daysSpecification.IncludingDays[key];
                        var sortList = daysCurrent.OrderBy(b => b).ToList();
                        daysSpecification.IncludingDays[key].Clear();
                        daysSpecification.IncludingDays[key].AddRange(sortList);
                    }
                }

                if (daysSpecification.ExcludingDays != null && daysSpecification.ExcludingDays.Any())
                {
                    daysSpecification.ExcludingDays =
                        daysSpecification.ExcludingDays.OrderBy(key => key.Key)
                            .ToDictionary(k => k.Key, pair => pair.Value);
                    foreach (var key in daysSpecification.ExcludingDays.Keys)
                    {
                        var daysCurrent = daysSpecification.ExcludingDays[key];
                        var sortList = daysCurrent.OrderBy(b => b).ToList();
                        daysSpecification.ExcludingDays[key].Clear();
                        daysSpecification.ExcludingDays[key].AddRange(sortList);
                    }
                }


                //Вызов обработчика 
                string result = null;
                switch (type)
                {
                    case "ОН":
                        result = ONHandler(daysSpecification);
                        break;

                    case "ЧЕТН":
                        result = EvenHandler(daysSpecification);
                        break;

                    case "НЕЧЕТН":
                        result = OddHandler(daysSpecification); //???
                        break;

                    case "Еж.":
                        result = EveryDayHandler(daysSpecification); //???
                        break;

                    case "В": //???
                        result = WeekendHandler(daysSpecification);
                        break;

                    case "Р": //???
                        result = WeekdaysHandler(daysSpecification);
                        break;

                    default:
                        result = DayOfWeekHandler(daysSpecification, type); //обработчик указанных дней недели.
                        break;
                }


                return result;
            });
        }


        public void RangeDaysConverter(string startMonthDay, string endMonthDay, Dictionary<byte, List<byte>> days)
        {
            var startMd = startMonthDay.Split('.');
            var endMd = endMonthDay.Split('.');

            if (startMd.Length != 2 || endMd.Length != 2)
                return;

            if (days == null)
                return;

            var year = DateTime.Now.Year;
            var startDay = new DateTime(year, Byte.Parse(startMd[1]), Byte.Parse(startMd[0]));
            var endDay = new DateTime(year, Byte.Parse(endMd[1]), Byte.Parse(endMd[0])).AddDays(1);
            for (var date = startDay; date != endDay; date = date.AddDays(1))
            {
                var key = (byte)date.Month;
                var value = (byte)date.Day;

                if (!days.ContainsKey(key))
                    days[key] = new List<byte>();

                days[key].Add(value);
            }
        }




        /// <summary>
        /// Обработчик дней особого назначения.
        /// В секции "включая" находятся выбранные дни в году.
        /// </summary>
        private string ONHandler(DaysSpecification daysSpecification)
        {
            if ((daysSpecification.IncludingDays == null) || (!daysSpecification.IncludingDays.Any()))
            {
                return "НЕ КОРРЕКТНОЕ ПРЕОБРАЗОВАНИЕ";
            }

            ПланРасписанияПоезда текущийПланРасписанияПоезда =
                ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(string.Empty);
            текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Выборочно);

            foreach (var key in daysSpecification.IncludingDays.Keys)
            {
                foreach (var day in daysSpecification.IncludingDays[key])
                {
                    текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(key - 1), (byte)(day - 1), true);
                }
            }

            return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
        }



        /// <summary>
        /// Обработчик четных дней
        /// </summary>
        private string EvenHandler(DaysSpecification daysSpecification)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда =
                ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(string.Empty);

            if ((daysSpecification.ExcludingDays) == null || !(daysSpecification.ExcludingDays.Any()))
            {
                текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.ПоЧетным);
                return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
            }


            текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Выборочно);
            var year = DateTime.Now.Year;
            var month = 1;
            var startDay = new DateTime(year, month, 1);
            var endDay = startDay.AddMonths(12);

            for (var date = startDay; date != endDay; date = date.AddDays(1))
            {
                if ((date.Day % 2) != 0)
                {
                    continue;
                }

                if (daysSpecification.ExcludingDays.ContainsKey((byte)date.Month))
                    if (daysSpecification.ExcludingDays[(byte)date.Month].Contains((byte)date.Day))
                    {
                        continue;
                    }

                текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1), (byte)(date.Day - 1),
                    true);
            }

            return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
        }




        /// <summary>
        /// Обработчик НЕчетных дней
        /// </summary>
        private string OddHandler(DaysSpecification daysSpecification)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда =
                ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(string.Empty);

            if ((daysSpecification.ExcludingDays) == null || !(daysSpecification.ExcludingDays.Any()))
            {
                текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.ПоНечетным);
                return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
            }


            текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Выборочно);
            var year = DateTime.Now.Year;
            var month = 1;
            var startDay = new DateTime(year, month, 1);
            var endDay = startDay.AddMonths(12);

            for (var date = startDay; date != endDay; date = date.AddDays(1))
            {
                if ((date.Day % 2) == 0)
                {
                    continue;
                }

                if (daysSpecification.ExcludingDays.ContainsKey((byte)date.Month))
                    if (daysSpecification.ExcludingDays[(byte)date.Month].Contains((byte)date.Day))
                    {
                        continue;
                    }

                текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1), (byte)(date.Day - 1),
                    true);
            }

            return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
        }




        /// <summary>
        /// Обработчик всех дней.
        /// В секции "исключая" находятся дни которые нужно исключить.
        /// </summary>
        private string EveryDayHandler(DaysSpecification daysSpecification)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда =
                ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(string.Empty);

            if ((daysSpecification.ExcludingDays) == null || !(daysSpecification.ExcludingDays.Any()))
            {
                текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Ежедневно);
                return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
            }


            текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Выборочно);
            var year = DateTime.Now.Year;
            var month = 1;
            var startDay = new DateTime(year, month, 1);
            var endDay = startDay.AddMonths(12);

            for (var date = startDay; date != endDay; date = date.AddDays(1))
            {
                if (daysSpecification.ExcludingDays.ContainsKey((byte)date.Month))
                    if (daysSpecification.ExcludingDays[(byte)date.Month].Contains((byte)date.Day))
                    {
                        continue;
                    }

                текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1), (byte)(date.Day - 1),
                    true);
            }


            return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
        }



        /// <summary>
        /// Обработчик выходных дней.
        /// В секции "включая" находятся дни которые нужно включить к вых. дням.
        /// </summary>
        private string WeekendHandler(DaysSpecification daysSpecification)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда =
                ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(string.Empty);

            if ((daysSpecification.IncludingDays) == null || !(daysSpecification.IncludingDays.Any()))
            {
                текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.ПоДням);
                текущийПланРасписанияПоезда.ЗадатьАктивностьПоДнямНедели(
                    (ushort)(MathingDayWeekBinary[DayOfWeek.Saturday] | MathingDayWeekBinary[DayOfWeek.Sunday]));
                return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
            }


            текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Выборочно);
            var year = DateTime.Now.Year;
            var month = 1;
            var startDay = new DateTime(year, month, 1);
            var endDay = startDay.AddMonths(12);

            for (var date = startDay; date != endDay; date = date.AddDays(1))
            {
                if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
                {
                    текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1),
                        (byte)(date.Day - 1), true);
                }
                else
                {
                    if (daysSpecification.IncludingDays.ContainsKey((byte)date.Month))
                        if (daysSpecification.IncludingDays[(byte)date.Month].Contains((byte)date.Day))
                        {
                            текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1),
                                (byte)(date.Day - 1), true);
                        }
                }
            }

            return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
        }



        /// <summary>
        /// Обработчик рабочих дней.
        /// В секции "исключая" находятся дни которые нужно исключить из раб. дней.
        /// </summary>
        private string WeekdaysHandler(DaysSpecification daysSpecification)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда =
                ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(string.Empty);

            if ((daysSpecification.ExcludingDays) == null || !(daysSpecification.ExcludingDays.Any()))
            {
                текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.ПоДням);
                текущийПланРасписанияПоезда.ЗадатьАктивностьПоДнямНедели(
                    (ushort)(MathingDayWeekBinary[DayOfWeek.Monday] |
                              MathingDayWeekBinary[DayOfWeek.Tuesday] |
                              MathingDayWeekBinary[DayOfWeek.Wednesday] |
                              MathingDayWeekBinary[DayOfWeek.Thursday] |
                              MathingDayWeekBinary[DayOfWeek.Friday]
                        ));
                return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
            }


            текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Выборочно);
            var year = DateTime.Now.Year;
            var month = 1;
            var startDay = new DateTime(year, month, 1);
            var endDay = startDay.AddMonths(12);

            for (var date = startDay; date != endDay; date = date.AddDays(1))
            {
                if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
                {
                    continue;
                }

                if (daysSpecification.ExcludingDays.ContainsKey((byte)date.Month))
                    if (daysSpecification.ExcludingDays[(byte)date.Month].Contains((byte)date.Day))
                    {
                        continue;
                    }

                текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1), (byte)(date.Day - 1),
                    true);

            }

            return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
        }



        /// <summary>
        /// Обработчик произвольных дней недели
        /// </summary>
        private string DayOfWeekHandler(DaysSpecification daysSpecification, string days)
        {
            var daysArr = days.Split(' ');
            bool exceptFlag = false;
            if (daysArr[0].Contains("кр."))
            {
                exceptFlag = true;
                daysArr[0] = daysArr[0].Remove(0, 3);
            }

            var englishDays =
                (from day in daysArr where MathingDayWeek.ContainsKey(day) select MathingDayWeek[day]).ToList();

            if (!englishDays.Any())
                return "Некорректное преобразование";

            ПланРасписанияПоезда текущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(string.Empty);

            //если нет секций "Включая" и "Исключая", то приводим к типу "ПоДням"
            if ((daysSpecification.ExcludingDays == null ) && (daysSpecification.IncludingDays == null))
            {
                ushort активностьПоДнямНедели = englishDays.Aggregate<DayOfWeek, ushort>(0x0000,
                    (current, engDay) => (ushort)(current | MathingDayWeekBinary[engDay]));
                текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.ПоДням);
                if (exceptFlag)
                {
                    текущийПланРасписанияПоезда.ЗадатьАктивностьПоДнямНедели((ushort)(активностьПоДнямНедели << 0x08));
                    return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
                }
                else
                {
                    текущийПланРасписанияПоезда.ЗадатьАктивностьПоДнямНедели(активностьПоДнямНедели);
                    return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
                }
            }



            текущийПланРасписанияПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.Выборочно);
            var year = DateTime.Now.Year;
            var month = 1;
            var startDay = new DateTime(year, month, 1);
            var endDay = startDay.AddMonths(12);

            for (var date = startDay; date != endDay; date = date.AddDays(1))
            {
                if ((daysSpecification.ExcludingDays) != null && (daysSpecification.ExcludingDays.Any()))
                {
                    if (daysSpecification.ExcludingDays.ContainsKey((byte)date.Month))
                        if (daysSpecification.ExcludingDays[(byte)date.Month].Contains((byte)date.Day))
                        {
                            continue;
                        }
                }

                if ((daysSpecification.IncludingDays) != null && (daysSpecification.IncludingDays.Any()))
                {
                    if (daysSpecification.IncludingDays.ContainsKey((byte)date.Month))
                        if (daysSpecification.IncludingDays[(byte)date.Month].Contains((byte)date.Day))
                        {
                            текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1),
                                (byte)(date.Day - 1), true);
                            continue;
                        }
                }

                if (exceptFlag)
                {
                    if (englishDays.Contains(date.DayOfWeek))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!englishDays.Contains(date.DayOfWeek))
                    {
                        continue;
                    }
                }

                текущийПланРасписанияПоезда.ЗадатьАктивностьДняДвижения((byte)(date.Month - 1), (byte)(date.Day - 1), true);

            }
            return текущийПланРасписанияПоезда.ПолучитьСтрокуРасписания();
        }
    }
}