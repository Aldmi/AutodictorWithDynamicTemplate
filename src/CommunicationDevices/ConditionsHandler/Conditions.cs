using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CommunicationDevices.DataProviders;
using Quartz.Util;
using NCalc;


namespace CommunicationDevices.ConditionsHandler
{
    public class Conditions
    {
        #region StaticCtor

        private static Dictionary<string, ConditionHandlerItem> _baseDict;
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
                        var res = (uit.Event == value);
                        return res;
                    })
                },

                {"типпоезда", new ConditionHandlerItem("типпоезда", ItemType.Expression, (uit, value) =>
                    {
                        var res = (uit.TypeTrain == value);
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

                {"время", new ConditionHandlerItem("время", ItemType.Expression, (uit, value) =>
                    {
                        var res = true;
                        return res;
                    })
                },

                {"классификация", new ConditionHandlerItem("классификация", ItemType.Expression, (uit, value) =>
                    {
                        var res = true;
                        return res;
                    })
                },

                {"направление", new ConditionHandlerItem("направление", ItemType.Expression, (uit, value) =>
                    {
                        var res = true;
                        return res;
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
        public List<ConditionHandlerItem> CreateListLogicalExpressions()
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
                    var handler = _baseDict[itemWs];//DeepClone
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