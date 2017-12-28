using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using Castle.Core.Internal;
using CommunicationDevices.Behavior;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Settings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;


namespace CommunicationDevices.Devices
{

    /// <summary>
    /// УСТРОЙСТВО. ОБМЕН ДАННЫМИ С КОТОРЫМ ОПРЕДЕЛЯЕТСЯ IExhangeBehavior.
    /// </summary>
    public class Device
    {
        #region Prop

        public int Id { get; private set; }
        public string Address { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public BindingType BindingType { get; private set; }
        public IExhangeBehavior ExhBehavior { get; }

        public DeviceSetting Setting { get; set; }

        #endregion




        #region ctor

        public Device(int id, string address, string name, string description, IExhangeBehavior behavior, BindingType bindingType, DeviceSetting setting)
        {
            Id = id;
            Address = address;
            Name = name;
            Description = description;
            ExhBehavior = behavior;
            BindingType = bindingType;

            Setting= setting;
        }

        #endregion




        #region Methode

        public void AddOneTimeSendData(UniversalInputType inData)
        {
            inData.AddressDevice= Address;
            ExhBehavior.AddOneTimeSendData(inData);
        }


        public void AddCycleFuncData(int index, UniversalInputType inData)
        {
            inData.AddressDevice = Address;
            ExhBehavior.GetData4CycleFunc[index].Initialize(inData);     
        }


        public void AddCycleFunc()
        {
            ExhBehavior.GetData4CycleFunc.ForEach(c=> c.AddressDevice = Address);       //Добавить во все данные циклического обмена адресс.
            ExhBehavior.StartCycleExchange();
        }


        public void RemoveCycleFunc()
        {
            ExhBehavior.StopCycleExchange();
        }

        #endregion
    }
}
