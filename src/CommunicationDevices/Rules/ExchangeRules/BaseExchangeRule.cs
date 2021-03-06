﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Settings;


namespace CommunicationDevices.Rules.ExchangeRules
{
    public class MainRule
    {
        public List<BaseExchangeRule> ExchangeRules { get; set; }
        public ViewType ViewType { get; set; }
    }



    public class ViewType
    {
        public string Type { get; set; }

        public int? TableSize { get; set; }                   // Размер таблицы. выставляется если Type == Table
        public int? FirstTableElement { get; set; }          //Номер стартового элемента. выставляется если Type == Table
    }



    public class BaseExchangeRule
    {
        #region prop

        public string Format { get; set; }
        public Conditions Resolution { get; set; }

        public RequestRule RequestRule { get; set; }
        public ResponseRule ResponseRule { get; set; }
        public RepeatRule RepeatRule { get; set; }

        #endregion




        #region ctor

        public BaseExchangeRule(RequestRule requestRule, ResponseRule responseRule, RepeatRule repeatRule, string format, Conditions resolution)
        {
            RequestRule = requestRule;
            ResponseRule = responseRule;
            RepeatRule = repeatRule;
            Format = format;
            Resolution = resolution;
        }

        #endregion





        #region Methode

        /// <summary>
        /// Проверка условий разрешения выполнения правила.
        /// </summary>
        public bool CheckResolution(UniversalInputType inData)
        {
            if (Resolution == null)
                return true;

            return Resolution.CheckResolutions(inData);  //инверсия ограничения 
        }

        #endregion

    }





    public class RequestRule
    {
        public int? MaxLenght { get; set; }
        public string Body { get; set; }


        #region Method

        public virtual string GetFillBody(UniversalInputType uit, byte? currentRow)
        {
            var str = MakeIndependentInserts(uit, currentRow);
            str = MakeDependentInserts(str, uit);
            return str;
        }


        /// <summary>
        /// Первоначальная вставка независимых переменных
        /// </summary>
        private string MakeIndependentInserts(UniversalInputType uit, byte? currentRow)
        {
            if (Body.Contains("}"))                                                           //если указанны переменные подстановки
            {
                var subStr = Body.Split('}');
                StringBuilder resStr = new StringBuilder();
                int parseVal;
                foreach (var s in subStr)
                {
                    var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                    var mathStr = Regex.Match(replaseStr, @"{(.*)}").Groups[1].Value;
                    var subvar= mathStr.Split(':').First();

                    if (subvar == nameof(uit.AddressDevice))
                    { 
                        if (mathStr.Contains(":")) //если указанн формат числа
                        {
                            if (int.TryParse(uit.AddressDevice, out parseVal))
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.AddressDevice), "0"), parseVal);
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.AddressDevice), "0"), uit.AddressDevice);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == "TypeName")
                    {
                        var ruTypeTrain = uit.TypeTrain.NameRu;
                        var formatStr = string.Format(replaseStr.Replace("TypeName", "0"), ruTypeTrain);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == "TypeAlias")
                    {
                        var ruTypeTrain = uit.TypeTrain.NameRu;
                        var formatStr = string.Format(replaseStr.Replace("TypeAlias", "0"), ruTypeTrain);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == nameof(uit.NumberOfTrain))
                    {
                        if (mathStr.Contains(":")) //если указан формат числа
                        {
                            if (int.TryParse(uit.NumberOfTrain, out parseVal))
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), parseVal);
                                resStr.Append(formatStr);
                            }
                            else
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), " ");
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), string.IsNullOrEmpty(uit.NumberOfTrain) ? " " : uit.NumberOfTrain);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == nameof(uit.PathNumber))
                    {
                        if (mathStr.Contains(":")) //если указан формат числа
                        {
                            if (int.TryParse(uit.PathNumber, out parseVal))
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.PathNumber), "0"), parseVal);
                                resStr.Append(formatStr);
                            }
                            else
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), " ");
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.PathNumber), "0"), string.IsNullOrEmpty(uit.PathNumber) ? " " : uit.PathNumber);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == nameof(uit.Event))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Event), "0"), string.IsNullOrEmpty(uit.Event) ? " " : uit.Event);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == nameof(uit.Addition))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Addition), "0"), !string.IsNullOrEmpty(uit.Addition) ? uit.Addition : " ");
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == "StationsCut")
                    {
                        var stationsCut = " ";
                        switch (uit.Event)
                        {
                            case "ПРИБ.":
                                stationsCut = (uit.StationArrival != null) ? uit.StationArrival.NameRu : " ";
                                break;

                            case "ОТПР.":
                                stationsCut = (uit.StationDeparture != null) ? uit.StationDeparture.NameRu : " ";
                                break;

                            case "СТОЯНКА":
                                stationsCut = (uit.StationArrival != null && uit.StationDeparture != null) ? $"{uit.StationArrival.NameRu}-{uit.StationDeparture.NameRu}" : " ";
                                break;
                        }
                        var formatStr = string.Format(replaseStr.Replace("StationsCut", "0"), stationsCut);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == nameof(uit.Stations))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Stations), "0"), string.IsNullOrEmpty(uit.Stations) ? " " : uit.Stations);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == nameof(uit.StationArrival))
                    {
                        var stationArrival = uit.StationArrival?.NameRu ?? " ";
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.StationArrival), "0"), stationArrival);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == nameof(uit.StationDeparture))
                    {
                        var stationDeparture = uit.StationDeparture?.NameRu ?? " ";
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.StationDeparture), "0"), stationDeparture);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == nameof(uit.Note))
                    {
                        var formatStr= string.Format(replaseStr.Replace(nameof(uit.Note), "0"), string.IsNullOrEmpty(uit.Note) ? " " : uit.Note);
                        resStr.Append(formatStr);
                        continue;
                    }

                    if (subvar == nameof(uit.DaysFollowingAlias))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.DaysFollowingAlias), "0"), string.IsNullOrEmpty(uit.DaysFollowingAlias) ? " " : uit.DaysFollowingAlias);
                        resStr.Append(formatStr);
                        continue;
                    }

                    if (subvar == nameof(uit.DaysFollowing))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.DaysFollowing), "0"), string.IsNullOrEmpty(uit.DaysFollowing) ? " " : uit.DaysFollowing);
                        resStr.Append(formatStr);
                        continue;
                    }

                    if (subvar == nameof(uit.DelayTime))
                    {
                        if (uit.DelayTime == null || uit.DelayTime.Value.TimeOfDay == TimeSpan.Zero)
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.DelayTime), "0"), " ");
                            resStr.Append(formatStr);
                            continue;
                        }

                        if (mathStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.DelayTime), "0"), (uit.DelayTime == DateTime.MinValue) ? " " : uit.DelayTime.Value.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else                         //вывод в минутах
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.DelayTime), "0"), (uit.DelayTime == DateTime.MinValue) ? " " : ((uit.DelayTime.Value.Hour * 60) + (uit.DelayTime.Value.Minute)).ToString());
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == nameof(uit.ExpectedTime))
                    {
                        if (mathStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.ExpectedTime), "0"), (uit.ExpectedTime == DateTime.MinValue) ? " " : uit.ExpectedTime.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.ExpectedTime), "0"), (uit.ExpectedTime == DateTime.MinValue) ? " " : uit.ExpectedTime.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == nameof(uit.Time))
                    {
                        if (mathStr.Contains(":")) //если указанн формат времени
                        {
                            string formatStr;
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            if (dateFormat.Contains("Sec"))   //формат задан в секундах
                            {
                                var intFormat = dateFormat.Substring(3, 2);
                                var intValue = (uit.Time.Hour * 3600 + uit.Time.Minute * 60);
                                formatStr = string.Format(replaseStr.Replace(nameof(uit.Time), "0"), (intValue == 0) ? " " : intValue.ToString(intFormat));
                            }
                            else
                            {
                                formatStr = string.Format(replaseStr.Replace(nameof(uit.Time), "0"), (uit.Time == DateTime.MinValue) ? " " : uit.Time.ToString(dateFormat));
                            }

                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.Time), "0"), (uit.Time == DateTime.MinValue) ? " " : uit.Time.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == "TDepart")
                    {
                        DateTime timeDepart = DateTime.MinValue;
                        switch (uit.Event)
                        {
                            case "СТОЯНКА":
                                timeDepart = (uit.TransitTime != null && uit.TransitTime.ContainsKey("отпр")) ? uit.TransitTime["отпр"] : DateTime.MinValue;
                                break;

                            case "ОТПР.":
                                timeDepart = uit.Time;
                                break;
                        }

                        if (mathStr.Contains(":")) //если указанн формат времени
                        {
                            string formatStr;
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            if (dateFormat.Contains("Sec"))   //формат задан в секундах
                            {
                                var intFormat = dateFormat.Substring(3, 2);
                                var intValue = (uit.Time.Hour * 3600 + uit.Time.Minute * 60);
                                formatStr = string.Format(replaseStr.Replace("TDepart", "0"), (intValue == 0) ? " " : intValue.ToString(intFormat));
                            }
                            else
                            {
                                formatStr = string.Format(replaseStr.Replace("TDepart", "0"), (timeDepart == DateTime.MinValue) ? " " : timeDepart.ToString(dateFormat));
                            }

                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace("TDepart", "0"), (timeDepart == DateTime.MinValue) ? " " : timeDepart.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == "TArrival")
                    {
                        DateTime timeArrival = DateTime.MinValue;
                        switch (uit.Event)
                        {
                            case "СТОЯНКА":
                                timeArrival = (uit.TransitTime != null && uit.TransitTime.ContainsKey("приб")) ? uit.TransitTime["приб"] : DateTime.MinValue;
                                break;

                            case "ПРИБ.":
                                timeArrival = uit.Time;
                                break;
                        }

                        if (mathStr.Contains(":")) //если указанн формат времени
                        {
                            string formatStr;
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            if (dateFormat.Contains("Sec"))   //формат задан в секундах
                            {
                                var intFormat = dateFormat.Substring(3, 2);
                                var intValue = (uit.Time.Hour * 3600 + uit.Time.Minute * 60);
                                formatStr = string.Format(replaseStr.Replace("TArrival", "0"), (intValue == 0) ? " " : intValue.ToString(intFormat));
                            }
                            else
                            {
                                formatStr = string.Format(replaseStr.Replace("TArrival", "0"), (timeArrival == DateTime.MinValue) ? " " : timeArrival.ToString(dateFormat));
                            }

                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace("TArrival", "0"), (timeArrival == DateTime.MinValue) ? " " : timeArrival.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar == "Hour")
                    {
                        var formatStr = string.Format(replaseStr.Replace("Hour", "0"), DateTime.Now.Hour);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == "Minute")
                    {
                        var formatStr = string.Format(replaseStr.Replace("Minute", "0"), DateTime.Now.Minute);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == "Second")
                    {
                        var formatStr = string.Format(replaseStr.Replace("Second", "0"), DateTime.Now.Second);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (subvar == "SyncTInSec")
                    {
                        var secTime = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
                        string formatStr;
                        if (mathStr.Contains(":")) //если указан формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            formatStr = string.Format(replaseStr.Replace("SyncTInSec", "0"), (secTime == 0) ? " " : secTime.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else
                        {
                            formatStr = string.Format(replaseStr.Replace("SyncTInSec", "0"), (secTime == 0) ? " " : secTime.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (subvar.Contains("rowNumber"))
                    {
                        if (currentRow.HasValue)
                        {
                            var formatStr = CalculateMathematicFormat(replaseStr, currentRow.Value);
                            resStr.Append(formatStr);
                            continue;
                        }
                    }


                    //Добавим в неизменном виде спецификаторы байтовой информации.
                    resStr.Append(replaseStr);
                }

                return resStr.ToString();
            }

            return Body;
        }



        /// <summary>
        /// Первоначальная вставка ЗАВИСИМЫХ переменных
        /// </summary>
        private string MakeDependentInserts(string body, UniversalInputType uit)
        {
            if (body.Contains("}"))                                                           //если указанны переменные подстановки
            {
                var subStr = body.Split('}');
                StringBuilder resStr = new StringBuilder();
                for (var index = 0; index < subStr.Length; index++)
                {
                    var s = subStr[index];
                    var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                    //Подсчет кол-ва символов
                    if (replaseStr.Contains("NumberOfCharacters"))
                    {
                        var targetStr = (subStr.Length > (index + 1)) ? subStr[index + 1] : string.Empty;
                        if (Regex.Match(targetStr, "\\\"(.*)\"").Success) //
                        {
                            var matchString = Regex.Match(targetStr, "\\\"(.*)\\\"").Groups[1].Value;
                            if (!string.IsNullOrEmpty(matchString))
                            {
                                var lenght = matchString.TrimEnd('\\').Length;

                                var dateFormat = Regex.Match(replaseStr, "\\{NumberOfCharacters:(.*)\\}").Groups[1].Value;
                                if (!string.IsNullOrEmpty(dateFormat)) //если указан формат числа
                                {
                                    var formatStr = string.Format(replaseStr.Replace("NumberOfCharacters", "0"), lenght.ToString(dateFormat));
                                    resStr.Append(formatStr);
                                }
                                else
                                {
                                    var formatStr = string.Format(replaseStr.Replace(nameof(uit.AddressDevice), "0"), uit.AddressDevice);
                                    resStr.Append(formatStr);
                                }
                            }
                        }
                        continue;
                    }

                    //Добавим в неизменном виде спецификаторы байтовой информации.
                    resStr.Append(replaseStr);
                }

                return resStr.ToString().Replace("\\\"", string.Empty);
            }

            return body;
        }




        public static string CalculateMathematicFormat(string str, int row)
        {
            var matchString = Regex.Match(str, "\\{\\((.*)\\)\\:(.*)\\}").Groups[1].Value;

            var calc = new Sprache.Calc.XtensibleCalculator();
            var expr = calc.ParseExpression(matchString, rowNumber => row);
            var func = expr.Compile();
            var arithmeticResult = (int)func();

            var reultStr = str.Replace("(" + matchString + ")", "0");
            reultStr = String.Format(reultStr, arithmeticResult);

            return reultStr;
        }

        #endregion
    }


    public class ResponseRule : RequestRule
    {
        public int Time { get; set; }
    }


    public class RepeatRule
    {
        public int Count { get; set; }
        public int? DeltaX { get; set; }
        public int? DeltaY { get; set; }
    }

}