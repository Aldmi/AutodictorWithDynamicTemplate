using System;
using System.Collections.Generic;
using AutodictorBL.DataAccess;
using AutodictorBL.Services.SoundRecordServices;
using Autofac;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Model;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Changes;
using DAL.NoSqlLiteDb.Entityes;
using Force.DeepCloner;


namespace MainExample.Mappers
{
    public static class Mapper
    {
        private static  IContainer _container;
        public static void SetContainer(IContainer container)
        {
            _container = container;
        }


        public static List<СтатическоеСообщение> MapSoundConfigurationRecord2СтатическоеСообщение(SoundConfigurationRecord scr, ref int newId)
        {
            СтатическоеСообщение statRecord;
            statRecord.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
            List<СтатическоеСообщение> resultList = new List<СтатическоеСообщение>();

            if (scr.Enable == true)
            {
                if (scr.EnablePeriodic == true)
                {
                    statRecord.ОписаниеКомпозиции = scr.Name;
                    statRecord.НазваниеКомпозиции = scr.Name;

                    if (statRecord.НазваниеКомпозиции == string.Empty)
                        return null;

                    string[] Times = scr.MessagePeriodic.Split(',');
                    if (Times.Length != 3)
                        return null;

                    DateTime НачалоИнтервала2 = DateTime.Parse(Times[0]), КонецИнтервала2 = DateTime.Parse(Times[1]);
                    int Интервал = int.Parse(Times[2]);

                    while (НачалоИнтервала2 < КонецИнтервала2)
                    {
                        statRecord.ID = newId++;
                        statRecord.Время = НачалоИнтервала2;
                        statRecord.Активность = true;

                        resultList.Add(statRecord);
                        НачалоИнтервала2 = НачалоИнтервала2.AddMinutes(Интервал);
                    }
                }

                if (scr.EnableSingle == true)
                {
                    statRecord.ОписаниеКомпозиции = scr.Name;
                    statRecord.НазваниеКомпозиции = scr.Name;

                    if (statRecord.НазваниеКомпозиции == string.Empty)
                        return null;

                    string[] Times = scr.MessageSingle.Split(',');

                    foreach (string time in Times)
                    {
                        statRecord.ID = newId++;
                        statRecord.Время = DateTime.Parse(time);
                        statRecord.Активность = true;

                        resultList.Add(statRecord);
                    }
                }
            }

            return resultList;
        }



        public static SoundRecord MapTrainTableRecord2SoundRecord(TrainTableRec config, ISoundReсordWorkerService soundReсordWorkerService, DateTime day, int id)
        {
            var record = new SoundRecord();
            record.Id = id;
            record.IdTrain = new IdTrain(config.Id);
            record.НомерПоезда = config.Num;
            record.НомерПоезда2 = config.Num2;
            record.НазваниеПоезда = config.Name;
            record.Дополнение = config.Addition;
            record.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = config.ИспользоватьДополнение["звук"],
                ["табло"] = config.ИспользоватьДополнение["табло"]
            };
            record.Направление = config.Direction?.Name;
            record.ДниСледования = config.Days;
            record.ДниСледованияAlias = config.DaysAlias;
            record.Активность = config.Active;
            record.Автомат = config.Автомат;
            record.ШаблонВоспроизведенияСообщений = String.Empty;//config.SoundTemplates;
            record.Pathway = ПолучитьНомерПутиПоДнямНедели(config).DeepClone();
            record.НомерПути = ПолучитьНомерПутиПоДнямНедели(config)?.Name ?? string.Empty;
            record.НомерПутиБезАвтосброса = record.НомерПути;
            record.НумерацияПоезда = (byte)config.WagonsNumbering;
            record.СменнаяНумерацияПоезда = config.ChangeTrainPathDirection ?? false;
            record.Примечание = config.Примечание;
            record.ТипПоезда = config.TrainTypeByRyle;
            record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
            record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
            record.Описание = "";
            record.КоличествоПовторений = 1;
            record.СостояниеКарточки = 0;
            record.ОписаниеСостоянияКарточки = "";
            record.ТаймерПовторения = 0;
            record.РазрешениеНаОтображениеПути = PathPermissionType.ИзФайлаНастроек;

            record.ИменаФайлов = new string[0];
            record.ФиксированноеВремяПрибытия = null;
            record.ФиксированноеВремяОтправления = null;

            record.СтанцияОтправления = config.StationDepart != null ? config.StationDepart.NameRu : string.Empty;
            record.СтанцияНазначения = config.StationArrival != null ? config.StationArrival.NameRu : string.Empty;

            record.ВыводНаТабло = config.IsScoreBoardOutput;
            record.ВыводЗвука= config.IsSoundOutput;


            //DateTime времяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
            //DateTime времяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);
            record.ВремяПрибытия = day;
            record.ВремяОтправления = day;
            record.ОжидаемоеВремя = day;
            record.ВремяСледования = null;
            record.ВремяЗадержки = null;
            byte номерСписка = 0x00;

            record.Event = config.Event;

            if (config.ArrivalTime.HasValue)
            {
                record.ВремяПрибытия = day.Add(config.ArrivalTime.Value.TimeOfDay);
                record.ОжидаемоеВремя = record.ВремяПрибытия;
                номерСписка |= 0x04;
            }

            if (config.DepartureTime.HasValue)
            {
                record.ВремяОтправления = day.Add(config.DepartureTime.Value.TimeOfDay);
                record.ОжидаемоеВремя = record.ВремяОтправления;
                номерСписка |= 0x10;
            }
   
            record.ВремяСледования = config.FollowingTime;
                
            
            //ТРАНЗИТ
            record.ВремяСтоянки = null;
            if (номерСписка == 0x14)
            {
                if (record.ВремяОтправления < record.ВремяПрибытия)          
                {
                    record.ВремяПрибытия = record.ВремяПрибытия.AddDays(-1);
                    record.IdTrain.ДеньПрибытия = record.ВремяПрибытия.Date;
                }
                record.ВремяСтоянки = config.StopTime;
                номерСписка |= 0x08;                          
            }

            //транзиты по ОТПР-------------------
            if ((номерСписка & 0x10) == 0x10 ||
                (номерСписка & 0x14) == 0x14)
            {
                record.Время = record.ВремяОтправления;
                record.ОжидаемоеВремя = record.ВремяОтправления;
            }
            else
            {
                record.Время = record.ВремяПрибытия;
                record.ОжидаемоеВремя = record.ВремяПрибытия;
            }

            record.БитыАктивностиПолей = номерСписка;
            record.БитыАктивностиПолей |= 0x03;                                   //TODO: ???


            // Шаблоны оповещения
            record.ActionTrainDynamiсList= soundReсordWorkerService.СreateActionTrainDynamic(record, config.ActionTrains);
            record.EmergencyTrainStaticList = config.EmergencyTrains.DeepClone();
            record.EmergencyTrainDynamiсList = new List<ActionTrainDynamic>();
            record.Emergency = Emergency.None;

            record.AplyIdTrain();

            return record;
        }



        public static UniversalInputType MapTrainTableRecord2UniversalInputType(TrainTableRec t)
        {
            Func<DateTime?, DateTime?, DateTime> timePars = (arrivalTime, departTime) =>
            {
                if (arrivalTime.HasValue)
                    return arrivalTime.Value;

                if (departTime.HasValue)
                    return departTime.Value;

                return DateTime.MinValue;
            };

            Func<Event, string> eventPars = (classification) =>
            {
                switch (classification)
                {
                    case Event.Arrival:
                        return "ПРИБ.";
                   
                    case Event.Departure:
                        return "ОТПР.";
                       
                    case Event.Transit:
                        return "СТОЯНКА";
                        
                    default:
                        return string.Empty;
                }
            };


            Func<DateTime?, DateTime?, Dictionary <string, DateTime>> transitTimePars = (arrivalTime, departTime) =>
            {
                var transitTime = new Dictionary<string, DateTime>();
                if (arrivalTime.HasValue && departTime.HasValue)
                {
                    transitTime["приб"] = arrivalTime.Value;
                    transitTime["отпр"] = departTime.Value;
                }

                return transitTime;
            };

            UniversalInputType uit = new UniversalInputType
            {
                IsActive = t.Active,
                Id = t.Id,
                EventOld = eventPars(t.Event),
                Event = t.Event,
                TypeTrain = t.TrainTypeByRyle.NameRu,
                TrainTypeByRyle = t.TrainTypeByRyle,
                Note = t.Примечание, //C остановками: ...
                WagonsNumbering = t.WagonsNumbering,
                NumberOfTrain = t.Num,
                Stations = t.Name,
                DirectionStation = t.Direction.Name,
                Direction = t.Direction,
                StationDeparture = t.StationDepart,
                StationArrival = t.StationArrival,
                Time = timePars(t.ArrivalTime, t.DepartureTime), 
                DepartureTime = t.DepartureTime,
                ArrivalTime = t.ArrivalTime,
                TransitTime = transitTimePars(t.ArrivalTime, t.DepartureTime),
                DelayTime = null,
                StopTime = t.StopTime,
                ExpectedTime = timePars(t.ArrivalTime, t.DepartureTime),
                DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(t.Days).ПолучитьСтрокуОписанияРасписания(),
                DaysFollowingAlias = t.DaysAlias,
                Addition = t.Addition,
                SendingDataLimit = t.IsScoreBoardOutput,
                Command = Command.None,
                Emergency = Emergency.None,
                EmergencySituation = 0x00
            };

            return uit;
        }



        public static TrainTableRec MapUniversalInputType2TrainTableRecord(UniversalInputType uit)
        {
            var tableRec= new TrainTableRec();
            return tableRec;
        }



        public static Pathway ПолучитьНомерПутиПоДнямНедели(TrainTableRec record)
        {
            if (!record.PathWeekDayes)
            {
                return record.TrainPathNumber[WeekDays.Постоянно];
            }

            DayOfWeek dayOfWeek= DateTime.Now.DayOfWeek;
            switch (MainWindowForm.РаботаПоНомеруДняНедели)  //TODO: РаботаПоНомеруДняНедели внедрять через DI
            {
                case 0:
                    dayOfWeek = DayOfWeek.Monday;
                    break;

                case 1:
                    dayOfWeek = DayOfWeek.Tuesday;
                    break;

                case 2:
                    dayOfWeek = DayOfWeek.Wednesday;
                    break;

                case 3:
                    dayOfWeek = DayOfWeek.Thursday;
                    break;

                case 4:
                    dayOfWeek = DayOfWeek.Friday;
                    break;

                case 5:
                    dayOfWeek = DayOfWeek.Saturday;
                    break;

                case 6:
                    dayOfWeek = DayOfWeek.Sunday;
                    break;
            }

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return record.TrainPathNumber[WeekDays.Пн];

                case DayOfWeek.Tuesday:
                    return record.TrainPathNumber[WeekDays.Вт];

                case DayOfWeek.Wednesday:
                    return record.TrainPathNumber[WeekDays.Ср];

                case DayOfWeek.Thursday:
                    return record.TrainPathNumber[WeekDays.Чт];

                case DayOfWeek.Friday:
                    return record.TrainPathNumber[WeekDays.Пт];

                case DayOfWeek.Saturday:
                    return record.TrainPathNumber[WeekDays.Сб];

                case DayOfWeek.Sunday:
                    return record.TrainPathNumber[WeekDays.Вс];
            }

            return null;
        }



        public static UniversalInputType MapSoundRecord2UniveralInputType(SoundRecord data, bool pathPermission, bool isShow)
        {
            DateTime time = DateTime.MinValue;
            Dictionary<string, DateTime> transitTimes = new Dictionary<string, DateTime>();

            string actStr = "   ";

            var номерПоезда = data.НомерПоезда;
            if ((data.БитыАктивностиПолей & 0x14) == 0x14)
            {
                actStr = "СТОЯНКА";
                time = data.ВремяПрибытия; //TODO: выполняется фильтрация по этому полю, нужно понять по какому времени фильтровать
                transitTimes["приб"] = data.ВремяПрибытия;
                transitTimes["отпр"] = data.ВремяОтправления;

                номерПоезда = (string.IsNullOrEmpty(data.НомерПоезда2) || string.IsNullOrWhiteSpace(data.НомерПоезда2)) ? data.НомерПоезда : (data.НомерПоезда + "/" + data.НомерПоезда2);
            }
            else if ((data.БитыАктивностиПолей & 0x04) == 0x04)
            {
                actStr = "ПРИБ.";
                time = data.ВремяПрибытия;
                номерПоезда = data.НомерПоезда;
            }
            else if ((data.БитыАктивностиПолей & 0x10) == 0x10)
            {
                actStr = "ОТПР.";
                time = data.ВремяОтправления;
                номерПоезда = data.НомерПоезда;
            }


            var command = Command.None;
            switch (data.СостояниеОтображения)
            {
                case TableRecordStatus.Отображение:
                    command = Command.View;
                    break;

                case TableRecordStatus.Очистка:
                    command = Command.Delete;
                    break;

                case TableRecordStatus.Обновление:
                    command = Command.Update;
                    break;
            }

            var номерПути = string.Empty;
            switch (data.РазрешениеНаОтображениеПути)
            {
                case PathPermissionType.ИзФайлаНастроек:
                    номерПути = pathPermission ? data.НомерПути : "   ";
                    break;

                case PathPermissionType.Отображать:
                    номерПути = data.НомерПути;
                    break;

                case PathPermissionType.НеОтображать:
                    номерПути = "   ";
                    break;
            }


            Station cтанцияОтправления;
            Station cтанцияНазначения;
            var defaultStation = ExchangeModel.NameRailwayStation;
            using (var scope = _container.BeginLifetimeScope())
            {
                var trainRecService = scope.Resolve<TrainRecService>();
                trainRecService.GetStationInDirectionByNameStation(data.Направление, data.СтанцияОтправления);
                cтанцияОтправления = trainRecService.GetStationInDirectionByNameStation(data.Направление, data.СтанцияОтправления) ?? defaultStation;
                cтанцияНазначения = trainRecService.GetStationInDirectionByNameStation(data.Направление, data.СтанцияНазначения) ?? defaultStation;
            }

            UniversalInputType mapData;
            if (isShow)
            {
                mapData = new UniversalInputType
                {
                    Id = data.Id,
                    ScheduleId = data.IdTrain.ScheduleId,
                    IsActive = data.Активность,
                    NumberOfTrain = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? номерПоезда : "   ",
                    WagonsNumbering = data.WagonsNumbering,
                    ChangeVagonDirection = data.СменнаяНумерацияПоезда,
                    PathNumber = номерПути,
                    PathNumberWithoutAutoReset = data.НомерПутиБезАвтосброса,
                    EventOld = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? actStr : "   ",
                    Event = data.Event,
                    Time = time,
                    TransitTime = transitTimes,
                    DelayTime = data.ВремяЗадержки,
                    ExpectedTime = data.ОжидаемоеВремя,
                    StopTime = data.ВремяСтоянки,
                    Stations = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.НазваниеПоезда : "   ",
                    DirectionStation = data.Направление,
                    Emergency = data.Emergency,
                    TrainTypeByRyle = data.ТипПоезда,
                    StationDeparture = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияОтправления : new Station(),
                    StationArrival = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияНазначения : new Station(),
                    Note = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.Примечание : "   ",
                    TypeTrain = data.ТипПоезда.NameRu,
                    DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(data.ДниСледования).ПолучитьСтрокуОписанияРасписания(),
                    DaysFollowingAlias = data.ДниСледованияAlias,
                    Addition = (data.ИспользоватьДополнение["табло"]) ? data.Дополнение : string.Empty,
                    SendingDataLimit = data.ВыводНаТабло,
                    Command = command,
                    EmergencySituation = (byte)data.Emergency  //TODO: в UIT ввести Emergency
                };
            }
            else
            {
                mapData = new UniversalInputType
                {
                    Id = data.Id,
                    ScheduleId = data.IdTrain.ScheduleId,
                    IsActive = data.Активность,
                    NumberOfTrain = номерПоезда,
                    WagonsNumbering = data.WagonsNumbering,
                    ChangeVagonDirection = data.СменнаяНумерацияПоезда,
                    PathNumber = номерПути,
                    PathNumberWithoutAutoReset = data.НомерПутиБезАвтосброса,
                    EventOld = actStr,
                    Event = data.Event,
                    Time = time,
                    TransitTime = transitTimes,
                    DelayTime = data.ВремяЗадержки,
                    ExpectedTime = data.ОжидаемоеВремя,
                    StopTime = data.ВремяСтоянки,
                    Stations = data.НазваниеПоезда,
                    DirectionStation = data.Направление,
                    Emergency = data.Emergency,
                    TrainTypeByRyle = data.ТипПоезда,
                    StationDeparture =  cтанцияОтправления,  //(data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияОтправления : new Station(),
                    StationArrival = cтанцияНазначения,     // (data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияНазначения : new Station(), 
                    Note = data.Примечание,
                    TypeTrain = data.ТипПоезда.NameRu,
                    DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(data.ДниСледования).ПолучитьСтрокуОписанияРасписания(),
                    DaysFollowingAlias = data.ДниСледованияAlias,
                    Addition = (data.ИспользоватьДополнение["табло"]) ? data.Дополнение : string.Empty,
                    SendingDataLimit = data.ВыводНаТабло,
                    Command = command,
                    EmergencySituation = (byte)data.Emergency
                };
            }

            return mapData;
        }




        public static SoundRecordDb MapSoundRecord2SoundRecordDb(SoundRecord data)
        {
            return new SoundRecordDb
            {
                Id = data.Id,
                Автомат = data.Автомат,
                Активность = data.Активность,
                БитыАктивностиПолей = data.БитыАктивностиПолей,
                Emergency = data.Emergency,
                Время = data.Время,
                ВремяЗадержки = data.ВремяЗадержки,
                ВремяОтправления = data.ВремяОтправления,
                ВремяПрибытия = data.ВремяПрибытия,
                ВремяСледования = data.ВремяСледования,
                ВремяСтоянки = data.ВремяСтоянки,
                ДниСледования = data.ДниСледования,
                ДниСледованияAlias = data.ДниСледованияAlias ?? string.Empty,
                Дополнение = data.Дополнение,
                ИменаФайлов = data.ИменаФайлов,
                ИспользоватьДополнение = data.ИспользоватьДополнение,
                КоличествоПовторений = data.КоличествоПовторений,
                НазваниеПоезда = data.НазваниеПоезда ?? string.Empty,
                НазванияТабло = data.НазванияТабло,
                Направление = data.Направление,
                НомерПоезда = data.НомерПоезда ?? string.Empty,
                НомерПоезда2 = data.НомерПоезда2 ?? string.Empty,
                НомерПути = data.НомерПути ?? string.Empty,
                НомерПутиБезАвтосброса = data.НомерПутиБезАвтосброса ?? string.Empty,
                НумерацияПоезда = data.НумерацияПоезда,
                ОжидаемоеВремя = data.ОжидаемоеВремя,
                Описание = data.Описание ?? string.Empty,
                ОписаниеСостоянияКарточки = data.ОписаниеСостоянияКарточки,
                Примечание = data.Примечание ?? string.Empty,
                РазрешениеНаОтображениеПути = data.РазрешениеНаОтображениеПути,
                Состояние = data.Состояние,
                СостояниеКарточки = data.СостояниеКарточки,
                СостояниеОтображения = data.СостояниеОтображения,
                СтанцияНазначения = data.СтанцияНазначения ?? string.Empty,
                СтанцияОтправления = data.СтанцияОтправления ?? string.Empty,
                ТаймерПовторения = data.ТаймерПовторения,
                IdТипПоезда = data.ТипПоезда.Id,
                ТипСообщения = data.ТипСообщения,
                ФиксированноеВремяОтправления = data.ФиксированноеВремяОтправления,
                ФиксированноеВремяПрибытия = data.ФиксированноеВремяПрибытия,
                ШаблонВоспроизведенияСообщений = data.ШаблонВоспроизведенияСообщений,
            };
        }


        public static SoundRecord MapSoundRecordDb2SoundRecord(SoundRecordDb data)
        {
            return new SoundRecord
            {
                Id = data.Id,
                Автомат = data.Автомат,
                Активность = data.Активность,
                БитыАктивностиПолей = data.БитыАктивностиПолей,
                Emergency = data.Emergency,
                Время = data.Время,
                ВремяЗадержки = data.ВремяЗадержки,
                ВремяОтправления = data.ВремяОтправления,
                ВремяПрибытия = data.ВремяПрибытия,
                ВремяСледования = data.ВремяСледования,
                ВремяСтоянки = data.ВремяСтоянки,
                ДниСледования = data.ДниСледования,
                ДниСледованияAlias = data.ДниСледованияAlias ?? string.Empty,
                Дополнение = data.Дополнение,
                ИменаФайлов = data.ИменаФайлов,
                ИспользоватьДополнение = data.ИспользоватьДополнение,
                КоличествоПовторений = data.КоличествоПовторений,
                НазваниеПоезда = data.НазваниеПоезда ?? string.Empty,
                НазванияТабло = data.НазванияТабло,
                Направление = data.Направление,
                НомерПоезда = data.НомерПоезда ?? string.Empty,
                НомерПоезда2 = data.НомерПоезда2 ?? string.Empty,
                НомерПути = data.НомерПути ?? string.Empty,
                НомерПутиБезАвтосброса = data.НомерПутиБезАвтосброса ?? string.Empty,
                НумерацияПоезда = data.НумерацияПоезда,
                ОжидаемоеВремя = data.ОжидаемоеВремя,
                Описание = data.Описание ?? string.Empty,
                ОписаниеСостоянияКарточки = data.ОписаниеСостоянияКарточки,
                Примечание = data.Примечание ?? string.Empty,
                РазрешениеНаОтображениеПути = data.РазрешениеНаОтображениеПути,
                Состояние = data.Состояние,
                СостояниеКарточки = data.СостояниеКарточки,
                СостояниеОтображения = data.СостояниеОтображения,
                СтанцияНазначения = data.СтанцияНазначения ?? string.Empty,
                СтанцияОтправления = data.СтанцияОтправления ?? string.Empty,
                ТаймерПовторения = data.ТаймерПовторения,
                //TODO: использовать TrainTypeByRyleService
                //ТипПоезда = Program.TrainRules.TrainTypeRules.FirstOrDefault(r=>r.Id == data.IdТипПоезда),
                ТипСообщения = data.ТипСообщения,
                ФиксированноеВремяОтправления = data.ФиксированноеВремяОтправления,
                ФиксированноеВремяПрибытия = data.ФиксированноеВремяПрибытия,
                ШаблонВоспроизведенияСообщений = data.ШаблонВоспроизведенияСообщений,
            };
        }


        public static SoundRecordChange SoundRecordChangesDb2SoundRecordChanges(SoundRecordChangesDb data)
        {      
             return new SoundRecordChange
             {
                 ScheduleId = data.ScheduleId,
                 Rec = MapSoundRecordDb2SoundRecord(data.Rec),
                 NewRec = MapSoundRecordDb2SoundRecord(data.NewRec),
                 TimeStamp = data.TimeStamp,
                 UserInfo= data.UserInfo,
                 CauseOfChange = data.CauseOfChange
             };
        }


        public static SoundRecordChangesDb SoundRecordChanges2SoundRecordChangesDb(SoundRecordChange data)
        {
            return new SoundRecordChangesDb
            {
                ScheduleId = data.ScheduleId,
                Rec = MapSoundRecord2SoundRecordDb(data.Rec),
                NewRec = MapSoundRecord2SoundRecordDb(data.NewRec),
                TimeStamp = data.TimeStamp,
                UserInfo= data.UserInfo,
                CauseOfChange = data.CauseOfChange
            };
        }
    }
}