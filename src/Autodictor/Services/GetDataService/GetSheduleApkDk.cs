using System;
using System.Collections.Generic;
using System.Linq;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using Domain.Entitys;

namespace MainExample.Services.GetDataService
{
    public class GetSheduleApkDk : GetSheduleAbstract
    {
        #region ctor

        public GetSheduleApkDk(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords) 
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
                var trainWithPut = data.Where(sh => !(string.IsNullOrEmpty(sh.PathNumber) || string.IsNullOrWhiteSpace(sh.PathNumber))).ToList();
                foreach (var tr in trainWithPut)
                {
                    //DEBUG------------------------------------------------------
                    //var str = $"N= {tr.Ntrain}  Путь= {tr.Put}  Дата отпр={tr.DtOtpr:d}  Время отпр={tr.TmOtpr:g}  Дата приб={tr.DtPrib:d} Время приб={tr.TmPrib:g}  Ст.Приб {tr.StFinish}   Ст.Отпр {tr.StDeparture}";
                    //Log.log.Fatal("ПОЕЗД ИЗ ПОЛУЧЕННОГО СПСИКА" + str);
                    //DEBUG-----------------------------------------------------

                    var dayArrival = tr.TransitTime["приб"].Date;        //день приб.
                    var dayDepart = tr.TransitTime["отпр"].Date;         //день отпр.
                    var stationArrival = tr.StationArrival.NameRu;       //станция приб.
                    var stationDepart = tr.StationDeparture.NameRu;      //станция отпр.


                    for (int i = 0; i < _soundRecords.Count; i++)
                    {
                        var key = _soundRecords.Keys.ElementAt(i);
                        var rec = _soundRecords.ElementAt(i).Value;
                        var idTrain = rec.IdTrain;

                        //ТРАНЗИТ
                        if (dayArrival != DateTime.MinValue && dayDepart != DateTime.MinValue)
                        {
                            var numberOfTrain = (string.IsNullOrEmpty(idTrain.НомерПоезда2) || string.IsNullOrWhiteSpace(idTrain.НомерПоезда2)) ? idTrain.НомерПоезда : (idTrain.НомерПоезда + "/" + idTrain.НомерПоезда2);
                            if (tr.NumberOfTrain == numberOfTrain &&
                                dayArrival == idTrain.ДеньПрибытия &&
                                dayDepart == idTrain.ДеньОтправления &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationArrival.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                // Log.log.Fatal("ТРАНЗИТ: " + numberOfTrain);//DEBUG
                                rec.НомерПути = tr.PathNumber;
                                _soundRecords[key] = rec;
                                break;
                            }
                        }
                        //ПРИБ.
                        else
                        if (dayArrival != DateTime.MinValue && dayDepart == DateTime.MinValue)
                        {
                            if (tr.NumberOfTrain == idTrain.НомерПоезда &&
                                dayArrival == rec.IdTrain.ДеньПрибытия &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationArrival.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                //Log.log.Fatal("ПРИБ: " + rec.НомерПоезда);//DEBUG
                                rec.НомерПути = tr.PathNumber;
                                _soundRecords[key] = rec;
                                break;
                            }
                        }
                        //ОТПР.
                        else
                        if (dayDepart != DateTime.MinValue && dayArrival == DateTime.MinValue)
                        {
                            if (tr.NumberOfTrain == idTrain.НомерПоезда &&
                                dayDepart == rec.IdTrain.ДеньОтправления &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationArrival.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                // Log.log.Fatal("ОТПР: " + rec.НомерПоезда);//DEBUG
                                rec.НомерПути = tr.PathNumber;
                                _soundRecords[key] = rec;
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}