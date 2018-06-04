using System;
using System.Collections.Generic;
using AutodictorBL.Models;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.TrainRecServices
{
    public class TrainRecWorkerService : ITrainRecWorkerService
    {
        public bool CheckTrainActuality(TrainTableRec config, DateTime dateCheck, Func<int, bool> limitationTime, byte workWithNumberOfDays)
        {
            var планРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(config.DaysFollowing, config.StartTimeSchedule, config.StopTimeSchedule);
            if ((workWithNumberOfDays == 7) || (планРасписанияПоезда.ПолучитьРежимРасписания() != РежимРасписанияДвиженияПоезда.ПоДням))// TODO: добавить || для всех дальних
            {
                var активностьНаДень = планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte)(dateCheck.Month - 1), (byte)(dateCheck.Day - 1), dateCheck);
                if (активностьНаДень == false)
                    return false;

                if (limitationTime != null)
                {
                    var времяПрибытия = config.ArrivalTime;
                    var времяОтправления = config.DepartureTime;

                    bool приб = config.ArrivalTime.HasValue;
                    bool отпр = config.DepartureTime.HasValue;

                    if (приб && отпр) //ТРАНЗИТ
                    {
                        if (!limitationTime(времяОтправления.Value.Hour))
                            return false;
                    }
                    else
                    if (приб)
                    {
                        if (!limitationTime(времяПрибытия.Value.Hour))
                            return false;
                    }
                    else
                    if (отпр)
                    {
                        if (!limitationTime(времяОтправления.Value.Hour))
                            return false;
                    }
                }
            }
            else
            {
                if (планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte)4, (byte)workWithNumberOfDays, dateCheck) == false)
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Проверяет, актуальность движения поезда в день dateCheck. Если проверка успешная то проверяется на диапазон offsetTime.
        /// Внимение!!! Функция меняет дату в config.ArrivalTime или в config.DepartureTime на дату переданную в dateCheck. (с сохранением времени)
        /// </summary>
        /// <param name="config"> Проверяемый поезд из расписания</param>
        /// <param name="dateCheck">дата на которую проверяется ходит ли поезд</param>
        /// <param name="offsetTime">функция проверки попадает ли поезд в заданный диапазон дат (со временем)</param>
        /// <param name="workWithNumberOfDays">работа по календарю</param>
        /// <returns></returns>
        public bool CheckTrainActualityByOffset(TrainTableRec config, DateTime dateCheck, Func<DateTime, bool> offsetTime, byte workWithNumberOfDays)
        {
            var планРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(config.DaysFollowing, config.StartTimeSchedule, config.StopTimeSchedule);
            if ((workWithNumberOfDays == 7) || (планРасписанияПоезда.ПолучитьРежимРасписания() != РежимРасписанияДвиженияПоезда.ПоДням)) // TODO: добавить || для всех дальних
            {
                var активностьНаДень = планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte) (dateCheck.Month - 1), (byte) (dateCheck.Day - 1), dateCheck);
                if (активностьНаДень == false)
                    return false;

                var time = DateTime.Now;
                switch (config.Event)
                {
                    case Event.None:
                        break;

                    case Event.Arrival:
                        if (!config.ArrivalTime.HasValue)
                            return false;
                        time = dateCheck.Date
                              .AddHours(config.ArrivalTime.Value.Hour)
                              .AddMinutes(config.ArrivalTime.Value.Minute);
                        config.ArrivalTime = time;
                        break;

                    case Event.Departure:
                    case Event.Transit:
                        if (!config.DepartureTime.HasValue)
                            return false;
                        time = dateCheck.Date
                            .AddHours(config.DepartureTime.Value.Hour)
                            .AddMinutes(config.DepartureTime.Value.Minute);
                        config.DepartureTime = time;
                        break;
                }

                return offsetTime(time);
            }

            return true; //TODO: добавить работу по дням недели
        }



        public string GetUniqueKey(IEnumerable<string> currentKeys, DateTime addingKey)
        {
            throw new NotImplementedException();
        }
    }
}