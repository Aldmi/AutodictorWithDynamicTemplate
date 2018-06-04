using System;
using System.Collections.Generic;
using System.Linq;
using AutodictorBL.Models;
using DAL.Abstract.Entitys;


namespace MainExample.Services
{
    public class SchedulingPipelineService
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


        public string GetUniqueKey(IEnumerable<string> currentKeys,  DateTime addingKey)
        {
            int tryCounter = 50;
            while (--tryCounter > 0)
            {
                string key = addingKey.ToString("yy.MM.dd  HH:mm:ss");
                if (!currentKeys.Contains(key))
                {
                    return key;
                }
                addingKey = addingKey.AddSeconds(1);
            }

            return null;

           // throw new Exception($"Невозможно добавить запись под ключем: {addingKey:yy.MM.dd  HH:mm:ss}");
        }

    }
}