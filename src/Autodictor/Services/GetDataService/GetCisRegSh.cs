using System.Collections.Generic;
using AutodictorBL.Services.DataAccessServices;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace MainExample.Services.GetDataService
{
    public class GetCisRegSh : GetSheduleAbstract
    {
        private readonly IUsersRepository _usersRepository;
        private readonly TrainRecService _trainRecService;


        #region ctor

        public GetCisRegSh(BaseGetDataBehavior baseGetDataBehavior, IUsersRepository usersRepository, TrainRecService trainRecService) 
            : base(baseGetDataBehavior)
        {
            _usersRepository = usersRepository;
            _trainRecService = trainRecService;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обработка полученных данных
        /// </summary>
        protected override void GetaDataRxEventHandler(IEnumerable<UniversalInputType> data)
        {
            //try
            //{
            //    if (!Enable)
            //        return;

            //    var universalInputTypes = data as IList<UniversalInputType> ?? data.ToList();
            //    if (universalInputTypes.Any())
            //    {
            //        if (universalInputTypes.FirstOrDefault(u => string.IsNullOrWhiteSpace(u.NumberOfTrain) && u.ViewBag.ContainsKey("login") && u.ViewBag["login"] != null) != null)
            //        {
            //            var usersDb = _usersRepository.List().ToList();
            //            usersDb.Clear();
            //            _usersRepository.Delete(u => true);
            //            foreach (var uit in universalInputTypes)
            //            {
            //                try
            //                {
            //                    User user = new User();

            //                    user.Id = uit.Id;
            //                    user.Login = uit.ViewBag["login"];
            //                    user.Password = uit.ViewBag["hash_salt_pass"];
            //                    int role_id = uit.ViewBag["role"];
            //                    switch (role_id)
            //                    {
            //                        case 9:
            //                            user.Role = Role.Диктор; break; // users - Основные пользователи
            //                        case 1:
            //                            user.Role = Role.Администратор; break; // imperator - Основная роль root админа
            //                        case 7:
            //                            user.Role = Role.Администратор; break; // administrator - Администратор ЦТС ПАСС
            //                        case 8:
            //                            user.Role = Role.Инженер; break; // sysadmin - Администратор ПТК
            //                        case 3:
            //                            user.Role = Role.Инженер; break; // apiReaders - Для чтения с API
            //                        case 4:
            //                            user.Role = Role.Инженер; break; // system - system
            //                        case 5:
            //                            user.Role = Role.Инженер; break; // daemon - Демоны
            //                        default:
            //                            user.Role = Role.Наблюдатель; break; // любой недокументированный id
            //                    }
            //                    user.IsEnabled = uit.ViewBag["status"];
            //                    user.StartDate = uit.ViewBag["start_date"];
            //                    user.EndDate = uit.ViewBag["end_date"];

            //                    usersDb.Add(user);
            //                }
            //                catch (Exception ex)
            //                {
            //                    System.Windows.Forms.MessageBox.Show("Не получилось обновить репозиторий. Ошибка: " + ex.Message);
            //                }
            //            }
            //            _usersRepository.AddRange(usersDb);
            //        }
            //        else
            //        {
                       
            //            var localTable = _trainRecService.GetAllAsync(TrainRecType.LocalMain).GetAwaiter().GetResult();
            //            if (localTable == null)
            //                return;


            //            //var tableRecords = new List<TrainTableRecord>();
            //            var tableRecords = new List<TrainTableRec>();
            //            foreach (var uit in universalInputTypes)
            //            {
            //                try
            //                {
            //                    uit.StationDeparture = _trainRecService.GetStationByCodeExpressStation(uit.StationDeparture.CodeExpress);
            //                    uit.StationArrival = _trainRecService.GetStationByCodeExpressStation(uit.StationArrival.CodeExpress);
            //                    uit.Stations = $"{uit.StationDeparture?.NameRu} - {uit.StationArrival?.NameRu}";
            //                    string[] num;
            //                    if (uit.NumberOfTrain.Contains("/"))
            //                    {
            //                        num = uit.NumberOfTrain.Split('/');
            //                    }
            //                    else
            //                    {
            //                        num = new string[] { uit.NumberOfTrain, string.Empty };
            //                    }
            //                    var tableRec = localTable.FirstOrDefault(tr => num[0] == tr.Num); // проверяем соответствие первой части номера в ЦИС и локальной базе
            //                                                                                      // если нет - создаем пустой объект. При любом условии переписываем все
            //                                                                                      // параметры, заданные ниже

            //                    tableRec.Id = uit.ScheduleId; // ID принимаем из ЦИС - является уникальным ID поезда в системах РЖД
            //                    if (tableRecords.Exists(tr => tr.Id == tableRec.Id))
            //                        continue;

            //                    // для существующих в локали поездов особые условия проверки (условие можно будет убрать полностью после автоматического получения всех необходимых параметров)
            //                    if (!string.IsNullOrWhiteSpace(tableRec.Num))
            //                    {
            //                        // Num и Num2 оставляем по умолчанию
            //                        if (!string.IsNullOrWhiteSpace(uit.Stations))
            //                            tableRec.Name = uit.Stations;
            //                        if (!string.IsNullOrWhiteSpace(uit.DirectionStation))
            //                            tableRec.Direction = uit.DirectionStation;
            //                        if (!string.IsNullOrWhiteSpace(uit.StationDeparture.NameRu))
            //                            tableRec.StationDepart = uit.StationDeparture.NameRu;
            //                        if (!string.IsNullOrWhiteSpace(uit.StationArrival.NameRu))
            //                            tableRec.StationArrival = uit.StationArrival.NameRu;
            //                        if (uit.TransitTime.ContainsKey("приб") && uit.TransitTime["приб"] != DateTime.MinValue)
            //                            tableRec.ArrivalTime = uit.TransitTime["приб"].ToString("hh:mm");
            //                        if (uit.StopTime != TimeSpan.MinValue)
            //                            tableRec.StopTime = uit.StopTime.ToString();
            //                        if (uit.TransitTime.ContainsKey("отпр") && uit.TransitTime["отпр"] != DateTime.MinValue)
            //                            tableRec.DepartureTime = uit.TransitTime["отпр"].ToString("hh:mm");
            //                        if (uit.ViewBag.ContainsKey("ItenaryTime") && !string.IsNullOrWhiteSpace(uit.ViewBag["ItenaryTime"]))
            //                            tableRec.FollowingTime = uit.ViewBag["ItenaryTime"];
            //                        //tableRec.Days = <метод преобразования в нужную строку>;
            //                        if (string.IsNullOrWhiteSpace(tableRec.Days))
            //                            tableRec.Days = string.Empty;
            //                        if (!string.IsNullOrWhiteSpace(uit.DaysFollowingAlias))
            //                            tableRec.DaysAlias = uit.DaysFollowingAlias;
            //                        if (uit.ViewBag.ContainsKey("Enabled") && !string.IsNullOrWhiteSpace(uit.ViewBag["Enabled"]))
            //                            tableRec.Active = uit.ViewBag["Enabled"] == "1" ? true : false;
            //                        if (uit.ViewBag.ContainsKey("SoundTemplate") && !string.IsNullOrWhiteSpace(uit.ViewBag["SoundTemplate"]))
            //                            tableRec.SoundTemplates = uit.ViewBag["SoundTemplate"];
            //                        if (uit.VagonDirection != VagonDirection.None)
            //                            tableRec.TrainPathDirection = (byte)uit.VagonDirection;
            //                        if (tableRec.ChangeTrainPathDirection != true)
            //                            tableRec.ChangeTrainPathDirection = uit.ChangeVagonDirection;
            //                        // TrainPathNumber
            //                        // PathWeekDayes
            //                        //if (string.IsNullOrWhiteSpace(tableRec.TrainTypeByRyle.NameRu) && !string.IsNullOrWhiteSpace(uit.TypeTrain))
            //                        //    tableRec.TrainTypeByRyle.NameRu = uit.TypeTrain;
            //                        if (!string.IsNullOrWhiteSpace(uit.Note))
            //                            tableRec.Примечание = uit.Note;
            //                        if (uit.ViewBag.ContainsKey("ScheduleStartDateTime") && uit.ViewBag["ScheduleStartDateTime"] != DateTime.MinValue)
            //                            tableRec.ВремяНачалаДействияРасписания = uit.ViewBag["ScheduleStartDateTime"];
            //                        if (uit.ViewBag.ContainsKey("ScheduleEndDateTime") && uit.ViewBag["ScheduleEndDateTime"] != DateTime.MinValue)
            //                            tableRec.ВремяОкончанияДействияРасписания = uit.ViewBag["ScheduleEndDateTime"];
            //                        if (!string.IsNullOrWhiteSpace(uit.Addition) && string.IsNullOrWhiteSpace(tableRec.Addition))
            //                            tableRec.Addition = uit.Addition;
            //                        // ИспользоватьДополнение
            //                        // Автомат
            //                        // IsScoreBoardOutput
            //                        // IsSoundOutput
            //                    }
            //                    // новые поезда просто заливаем как есть
            //                    else
            //                    {
            //                        tableRec.Num = num[0];
            //                        tableRec.Num2 = num[1];
            //                        tableRec.Direction = string.Empty;

            //                        tableRec.StationDepart = uit.StationDeparture?.NameRu ?? string.Empty;
            //                        tableRec.StationArrival = uit.StationArrival?.NameRu ?? string.Empty;
            //                        tableRec.Name = uit.Stations;

            //                        // Проверяем, верно ли распарсилось время прибытия
            //                        tableRec.ArrivalTime = uit.TransitTime.ContainsKey("приб") && (uit.TransitTime["приб"] != DateTime.MinValue) ? uit.TransitTime["приб"].ToString("hh:mm") : string.Empty;
            //                        tableRec.StopTime = uit.StopTime?.ToString("t") ?? string.Empty; // Время стоянки
            //                        // Проверяем, верно ли распарсилось время отправления
            //                        tableRec.DepartureTime = uit.TransitTime.ContainsKey("отпр") && (uit.TransitTime["отпр"] != DateTime.MinValue) ? uit.TransitTime["отпр"].ToString("hh:mm") : string.Empty;
            //                        tableRec.FollowingTime = uit.ViewBag.ContainsKey("ItenaryTime") ? uit.ViewBag["ItenaryTime"] : string.Empty;
            //                        tableRec.Days = uit.DaysFollowing != null ? uit.DaysFollowing : string.Empty;
            //                        tableRec.DaysAlias = uit.DaysFollowingAlias != null ? uit.DaysFollowingAlias : string.Empty;
            //                        tableRec.Active = true;
            //                        tableRec.SoundTemplates = uit.ViewBag.ContainsKey("SoundTemplate") ? uit.ViewBag["SoundTemplate"] : string.Empty;
            //                        tableRec.TrainPathDirection = (byte)uit.VagonDirection;
            //                        tableRec.ChangeTrainPathDirection = uit.ChangeVagonDirection;
            //                        tableRec.TrainPathNumber = new Dictionary<WeekDays, string>
            //                        {
            //                            [WeekDays.Постоянно] = uit.ViewBag.ContainsKey("DefaultPaths") ? uit.ViewBag["DefaultsPaths"] : "",
            //                            [WeekDays.Пн] = "",
            //                            [WeekDays.Вт] = "",
            //                            [WeekDays.Ср] = "",
            //                            [WeekDays.Чт] = "",
            //                            [WeekDays.Пт] = "",
            //                            [WeekDays.Сб] = "",
            //                            [WeekDays.Вс] = ""
            //                        };
            //                        tableRec.PathWeekDayes = false;
            //                        //tableRec.ТипПоезда = (ТипПоезда)uit.TypeTrain;
            //                        tableRec.Примечание = uit.Note != null ? uit.Note : string.Empty;

            //                        tableRec.ВремяНачалаДействияРасписания = uit.ViewBag.ContainsKey("ScheduleStartDateTime")
            //                            ? uit.ViewBag["ScheduleStartDateTime"]
            //                            : new DateTime(1900, 1, 1);

            //                        tableRec.ВремяОкончанияДействияРасписания = uit.ViewBag.ContainsKey("ScheduleEndDateTime")
            //                            ? uit.ViewBag["ScheduleEndDateTime"]
            //                            : new DateTime(2100, 1, 1);

            //                        tableRec.Addition = uit.Addition != null ? uit.Addition : string.Empty;
            //                        tableRec.ИспользоватьДополнение = new Dictionary<string, bool>
            //                        {
            //                            ["звук"] = uit.ViewBag.ContainsKey("AdditionSendSound") && uit.ViewBag["AdditionSendSound"] == "1" ? true : false,
            //                            ["табло"] = uit.ViewBag.ContainsKey("AdditionSend") && uit.ViewBag["AdditionSend"] == "1" ? true : false
            //                        };
            //                        tableRec.Автомат = true;
            //                        tableRec.IsScoreBoardOutput = true;
            //                        tableRec.IsSoundOutput = true;
            //                    }

            //                    tableRecords.Add(tableRec);
            //                }
            //                catch (Exception ex)
            //                {
            //                    System.Windows.Forms.MessageBox.Show("Поезд № " + uit.NumberOfTrain + " " + uit.StationDeparture.NameRu + " - " + uit.StationArrival.NameRu + " " + uit.Addition + "; Ошибка: " + ex.Message);
            //                }

            //            }
            //            if (tableRecords.Any())
            //            {
            //                //КОНТРОЛЬ ИЗМЕНЕНИЙ
            //                bool changeFlag = false;         
            //                var remoteCisTable = _trainRecService.GetAllAsync(TrainRecType.RemoteCis).GetAwaiter().GetResult();
            //                if (remoteCisTable != null)
            //                {
            //                    if (remoteCisTable.Count == tableRecords.Count)
            //                    {
            //                        foreach (var trCis in remoteCisTable)
            //                        {
            //                            var findElem = tableRecords.FirstOrDefault(t =>
            //                            {
            //                                var notChange = (t.Name == trCis.Name) &&
            //                                             (t.ArrivalTime == trCis.ArrivalTime) &&
            //                                             (t.DepartureTime == trCis.DepartureTime) &&
            //                                             (t.StopTime == trCis.StopTime) &&
            //                                             (t.TrainPathDirection == trCis.TrainPathDirection) &&
            //                                             (t.ВремяНачалаДействияРасписания == trCis.ВремяНачалаДействияРасписания) &&
            //                                             (t.ВремяОкончанияДействияРасписания == trCis.ВремяОкончанияДействияРасписания) &&
            //                                             (t.Addition == trCis.Addition) &&
            //                                             (t.DaysAlias == trCis.DaysAlias) &&
            //                                             (t.StationArrival == trCis.StationArrival) &&
            //                                             (t.StationDepart == trCis.StationDepart) &&
            //                                             (t.Direction == trCis.Direction) &&
            //                                             (t.ChangeTrainPathDirection == trCis.ChangeTrainPathDirection);

            //                                return notChange;
            //                            });

            //                            //если такого элемента нет.
            //                            if (string.IsNullOrEmpty(findElem.Num))
            //                            {
            //                                changeFlag = true;
            //                                break;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        changeFlag = true;
            //                    }
            //                }
            //                else
            //                {
            //                    //удаленная таблица не созданна
            //                    changeFlag = true;
            //                }

            //                if (changeFlag)
            //                {
            //                    _trainRecService.SaveListByRemoteCis(tableRecords).GetAwaiter();
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        #endregion
    }
}
