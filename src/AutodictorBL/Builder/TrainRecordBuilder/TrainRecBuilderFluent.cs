﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutodictorBL.DataAccess;
using DAL.Abstract.Entitys;
using Force.DeepCloner;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public class TrainRecBuilderFluent : ITrainRecBuilder
    {
        private readonly TrainRecService _trainRecService;



        #region prop

        private TrainTableRec TrainTableRec { get; set; }

        #endregion




        #region ctor

        public TrainRecBuilderFluent(TrainRecService trainRecService)
        {
            if(trainRecService == null)
                throw new ArgumentNullException("trainRecService не может быть Null");

            _trainRecService = trainRecService;
        }

        #endregion




        #region Methode

        public TrainRecBuilderFluent SetDefaultMain(int newId)
        {
            TrainTableRec = TrainTableRec ?? new TrainTableRec();

            TrainTableRec.Id = newId;
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
            TrainTableRec.Примечание = "";
            TrainTableRec.Addition = "";
            TrainTableRec.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = false,
                ["табло"] = false
            };
            TrainTableRec.Автомат = true;
            TrainTableRec.IsScoreBoardOutput = false;
            TrainTableRec.IsSoundOutput = true;

            return this;
        }


        public TrainRecBuilderFluent SetDefaultDaysOfGoing()
        {
            TrainTableRec = TrainTableRec ?? new TrainTableRec();
            TrainTableRec.ВремяНачалаДействияРасписания = DateTime.MinValue;
            TrainTableRec.ВремяОкончанияДействияРасписания = DateTime.MaxValue;
            TrainTableRec.Days = string.Empty;
            TrainTableRec.DaysAlias = string.Empty;

            return this;
        }


        public TrainRecBuilderFluent SetDefaultTrainTypeAndActionsAndEmergency()
        {
            TrainTableRec = TrainTableRec ?? new TrainTableRec();
            TrainTableRec.TrainTypeByRyle = _trainRecService.GetTrainTypeByRyles().FirstOrDefault();
            TrainTableRec.ActionTrains = new List<ActionTrain>();
            TrainTableRec.EmergencyTrains = TrainTableRec.TrainTypeByRyle?.EmergencyTrains.DeepClone();

            return this;
        }


        public TrainRecBuilderFluent SetActionTrainsByType(TrainTypeByRyle trainTypeByRyle)
        {
            trainTypeByRyle = trainTypeByRyle ?? TrainTableRec.TrainTypeByRyle;
            TrainTableRec.ActionTrains = trainTypeByRyle.ActionTrains.Where(at => at.IsActiveBase).ToList();

            return this;
        }

        public TrainRecBuilderFluent SetActionTrainsByTypeId(int typeId)
        {
           var trainTypeByRyle= _trainRecService.GetTrainTypeByRyleById(typeId);
           if(trainTypeByRyle == null)
              throw new ArgumentException($"Элемент с {typeId} не найден");

            TrainTableRec.ActionTrains = trainTypeByRyle.ActionTrains.Where(at => at.IsActiveBase).ToList();

            return this;
        }


        public TrainRecBuilderFluent SetEmergencysByType(TrainTypeByRyle trainTypeByRyle)
        {
            trainTypeByRyle = trainTypeByRyle ?? TrainTableRec.TrainTypeByRyle;
            TrainTableRec.EmergencyTrains = trainTypeByRyle.EmergencyTrains.Where(at => at.IsActiveBase).ToList();

            return this;
        }



        public TrainTableRec Build()
        {
            return TrainTableRec;
        }

        #endregion

    }
}