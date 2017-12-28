using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using Domain.Entitys;
using Library.Logs;
using MainExample.Entites;

namespace MainExample.Services.GetDataService
{
    public class GetSheduleDispatcherControl : GetSheduleAbstract
    {
        #region ctor

        public GetSheduleDispatcherControl(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords) 
            : base(baseGetDataBehavior, soundRecords)
        {

        }

        #endregion





        #region Methode

        /// <summary>
        /// Обработка полученных данных
        /// </summary>
        protected override void GetaDataRxEventHandler(IEnumerable<UniversalInputType> data)
        {
            if (!Enable)
                return;

            if (data != null && data.Any())
            {
                foreach (var tr in data)
                {
                    var dateTimeArrival = tr.TransitTime["приб"];              //день и время приб.
                    var dateTimeDepart = tr.TransitTime["отпр"];               //день и время отпр.
                    var stationArrival = tr.StationArrival.NameRu;             //станция приб.
                    var stationDepart = tr.StationDeparture.NameRu;            //станция отпр.

                    //DEBUG------------------------------------------------------
                    //var str = $" N= {tr.NumberOfTrain}  Путь= {tr.PathNumber}  Время отпр={dateTimeDepart:g}   Время приб={dateTimeArrival:g}  Ст.Приб {stationArrival}   Ст.Отпр {stationDepart}";
                    //Log.log.Trace("ПОЕЗД ИЗ ПОЛУЧЕННОГО СПСИКА" + str);
                    //DEBUG-----------------------------------------------------

                    for (int i = 0; i < _soundRecords.Count; i++)
                    {
                        var rec = _soundRecords.ElementAt(i).Value;
                        var recOld = rec;
                        var idTrain = rec.IdTrain;
                        bool changeFlag = false;

                        //ТРАНЗИТ
                        if (dateTimeArrival != DateTime.MinValue && dateTimeDepart != DateTime.MinValue)
                        {
                            var numberOfTrain = (string.IsNullOrEmpty(idTrain.НомерПоезда2) || string.IsNullOrWhiteSpace(idTrain.НомерПоезда2)) ? idTrain.НомерПоезда : (idTrain.НомерПоезда + "/" + idTrain.НомерПоезда2);
                            if (tr.NumberOfTrain == numberOfTrain &&
                                dateTimeArrival.Date == idTrain.ДеньПрибытия &&
                                dateTimeDepart.Date == idTrain.ДеньОтправления &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationDepart.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                if (rec.БитыНештатныхСитуаций != tr.EmergencySituation)
                                {
                                    rec.БитыНештатныхСитуаций = tr.EmergencySituation;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. БитыНештатныхСитуаций: " + rec.БитыНештатныхСитуаций);//LOG    
                                }

                                if (rec.ВремяЗадержки != tr.DelayTime)
                                {
                                    rec.ВремяЗадержки = tr.DelayTime;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. ВремяЗадержки: " + rec.ВремяЗадержки);//LOG    
                                }

                                if (rec.ВремяСтоянки != tr.StopTime)
                                {
                                    rec.ВремяСтоянки = tr.StopTime;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. ВремяСтоянки: " + rec.ВремяЗадержки);//LOG    
                                }

                                if (rec.НомерПути != tr.PathNumber)
                                {
                                    rec.НомерПути = tr.PathNumber;
                                    rec.НомерПутиБезАвтосброса = rec.НомерПути;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. Путь: " + rec.НомерПути);//LOG    
                                }

                                if (rec.ВремяПрибытия.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["приб"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяПрибытия = tr.TransitTime["приб"];
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. ВремяПрибытия: " + rec.ВремяПрибытия);//LOG    
                                }

                                if (rec.ВремяОтправления.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["отпр"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяОтправления = tr.TransitTime["отпр"];
                                    rec.Время = rec.ВремяОтправления;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. ВремяОтправления: " + rec.ВремяОтправления);//LOG  
                                }

                                //Debug.WriteLine($"{rec.НазваниеПоезда} Время= {rec.Время} key= {key} ВремяПрибытия= {rec.ВремяПрибытия}  ВремяОтправления= {rec.ВремяОтправления}");       
                            }
                        }
                        //ПРИБ.
                        else
                        if (dateTimeArrival != DateTime.MinValue && dateTimeDepart == DateTime.MinValue)
                        {
                            if (tr.NumberOfTrain == idTrain.НомерПоезда &&
                                dateTimeArrival.Date == rec.IdTrain.ДеньПрибытия &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationArrival.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                if (rec.БитыНештатныхСитуаций != tr.EmergencySituation)
                                {
                                    rec.БитыНештатныхСитуаций = tr.EmergencySituation;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. БитыНештатныхСитуаций: " + rec.БитыНештатныхСитуаций);//LOG    
                                }

                                if (rec.ВремяЗадержки != tr.DelayTime)
                                {
                                    rec.ВремяЗадержки = tr.DelayTime;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. ВремяЗадержки: " + rec.ВремяЗадержки);//LOG    
                                }

                                if (rec.НомерПути != tr.PathNumber)
                                {
                                    rec.НомерПути = tr.PathNumber;
                                    rec.НомерПутиБезАвтосброса = rec.НомерПути;
                                    changeFlag = true;
                                   // Log.log.Trace("нашли изменения для ПРИБ. Путь: " + rec.НомерПути);//LOG   
                                }

                                if (rec.ВремяПрибытия.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["приб"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяПрибытия = tr.TransitTime["приб"];
                                    rec.Время = rec.ВремяПрибытия;
                                    changeFlag = true;
                                   // Log.log.Trace("нашли изменения для ТРАНЗИТ. ВремяПрибытия: " + rec.ВремяПрибытия);//LOG  
                                }
                            }
                        }
                        //ОТПР.
                        else
                        if (dateTimeDepart != DateTime.MinValue && dateTimeArrival == DateTime.MinValue)
                        {
                            if (tr.NumberOfTrain == idTrain.НомерПоезда &&
                                dateTimeDepart.Date == rec.IdTrain.ДеньОтправления &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationArrival.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                if (rec.БитыНештатныхСитуаций != tr.EmergencySituation)
                                {
                                    rec.БитыНештатныхСитуаций = tr.EmergencySituation;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. БитыНештатныхСитуаций: " + rec.БитыНештатныхСитуаций);//LOG    
                                }

                                if (rec.ВремяЗадержки != tr.DelayTime)
                                {
                                    rec.ВремяЗадержки = tr.DelayTime;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ТРАНЗИТ. ВремяЗадержки: " + rec.ВремяЗадержки);//LOG    
                                }

                                if (rec.НомерПути != tr.PathNumber)
                                {
                                    rec.НомерПути = tr.PathNumber;
                                    rec.НомерПутиБезАвтосброса = rec.НомерПути;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ОТПР. Путь: " + rec.НомерПути);//LOG   
                                }

                                if (rec.ВремяОтправления.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["отпр"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяОтправления = tr.TransitTime["отпр"];
                                    rec.Время = rec.ВремяОтправления;
                                    changeFlag = true;
                                    //Log.log.Trace("нашли изменения для ОТПР. ВремяОтправления: " + rec.ВремяОтправления);//LOG 
                                }
                            }
                        }

                        if (changeFlag)
                        {
                            SoundRecordChangesRx.OnNext(new SoundRecordChanges { NewRec = rec, Rec = recOld, TimeStamp = DateTime.Now, UserInfo = "Удаленный диспетчер" });
                        }
                    }
                }
            }
        }

        #endregion
    }
}