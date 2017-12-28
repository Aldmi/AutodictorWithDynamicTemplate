using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace WCFAvtodictor2PcTableContract.DataContract
{
    public enum TypeTrain { None, Suburb, LongDistance }
    public enum Command { None, View, Update, Clear, Restart }

    [DataContract]
    public class UniversalDisplayType
    {
        [DataMember]
        public string NumberOfTrain { get; set; }                      //Номер поезда

        [DataMember]
        public string PathNumber { get; set; }                         //Номер пути

        [DataMember]
        public TypeTrain TypeTrain { get; set; }                      //Пригород или дальнего следования

        [DataMember]
        public string Event { get; set; }                              //Событие (отправление/прибытие)

        [DataMember]
        public string Note { get; set; }                              //Примечание. (станции следования)

        [DataMember]
        public string DaysFollowing { get; set; }                     //Дни следования

        [DataMember]
        public string Stations { get; set; }                           //Станции

        [DataMember]
        public DateTime Time { get; set; }                             //Время

        [DataMember]
        public string Message { get; set; }                            //Сообщение

        [DataMember]
        public Command Command { get; set; }                           //Команда (если указанна команда, то приоритет отдается выполнению команды а не отображению информации)

        [DataMember]
        public List<UniversalDisplayType> TableData { get; set; }     //Данные для табличного представления
    }

}