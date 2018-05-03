using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;
using Quartz.Util;
using NCalc;


namespace CommunicationDevices.ConditionsHandler
{
    /// <summary>
    /// Прверяет строковые выражения булевого типа.
    /// Все элементы выражения отделяются пробелами.
    /// ОПЕРАТОРЫ:
    /// "("
    /// ")"
    /// "&&"
    /// "||"
    /// "!"
    /// ВЫРАЖЕНИЯ:
    /// событие="приб." / "отпр." / "транз."
    /// типпоезда="тип поезда из DynamicSound.xml"
    /// путь="1,2,3,4"
    /// время="МеньшеТекВремени" / "БольшеТекВремени" / "120|120" / "120|120:60|60:10|10"
    /// классификация="дальний" / "пригород"
    /// направление="направление из Direction"
    /// задержкаприб="true" / "false"
    /// задержкаотпр="true" / "false"
    /// отмена="true" / "false"
    /// отправлениепоготовности="true" / "false"
    /// ограничениеотправкиданных="флаг отправка данных из uit"
    /// команда="очистка" / "удаление" / "рестарт" / "обновить" / "отобразить"
    /// </summary>
    public class Conditions
    {
        #region StaticCtor

        private static readonly Dictionary<string, ConditionHandlerItem> _baseDict;
        static Conditions()
        {
            _baseDict = new Dictionary<string, ConditionHandlerItem>
            {
                {"(", new ConditionHandlerItem("(", ItemType.Operator, null)},
                {")", new ConditionHandlerItem(")", ItemType.Operator, null)},
                {"&&", new ConditionHandlerItem("&&", ItemType.Operator, null)},
                {"||", new ConditionHandlerItem("||", ItemType.Operator, null)},
                {"!", new ConditionHandlerItem("!", ItemType.Operator, null)},

                {"событие", new ConditionHandlerItem("событие", ItemType.Expression, (uit, value) =>
                    {
                        Event eventTrain = Event.None;
                        switch (value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "приб.":
                                eventTrain = Event.Arrival;
                                break;
                            case "отпр.":
                                eventTrain = Event.Departure;
                                break;
                            case "транз.":
                                eventTrain = Event.Transit;
                                break;
                        }

                        var res = (uit.Event == eventTrain);
                        return res;
                    })
                },

                {"типпоезда", new ConditionHandlerItem("типпоезда", ItemType.Expression, (uit, value) =>
                    {
                        var res = (uit.TrainTypeByRyle.NameRu == value);
                        return res;
                    })
                },

                {"путь", new ConditionHandlerItem("путь", ItemType.Expression, (uit, value) =>
                    {
                        var path = value.Split(',').ToList();
                        var res = path.Contains(uit.PathNumber);
                        return res;
                    })
                },

                //Время=120|120               //ДельтаТекВремени
                //Время=МеньшеТекВремени      //МеньшеТекВремени
                //Время=БольшеТекВремени      //БольшеТекВремени
                //Время=120|120:60|60:10|10   //ДельтаТекВремениПоТипамПоездов  (по событиям)
                {"время", new ConditionHandlerItem("время", ItemType.Expression, (uit, value) =>
                    {
                        switch (value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "меньшетеквремени":
                                return uit.Time < DateTime.Now;

                            case "большетеквремени":
                                return uit.Time > DateTime.Now;
                        }

                        //Функция проверки диапазона времени в формате "120|120"  
                        Func<string, bool> checkDeltaTime = (inputStr) =>
                        {
                            var deltaTime = inputStr.Split('|');
                            if (deltaTime.Length == 2)
                            {
                                int minMinute;
                                int maxMinute;
                                if (int.TryParse(deltaTime[0], out minMinute) && int.TryParse(deltaTime[1], out maxMinute))
                                {
                                    var min = DateTime.Now.AddMinutes(-1 * minMinute);
                                    var max = DateTime.Now.AddMinutes(maxMinute);
                                    return (uit.Time > min && uit.Time < max);
                                }
                            }
                            return true;
                        };

                        // дельта по событию поезда ДельтаПриб|ДельтаОтпр|ДельтаТранзит()
                        if (value.Contains(":"))
                        {
                            var deltas = value.Split(':');
                            if (deltas.Length < 3)
                                return false;

                            int indexItem= 0;
                            switch (uit.Event)
                            {
                                case Event.Arrival:
                                    indexItem = 0;
                                    break;
                                case Event.Departure:
                                    indexItem = 1;
                                    break;
                                case Event.Transit:
                                    indexItem = 2;
                                    break;
                            }
                            var item= deltas[indexItem];
                            return checkDeltaTime(item);
                        }

                        //ДельтаТекВремени
                        return checkDeltaTime(value);
                    })
                },

                {"классификация", new ConditionHandlerItem("классификация", ItemType.Expression, (uit, value) =>      //Дальний, Приг.
                    {
                        var categoryTrain = CategoryTrain.None;
                        switch (value.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "дальний":
                                categoryTrain= CategoryTrain.LongDist;
                                break;
                            case "пригород":
                                categoryTrain= CategoryTrain.Suburb;
                                break;
                        }
                        var res = uit.TrainTypeByRyle.CategoryTrain == categoryTrain;
                        return res;
                    })
                },

                {"направление", new ConditionHandlerItem("направление", ItemType.Expression, (uit, value) =>
                    {
                        var res = uit.Direction.Name == value;
                        return res;
                    })
                },

                {"задержкаприб", new ConditionHandlerItem("задержкаприб", ItemType.Expression, (uit, value) =>
                    {
                        bool boolVal;
                        if (bool.TryParse(value, out boolVal))
                        {
                            return uit.Emergency == Emergency.DelayedArrival && boolVal;
                        }
                        return false;
                    })
                },

                {"задержкаотпр", new ConditionHandlerItem("задержкаотпр", ItemType.Expression, (uit, value) =>
                    {
                        bool boolVal;
                        if (bool.TryParse(value, out boolVal))
                        {
                            return uit.Emergency == Emergency.DelayedDeparture && boolVal;
                        }
                        return false;
                    })
                },

                {"отмена", new ConditionHandlerItem("отмена", ItemType.Expression, (uit, value) =>
                    {
                        bool boolVal;
                        if (bool.TryParse(value, out boolVal))
                        {
                            return uit.Emergency == Emergency.Cancel && boolVal;
                        }
                        return false;
                    })
                },

                {"отправлениепоготовности", new ConditionHandlerItem("отправлениепоготовности", ItemType.Expression, (uit, value) =>
                    {
                        bool boolVal;
                        if (bool.TryParse(value, out boolVal))
                        {
                            return uit.Emergency == Emergency.DispatchOnReadiness && boolVal;
                        }
                        return false;
                    })
                },

                {"ограничениеотправкиданных", new ConditionHandlerItem("ограничениеотправкиданных", ItemType.Expression, (uit, value) =>
                    {
                        bool boolVal;
                        if(bool.TryParse(value, out boolVal))
                            return uit.SendingDataLimit == boolVal;

                        return false;
                    })
                },

                {"команда", new ConditionHandlerItem("команда", ItemType.Expression, (uit, value) =>
                    {
                       var command = Command.None;
                       switch (value.ToLower(CultureInfo.InvariantCulture))
                       {
                            case "очистка":
                                command= Command.Clear;
                                break;
                            case "удаление":
                                command= Command.Delete;
                                break;
                            case "рестарт":
                                command= Command.Restart;
                                break;
                            case "обновить":
                                command= Command.Update;
                                break;
                            case "отобразить":
                                command= Command.View;
                                break;
                       }
                       return (uit.Command == command);
                    })
                },
            };
        }

        #endregion




        #region ctor

        private readonly string _conditionStr;
        public Conditions(string conditionStr)
        {
            _conditionStr = conditionStr;
            HandlerItems = CreateListLogicalExpressions();
        }

        #endregion




        #region prop

        public List<ConditionHandlerItem> HandlerItems { get; }       //Список обработчиков условий

        #endregion




        #region Methode

        /// <summary>
        /// Создать список логических выражений.
        /// </summary>
        private List<ConditionHandlerItem> CreateListLogicalExpressions()
        {
            if (string.IsNullOrEmpty(_conditionStr))
                return null;

            var handlers = new List<ConditionHandlerItem>();

            var items = _conditionStr.Split(' ');
            foreach (var item in items)
            {
                var itemWs = item.Replace(" ", string.Empty);
                //Operator-------------------------------------------
                if (_baseDict.ContainsKey(itemWs))
                {
                    var handler = _baseDict[itemWs];
                    if (handler.ItemType == ItemType.Operator)
                    {
                        handlers.Add(handler);
                        continue;
                    }
                }
                //Expression-----------------------------------------
                var kvp = ExtractKeyValuePair(itemWs);
                var key = kvp.Key.ToLower(CultureInfo.InvariantCulture);
                if (_baseDict.ContainsKey(key))
                {
                    var handler = _baseDict[key].DeepClone();
                    if (handler.ItemType == ItemType.Expression)
                    {
                        handler.Value = kvp.Value;
                        handlers.Add(handler);
                    }
                }
            }

            return handlers;
        }


        private KeyValuePair<string, string> ExtractKeyValuePair(string str)
        {
            var matchString = Regex.Match(str, "(.*)=(.*)");
            var kvp = new KeyValuePair<string, string>(matchString.Groups[1].Value, matchString.Groups[2].Value);
            return kvp;
        }


        /// <summary>
        /// Проверка ограничения
        /// </summary>
        public bool CheckContrains(UniversalInputType uit)
        {
            var sumStr = HandlerItems.Aggregate(string.Empty, (current, handler) => current + handler.Handle(uit)); //"!True&&True&&False&&!True&&True&&True"
            var sumStrLower = sumStr.ToLower(CultureInfo.InvariantCulture);
            var res = (bool)new Expression(sumStrLower).Evaluate();
            return res;
        }


        /// <summary>
        /// Проверка разрешения.
        /// </summary>
        public bool CheckResolutions(UniversalInputType inData)
        {
            return CheckContrains(inData);
        }

        #endregion
    }

}