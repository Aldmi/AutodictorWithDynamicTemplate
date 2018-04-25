using System;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.ConditionsHandler
{
    /// <summary>
    /// 
    /// </summary>
    public enum ItemType { Operator, Expression }

    /// <summary>
    /// обработчик одного заданного условия
    /// </summary>
    public class ConditionHandlerItem
    {
        #region Field

        private readonly string _element;
        private readonly Func<UniversalInputType, string, bool> _handleFunc;  // обработчик

        #endregion




        #region prop

        public ItemType ItemType { get; }
        public string Value { get; set; } = null; // значение выставляемое для обработчика

        #endregion




        #region ctor

        public ConditionHandlerItem(string element, ItemType itemType, Func<UniversalInputType, string, bool> handleFunc)
        {
            _element = element;
            ItemType = itemType;
            _handleFunc = handleFunc;
        }

        #endregion




        #region Methode

        public string Handle(UniversalInputType uit)
        {
            switch (ItemType)
            {
                case ItemType.Operator:
                    return _element;

                case ItemType.Expression:
                    if (Value == null)
                        throw new ArgumentException("Value не должно быть равно null");

                    return _handleFunc(uit, Value).ToString();
            }
            return null;
        }

        #endregion
    }
}