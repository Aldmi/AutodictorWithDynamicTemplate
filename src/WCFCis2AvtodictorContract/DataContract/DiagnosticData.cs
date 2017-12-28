using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class DiagnosticData
    {
        [DataMember]
        public int DeviceNumber { get; set; }   //Числовой Уникальный номер, присвоенный устройству в системе информирования.

        [DataMember]
        public string DeviceName { get; set; }  //Имя устройства

        [DataMember]
        public int Status { get; set; }        //Код общего статуса технического состояния устройства: исправен - неисправен

        [DataMember]
        public string Fault { get; set; }      //Строковый Описание ошибки по устройству, 
    }
}