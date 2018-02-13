using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;
using MainExample.Entites;
using MainExample.Mappers;

namespace MainExample.Services.GetDataService
{
    class GetCisRegSh : GetSheduleAbstract
    {
        #region ctor

        public GetCisRegSh(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords) 
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
            try
            {
                if (!Enable)
                    return;

                var universalInputTypes = data as IList<UniversalInputType> ?? data.ToList();
                if (universalInputTypes.Any())
                {
                    var localTable = TrainSheduleTable.ЗагрузитьРасписаниеЛокальноеAsync().GetAwaiter().GetResult();
                    if (localTable == null)
                        return;

                    var tableRecords = new List<TrainTableRecord>();
                    foreach (var uit in universalInputTypes)
                    {
                        //var trTable= Mapper.MapUniversalInputType2TrainTableRecord(uit);
                        //TrainTableRecord tableRec = new TrainTableRecord();
                        var tableRec = localTable.FirstOrDefault(tr => tr.Num == uit.NumberOfTrain);
                        tableRec.Id = uit.Id;

                        tableRec.ArrivalTime = (uit.TransitTime["приб"] == DateTime.MinValue) ? string.Empty : uit.TransitTime["приб"].ToString("HH:mm");
                        tableRec.DepartureTime = (uit.TransitTime["отпр"] == DateTime.MinValue) ? string.Empty : uit.TransitTime["отпр"].ToString("HH:mm");
                        tableRec.StopTime = uit.StopTime?.ToString("t") ?? string.Empty;
                        tableRec.FollowingTime = uit.ViewBag["ItenaryTime"];

                        tableRec.ВремяНачалаДействияРасписания = uit.ViewBag.ContainsKey("ScheduleStartDateTime")
                            ? uit.ViewBag["ScheduleStartDateTime"]
                            : new DateTime(1900, 1, 1);

                        tableRec.ВремяОкончанияДействияРасписания = uit.ViewBag.ContainsKey("ScheduleEndDateTime")
                            ? uit.ViewBag["ScheduleEndDateTime"]
                            : new DateTime(2100, 1, 1);
                        if (!string.IsNullOrEmpty(tableRec.Num))
                        {
                            tableRec.Addition = uit.Addition;
                            tableRec.DaysAlias = !string.IsNullOrEmpty(uit.DaysFollowingAlias) ? uit.DaysFollowingAlias : string.Empty;

                            tableRec.StationArrival = string.IsNullOrEmpty(uit.StationArrival.NameRu) ? tableRec.StationArrival : uit.StationArrival.NameRu;
                            tableRec.StationDepart = string.IsNullOrEmpty(uit.StationDeparture.NameRu) ? tableRec.StationDepart : uit.StationDeparture.NameRu;
                            //tableRec.Name = string.IsNullOrEmpty(uit.StationArrival.NameRu + uit.StationDeparture.NameRu) ? tableRec.Name : uit.Stations;
                            tableRec.Active = !string.IsNullOrEmpty(uit.ViewBag["Enabled"]) ? (uit.ViewBag["Enabled"] == "1" ? true : false) : tableRec.Active;

                            tableRec.Direction = string.IsNullOrEmpty(uit.DirectionStation) ? tableRec.Direction : uit.DirectionStation;
                        }
                        else
                        {

                            if (uit.NumberOfTrain.Contains("/"))
                            {
                                string[] num = uit.NumberOfTrain.Split('/');
                                tableRec.Num = num[0];
                                tableRec.Num2 = num[1];
                            }
                            else
                            {
                                tableRec.Num = uit.NumberOfTrain;
                                tableRec.Num2 = string.Empty;
                            }
                            tableRec.StationArrival = uit.StationArrival.NameRu;
                            tableRec.StationDepart = uit.StationDeparture.NameRu;
                            tableRec.Name = uit.Stations;
                            tableRec.Active = true;
                            tableRec.Direction = string.Empty;
                            tableRec.Days = uit.DaysFollowing;
                            tableRec.TrainPathDirection = (byte)uit.VagonDirection;
                            tableRec.ChangeTrainPathDirection = uit.ChangeVagonDirection;
                            tableRec.SoundTemplates = uit.ViewBag["SoundTemplate"];
                            tableRec.PathWeekDayes = false;
                            tableRec.TrainPathNumber = new Dictionary<WeekDays, string>
                            {
                                [WeekDays.Постоянно] = uit.ViewBag["DefaultsPaths"],
                                [WeekDays.Пн] = "",
                                [WeekDays.Вт] = "",
                                [WeekDays.Ср] = "",
                                [WeekDays.Чт] = "",
                                [WeekDays.Пт] = "",
                                [WeekDays.Сб] = "",
                                [WeekDays.Вс] = ""
                            };
                            tableRec.ИспользоватьДополнение = new Dictionary<string, bool>
                            {
                                ["звук"] = uit.ViewBag["AdditionSendSound"] == "1" ? true : false,
                                ["табло"] = uit.ViewBag["AdditionSend"] == "1" ? true : false
                            };
                            tableRec.Примечание = uit.ViewBag["Stops"];
                        }


                        tableRecords.Add(tableRec);
                    }

                    if (tableRecords.Any())
                    {
                        //КОНТРОЛЬ ИЗМЕНЕНИЙ
                        bool changeFlag = false;
                        var remoteCisTable = TrainSheduleTable.ЗагрузитьРасписаниеЦисAsync().GetAwaiter().GetResult();

                        if (remoteCisTable != null)
                        {
                            if (remoteCisTable.Count == tableRecords.Count)
                            {
                                foreach (var trCis in remoteCisTable)
                                {
                                    var findElem = tableRecords.FirstOrDefault(t =>
                                    {
                                        var notChange = (t.Name == trCis.Name) &&
                                                     (t.ArrivalTime == trCis.ArrivalTime) &&
                                                     (t.DepartureTime == trCis.DepartureTime) &&
                                                     (t.StopTime == trCis.StopTime) &&
                                                     (t.TrainPathDirection == trCis.TrainPathDirection) &&
                                                     (t.ВремяНачалаДействияРасписания == trCis.ВремяНачалаДействияРасписания) &&
                                                     (t.ВремяОкончанияДействияРасписания == trCis.ВремяОкончанияДействияРасписания) &&
                                                     (t.Addition == trCis.Addition) &&
                                                     (t.DaysAlias == trCis.DaysAlias) &&
                                                     (t.StationArrival == trCis.StationArrival) &&
                                                     (t.StationDepart == trCis.StationDepart) &&
                                                     (t.Direction == trCis.Direction) &&
                                                     (t.ChangeTrainPathDirection == trCis.ChangeTrainPathDirection);

                                        return notChange;
                                    });

                                    //если такого элемента нет.
                                    if (string.IsNullOrEmpty(findElem.Num))
                                    {
                                        changeFlag = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                changeFlag = true;
                            }
                        }
                        else
                        {
                            //удаленная таблица не созданна
                            changeFlag = true;
                        }

                        if (changeFlag)
                        {
                            TrainSheduleTable.СохранитьИПрименитьСписокРегулярноеРасписаниеЦис(tableRecords).GetAwaiter();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
