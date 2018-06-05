using System;
using System.Collections.Generic;
using System.Linq;
using AutodictorBL.Services.DataAccessServices;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;
using Force.DeepCloner;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public class TrainRecBuilderFluent : ITrainRecBuilder, IDisposable
    {
        #region field

        private readonly TrainRecService _trainRecService;

        #endregion




        #region prop

        private TrainTableRec TrainTableRec { get; }

        #endregion




        #region ctor

        public TrainRecBuilderFluent(TrainRecService trainRecService)
        {
            if(trainRecService == null)
                throw new ArgumentNullException("trainRecService не может быть Null");

            _trainRecService = trainRecService;
            TrainTableRec = new TrainTableRec();
        }

        #endregion




        #region Methode


        /// <summary>
        /// Установить дефолтное состояние объекта
        /// </summary>
        public ITrainRecBuilder SetDefault()
        {
            TrainTableRec.Id = 0;
            TrainTableRec.Num = "";
            TrainTableRec.Num2 = "";
            TrainTableRec.Addition = "";
            TrainTableRec.Name = "";
            TrainTableRec.StationArrival = null;
            TrainTableRec.StationDepart = null;
            TrainTableRec.Direction = null;
            TrainTableRec.ArrivalTime = null;
            TrainTableRec.StopTime = null;
            TrainTableRec.DepartureTime = null;
            TrainTableRec.FollowingTime = null;
            TrainTableRec.Active = false;
            TrainTableRec.WagonsNumbering = WagonsNumbering.None;
            TrainTableRec.ChangeTrainPathDirection = false;
            TrainTableRec.TrainPathNumber = new Dictionary<WeekDays, Pathway>
            {
                [WeekDays.Постоянно] = null,
                [WeekDays.Пн] = null,
                [WeekDays.Вт] = null,
                [WeekDays.Ср] = null,
                [WeekDays.Ср] = null,
                [WeekDays.Чт] = null,
                [WeekDays.Пт] = null,
                [WeekDays.Сб] = null,
                [WeekDays.Вс] = null
            };
            TrainTableRec.PathWeekDayes = false;
            TrainTableRec.Route = null;
            TrainTableRec.Addition = "";
            TrainTableRec.UseAddition = new Dictionary<string, bool>
            {
                ["звук"] = false,
                ["табло"] = false
            };
            TrainTableRec.Automate = true;
            TrainTableRec.IsScoreBoardOutput = false;
            TrainTableRec.IsSoundOutput = true;

            TrainTableRec.StartTimeSchedule = DateTime.MinValue;
            TrainTableRec.StopTimeSchedule = DateTime.MaxValue;

            return this;
        }



        /// <summary>
        /// Установить состояние объекта из внешнего объекта UniversalInputType
        /// </summary>
        public ITrainRecBuilder SetExternalData(UniversalInputType uit)
        {
            TrainTableRec.Id = uit.Id;
            TrainTableRec.Num = uit.NumberOfTrain;
            TrainTableRec.Event = uit.Event;
            TrainTableRec.Addition = uit.Addition;
            TrainTableRec.Route = uit.Route;
            TrainTableRec.DaysFollowing = uit.DaysFollowing;
            TrainTableRec.StartTimeSchedule = uit.StartTimeSchedule;
            TrainTableRec.StopTimeSchedule = uit.StopTimeSchedule;
            TrainTableRec.ArrivalTime = uit.ArrivalTime;
            TrainTableRec.DepartureTime = uit.DepartureTime;
            TrainTableRec.WagonsNumbering = uit.WagonsNumbering;

            return this;
        }


        /// <summary>
        /// Установить состояние объекта из внешнего объекта UniversalInputType
        /// </summary>
        public ITrainRecBuilder SetDefaultDaysOfGoing()
        {
            TrainTableRec.StartTimeSchedule = DateTime.MinValue;
            TrainTableRec.StopTimeSchedule = DateTime.MaxValue;
            TrainTableRec.DaysFollowing = string.Empty;
            TrainTableRec.DaysAlias = string.Empty;

            return this;
        }


        public ITrainRecBuilder SetDefaultTrainTypeAndActionsAndEmergency()
        {
            TrainTableRec.TrainTypeByRyle = _trainRecService.GetTrainTypeByRyles().FirstOrDefault();
            TrainTableRec.ActionTrains = new List<ActionTrain>();
            TrainTableRec.EmergencyTrains = TrainTableRec.TrainTypeByRyle?.EmergencyTrains.DeepClone();

            return this;
        }


        public ITrainRecBuilder SetActionTrainsByType(TrainTypeByRyle trainTypeByRyle)
        {
            trainTypeByRyle = trainTypeByRyle ?? TrainTableRec.TrainTypeByRyle;
            TrainTableRec.ActionTrains = trainTypeByRyle.ActionTrains.Where(at => at.IsActiveBase).Select(act => act.DeepClone()).ToList();

            return this;
        }


        public ITrainRecBuilder SetActionTrainsByTypeId(int typeId)
        {
           var trainTypeByRyle= _trainRecService.GetTrainTypeByRyleById(typeId);
           if(trainTypeByRyle == null)
              throw new ArgumentException($"Элемент с {typeId} не найден");

            TrainTableRec.ActionTrains = trainTypeByRyle.ActionTrains.Where(at => at.IsActiveBase).Select(act => act.DeepClone()).ToList();

            return this;
        }


        public ITrainRecBuilder SetEmergencysByType(TrainTypeByRyle trainTypeByRyle)
        {
            trainTypeByRyle = trainTypeByRyle ?? TrainTableRec.TrainTypeByRyle;
            TrainTableRec.EmergencyTrains = trainTypeByRyle.EmergencyTrains.Where(at => at.IsActiveBase).Select(act => act.DeepClone()).ToList();

            return this;
        }


        /// <summary>
        /// Выставить свойства определяемые ТИПОМ поезда
        /// </summary>
        public ITrainRecBuilder SetAllByTypeId(int typeId)
        {
            var trainTypeByRyle = _trainRecService.GetTrainTypeByRyleById(typeId);
            if (trainTypeByRyle == null)
                throw new ArgumentException($"Элемент с {typeId} не найден");

            TrainTableRec.TrainTypeByRyle = trainTypeByRyle;
            TrainTableRec.ActionTrains = trainTypeByRyle.ActionTrains.Where(at => at.IsActiveBase).Select(act=> act.DeepClone()).ToList();
            TrainTableRec.EmergencyTrains = trainTypeByRyle.EmergencyTrains.Where(at => at.IsActiveBase).Select(act=> act.DeepClone()).ToList();

            return this;
        }


        /// <summary>
        /// Найти и установить направление по ИМЕНИ
        /// </summary>
        public ITrainRecBuilder SetDirectionByName(string name)
        {
            var direction = _trainRecService.GetDirectionByName(name);
            if (direction == null)
                throw new ArgumentException($"Направление {direction} не найдено");

            TrainTableRec.Direction = direction;

            return this;
        }


        /// <summary>
        /// Найти и установить станции ПРИБ и ОТПР по ЕСР коду
        /// </summary>
        public ITrainRecBuilder SetStationsByCodeEsr(int codeEsrStationArrival, int codeEsrStationDeparture)
        {
            if (TrainTableRec.Direction == null)
                throw new ArgumentException("Направление (Direction) не установленно");

            TrainTableRec.StationArrival= TrainTableRec.Direction.Stations.FirstOrDefault(st=> st.CodeEsr == codeEsrStationArrival);
            TrainTableRec.StationDepart= TrainTableRec.Direction.Stations.FirstOrDefault(st=> st.CodeEsr == codeEsrStationDeparture);

            return this;
        }


        /// <summary>
        /// Найти и установить станции ПРИБ и ОТПР по коду
        /// </summary>
        public ITrainRecBuilder SetStationsById(int idStationArrival, int idStationDeparture)
        {
            if (TrainTableRec.Direction == null)
                throw new ArgumentException("Направление (Direction) не установленно");

            TrainTableRec.StationArrival = TrainTableRec.Direction.Stations.FirstOrDefault(st => st.Id == idStationArrival);
            TrainTableRec.StationDepart = TrainTableRec.Direction.Stations.FirstOrDefault(st => st.Id == idStationDeparture);

            return this;
        }



        public TrainTableRec Build()
        {
            return TrainTableRec;
        }

        #endregion



        #region Disposable

        public void Dispose()
        {
         
        }

        #endregion
    }
}