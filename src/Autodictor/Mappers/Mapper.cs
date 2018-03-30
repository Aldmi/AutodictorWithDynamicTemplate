using System;
using System.Collections.Generic;
using System.Linq;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Model;
using DAL.Abstract.Entitys;
using Force.DeepCloner;
using MainExample.Entites;


namespace MainExample.Mappers
{
    public static class Mapper
    {
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



        public static SoundRecord MapTrainTableRecord2SoundRecord(TrainTableRec config, DateTime day, int id)
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
            record.БитыНештатныхСитуаций = 0x00;
            record.ТаймерПовторения = 0;
            record.РазрешениеНаОтображениеПути = PathPermissionType.ИзФайлаНастроек;

            record.ИменаФайлов = new string[0];
            record.ФиксированноеВремяПрибытия = null;
            record.ФиксированноеВремяОтправления = null;

            record.СтанцияОтправления = config.StationDepart != null ? config.StationDepart.NameRu : string.Empty;
            record.СтанцияНазначения = config.StationArrival != null ? config.StationArrival.NameRu : string.Empty;

            record.ВыводНаТабло = config.IsScoreBoardOutput;
            record.ВыводЗвука= config.IsSoundOutput;


            DateTime времяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime времяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);
            record.ВремяПрибытия = DateTime.Now;
            record.ВремяОтправления = DateTime.Now;
            record.ОжидаемоеВремя = DateTime.Now;
            record.ВремяСледования = null;
            record.ВремяЗадержки = null;
            byte номерСписка = 0x00;

            if (config.ArrivalTime.HasValue)
            {
                времяПрибытия = config.ArrivalTime.Value;
                record.ВремяПрибытия = времяПрибытия;
                record.ОжидаемоеВремя = времяПрибытия;
                номерСписка |= 0x04;
            }

            if (config.DepartureTime.HasValue)
            {
                времяОтправления = config.DepartureTime.Value;
                record.ВремяОтправления = времяОтправления;
                record.ОжидаемоеВремя = времяОтправления;
                номерСписка |= 0x10;
            }
   
            record.ВремяСледования = config.FollowingTime;
                
            
            //ТРАНЗИТ
            record.ВремяСтоянки = null;
            if (номерСписка == 0x14)
            {
                if (времяОтправления < времяПрибытия)          
                {
                    record.ВремяПрибытия = времяПрибытия.AddDays(-1);
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
            record.СписокФормируемыхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();//УБРАТЬ


            int idActDyn = 1;
            record.ActionTrainDynamiсList = new List<ActionTrainDynamic>();
            foreach (var actionTrain in config.ActionTrains)
            {
                if (actionTrain.Time.IsDeltaTimes) //Указанны временные смещения
                {
                    foreach (var time in actionTrain.Time.DeltaTimes) //копируем шаблон для каждого временного смещения
                    {
                        var newActionTrain= actionTrain.DeepClone(); //COPY
                        newActionTrain.Time.DeltaTimes = new List<int> {time};
                        var actDyn= new ActionTrainDynamic
                        {
                            Id = idActDyn++,
                            SoundRecordId = record.Id,
                            Activity = true,
                            PriorityMain = Priority.Midlle,
                            SoundRecordStatus = SoundRecordStatus.ОжиданиеВоспроизведения,
                            ActionTrain = newActionTrain
                        };
                        record.ActionTrainDynamiсList.Add(actDyn);
                    }
                }
                else                                   //Указан циклический повтор
                {
                    var newActionTrain= actionTrain.DeepClone(); //COPY
                    var actDyn = new ActionTrainDynamic
                    {
                        Id = idActDyn++,
                        SoundRecordId = record.Id,
                        Activity = true,
                        PriorityMain = Priority.Midlle,
                        SoundRecordStatus = SoundRecordStatus.ОжиданиеВоспроизведения,
                        ActionTrain = newActionTrain
                    };
                    record.ActionTrainDynamiсList.Add(actDyn);
                }
            }




            string[] шаблонОповещения = record.ШаблонВоспроизведенияСообщений.Split(':');
            if ((шаблонОповещения.Length % 3) == 0)
            {
                bool активностьШаблоновДанногоПоезда = true;//record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.АвтФормСообщНаПассажирскийПоезд;
                //if (record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд) активностьШаблоновДанногоПоезда = true;
                //if (record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.АвтФормСообщНаСкоростнойПоезд) активностьШаблоновДанногоПоезда = true;
                //if (record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.АвтФормСообщНаСкорыйПоезд) активностьШаблоновДанногоПоезда = true;
                //if (record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.АвтФормСообщНаЛасточку) активностьШаблоновДанногоПоезда = true;
                //if (record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.АвтФормСообщНаФирменный) активностьШаблоновДанногоПоезда = true;
                //if (record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.АвтФормСообщНаРЭКС) активностьШаблоновДанногоПоезда = true;

                int indexШаблона = 0;
                for (int i = 0; i < шаблонОповещения.Length / 3; i++)
                {
                    bool наличиеШаблона = false;
                    string шаблон = "";
                    PriorityPrecise приоритетШаблона= PriorityPrecise.Zero;
                    foreach (var item in DynamicSoundForm.DynamicSoundRecords)
                        if (item.Name == шаблонОповещения[3 * i + 0])
                        {
                            наличиеШаблона = true;
                            шаблон = item.Message;
                            приоритетШаблона = item.PriorityTemplate;
                            break;
                        }

                    if (наличиеШаблона == true)
                    {
                        var привязкаВремени = 0;
                        int.TryParse(шаблонОповещения[3 * i + 2], out привязкаВремени);

                        string[] времяАктивацииШаблона = шаблонОповещения[3 * i + 1].Replace(" ", "").Split(',');
                        foreach (var время in времяАктивацииШаблона)
                        {
                            int времяСмещения = 0;
                            if ((int.TryParse(время, out времяСмещения)) == true)
                            {
                                СостояниеФормируемогоСообщенияИШаблон новыйШаблон;
                                новыйШаблон.Id = indexШаблона++;
                                новыйШаблон.SoundRecordId = record.Id;
                                новыйШаблон.Активность = активностьШаблоновДанногоПоезда;
                                новыйШаблон.ПриоритетГлавный = Priority.Midlle;
                                новыйШаблон.ПриоритетВторостепенный = приоритетШаблона;
                                новыйШаблон.Воспроизведен = false;
                                новыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                новыйШаблон.ВремяСмещения = времяСмещения;
                                новыйШаблон.НазваниеШаблона = шаблонОповещения[3 * i + 0];
                                новыйШаблон.Шаблон = шаблон;
                                новыйШаблон.ПривязкаКВремени = привязкаВремени;
                                новыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Rus, NotificationLanguage.Eng };  //TODO:Брать из ШаблонОповещения полученого из TrainTable.

                                record.СписокФормируемыхСообщений.Add(новыйШаблон);
                            }
                        }
                    }
                }
            }

            record.СписокНештатныхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();
            record.AplyIdTrain();

            return record;
        }



        public static UniversalInputType MapTrainTableRecord2UniversalInputType(TrainTableRecord t)
        {
            Func<string, string, DateTime> timePars = (arrival, depart) =>
            {
                DateTime outData;
                if (DateTime.TryParse(arrival, out outData))
                    return outData;

                if (DateTime.TryParse(depart, out outData))
                    return outData;

                return DateTime.MinValue;
            };

            Func<string, string, string> eventPars = (arrivalTime, departTime) =>
            {
                if ((!string.IsNullOrEmpty(arrivalTime)) && (!string.IsNullOrEmpty(departTime)))
                {
                    return "СТОЯНКА";
                }

                if (!string.IsNullOrEmpty(arrivalTime))
                {
                    return "ПРИБ.";
                }

                if (!string.IsNullOrEmpty(departTime))
                {
                    return "ОТПР.";
                }

                return String.Empty;
            };


            Func<string, string, Dictionary<string, DateTime>> transitTimePars = (arrivalTime, departTime) =>
            {
                var transitTime = new Dictionary<string, DateTime>();
                if ((!string.IsNullOrEmpty(arrivalTime)) && (!string.IsNullOrEmpty(departTime)))
                {
                    transitTime["приб"] = timePars(arrivalTime, String.Empty);
                    transitTime["отпр"] = timePars(departTime, String.Empty);
                }

                return transitTime;
            };


            Func<string, string, Station> stationsPars2 = (station, direction) =>
            {
                var emptyStation= new Station { NameRu = string.Empty, NameEng = string.Empty, NameCh = string.Empty };
                if (string.IsNullOrEmpty(direction) || string.IsNullOrEmpty(station))
                {
                    return emptyStation;
                }

                var stationDir = Program.ПолучитьСтанциюНаправления(direction, station);
                if (stationDir == null)
                    return emptyStation;

                return stationDir;
            };


            TimeSpan stopTime;
            UniversalInputType uit = new UniversalInputType
            {
                IsActive = t.Active,
                Id = t.Id,
                Event = eventPars(t.ArrivalTime, t.DepartureTime),
                TypeTrain = t.TrainTypeByRyle.NameRu,
                Note = t.Примечание, //C остановками: ...
                //PathNumber = ПолучитьНомерПутиПоДнямНедели(t),                    //TODO:  ?????
                VagonDirection = (VagonDirection)t.TrainPathDirection,
                NumberOfTrain = t.Num,
                Stations = t.Name,
                DirectionStation = t.Direction,
                StationDeparture = stationsPars2(t.StationDepart, t.Direction),
                StationArrival = stationsPars2(t.StationArrival, t.Direction),
                Time = timePars(t.ArrivalTime, t.DepartureTime),
                TransitTime = transitTimePars(t.ArrivalTime, t.DepartureTime),
                DelayTime = null,
                StopTime = (TimeSpan?)(TimeSpan.TryParse(t.StopTime, out stopTime) ? (ValueType)stopTime : null),
                ExpectedTime = timePars(t.ArrivalTime, t.DepartureTime),
                DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(t.Days).ПолучитьСтрокуОписанияРасписания(),
                DaysFollowingAlias = t.DaysAlias,
                Addition = t.Addition,
                SendingDataLimit = t.IsScoreBoardOutput,
                Command = Command.None,
                EmergencySituation = 0x00
            };

            return uit;
        }



        public static TrainTableRecord MapUniversalInputType2TrainTableRecord(UniversalInputType uit)
        {
            var tableRec= new TrainTableRecord();
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

            var defaultStation = ExchangeModel.NameRailwayStation;
            var cтанцияОтправления = Program.ПолучитьСтанциюНаправления(data.Направление, data.СтанцияОтправления) ?? defaultStation;
            var cтанцияНазначения = Program.ПолучитьСтанциюНаправления(data.Направление, data.СтанцияНазначения) ?? defaultStation;


            UniversalInputType mapData;
            if (isShow)
            {
                mapData = new UniversalInputType
                {
                    Id = data.Id,
                    ScheduleId = data.IdTrain.ScheduleId,
                    IsActive = data.Активность,
                    NumberOfTrain = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? номерПоезда : "   ",
                    VagonDirection = (VagonDirection)data.НумерацияПоезда,
                    ChangeVagonDirection = data.СменнаяНумерацияПоезда,
                    PathNumber = номерПути,
                    PathNumberWithoutAutoReset = data.НомерПутиБезАвтосброса,
                    Event = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? actStr : "   ",
                    Time = time,
                    TransitTime = transitTimes,
                    DelayTime = data.ВремяЗадержки,
                    ExpectedTime = data.ОжидаемоеВремя,
                    StopTime = data.ВремяСтоянки,
                    Stations = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.НазваниеПоезда : "   ",
                    DirectionStation = data.Направление,

                    StationDeparture = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияОтправления : new Station(),
                    StationArrival = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияНазначения : new Station(),

                    Note = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.Примечание : "   ",
                    TypeTrain = data.ТипПоезда.NameRu,
                    DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(data.ДниСледования).ПолучитьСтрокуОписанияРасписания(),
                    DaysFollowingAlias = data.ДниСледованияAlias,
                    Addition = (data.ИспользоватьДополнение["табло"]) ? data.Дополнение : string.Empty,
                    SendingDataLimit = data.ВыводНаТабло,
                    Command = command,
                    EmergencySituation = data.БитыНештатныхСитуаций
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
                    VagonDirection = (VagonDirection)data.НумерацияПоезда,
                    ChangeVagonDirection = data.СменнаяНумерацияПоезда,
                    PathNumber = номерПути,
                    PathNumberWithoutAutoReset = data.НомерПутиБезАвтосброса,
                    Event = actStr,
                    Time = time,
                    TransitTime = transitTimes,
                    DelayTime = data.ВремяЗадержки,
                    ExpectedTime = data.ОжидаемоеВремя,
                    StopTime = data.ВремяСтоянки,
                    Stations = data.НазваниеПоезда,
                    DirectionStation = data.Направление,
                    StationDeparture =  cтанцияОтправления,  //(data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияОтправления : new Station(),
                    StationArrival = cтанцияНазначения,     // (data.СостояниеОтображения != TableRecordStatus.Очистка) ? cтанцияНазначения : new Station(), 
                    Note = data.Примечание,
                    TypeTrain = data.ТипПоезда.NameRu,
                    DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(data.ДниСледования).ПолучитьСтрокуОписанияРасписания(),
                    DaysFollowingAlias = data.ДниСледованияAlias,
                    Addition = (data.ИспользоватьДополнение["табло"]) ? data.Дополнение : string.Empty,
                    SendingDataLimit = data.ВыводНаТабло,
                    Command = command,
                    EmergencySituation = data.БитыНештатныхСитуаций
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
                БитыНештатныхСитуаций = data.БитыНештатныхСитуаций,
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
                СписокФормируемыхСообщений = data.СписокФормируемыхСообщений.Select(СостояниеФормируемогоСообщенияИШаблон2СостояниеФормируемогоСообщенияИШаблонDb).ToList(),
               // СписокНештатныхСообщений = data.СписокНештатныхСообщений.Select(СостояниеФормируемогоСообщенияИШаблон2СостояниеФормируемогоСообщенияИШаблонDb).ToList()
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
                БитыНештатныхСитуаций = data.БитыНештатныхСитуаций,
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
                СписокФормируемыхСообщений = data.СписокФормируемыхСообщений.Select(СостояниеФормируемогоСообщенияИШаблонDb2СостояниеФормируемогоСообщенияИШаблон).ToList(),
                //СписокНештатныхСообщений = data.СписокНештатныхСообщений.Select(СостояниеФормируемогоСообщенияИШаблонDb2СостояниеФормируемогоСообщенияИШаблон).ToList()
            };
        }



        public static СостояниеФормируемогоСообщенияИШаблонDb СостояниеФормируемогоСообщенияИШаблон2СостояниеФормируемогоСообщенияИШаблонDb(СостояниеФормируемогоСообщенияИШаблон data)
        {
            return new СостояниеФормируемогоСообщенияИШаблонDb
            {
                Id = data.Id,
                SoundRecordId = data.SoundRecordId,
                Активность = data.Активность,
                НазваниеШаблона = data.НазваниеШаблона,
                ВремяСмещения = data.ВремяСмещения,
                ЯзыкиОповещения = data.ЯзыкиОповещения,
                Воспроизведен = data.Воспроизведен,
                ПривязкаКВремени = data.ПривязкаКВремени,
                Приоритет = data.ПриоритетГлавный,
                СостояниеВоспроизведения = data.СостояниеВоспроизведения,
                Шаблон = data.Шаблон
            };
        }



        public static СостояниеФормируемогоСообщенияИШаблон СостояниеФормируемогоСообщенияИШаблонDb2СостояниеФормируемогоСообщенияИШаблон(СостояниеФормируемогоСообщенияИШаблонDb data)
        {
            return new СостояниеФормируемогоСообщенияИШаблон
            {
                Id = data.Id,
                SoundRecordId = data.SoundRecordId,
                Активность = data.Активность,
                НазваниеШаблона = data.НазваниеШаблона,
                ВремяСмещения = data.ВремяСмещения,
                ЯзыкиОповещения = data.ЯзыкиОповещения,
                Воспроизведен = data.Воспроизведен,
                ПривязкаКВремени = data.ПривязкаКВремени,
                ПриоритетГлавный = data.Приоритет,
                СостояниеВоспроизведения = data.СостояниеВоспроизведения,
                Шаблон = data.Шаблон
            };
        }



        public static SoundRecordChanges SoundRecordChangesDb2SoundRecordChanges(SoundRecordChangesDb data)
        {      
             return new SoundRecordChanges
             {
                 ScheduleId = data.ScheduleId,
                 Rec = MapSoundRecordDb2SoundRecord(data.Rec),
                 NewRec = MapSoundRecordDb2SoundRecord(data.NewRec),
                 TimeStamp = data.TimeStamp,
                 UserInfo= data.UserInfo,
                 CauseOfChange = data.CauseOfChange
             };
        }



        public static SoundRecordChangesDb SoundRecordChanges2SoundRecordChangesDb(SoundRecordChanges data)
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