using System;
using AutodictorBL.Services.DataAccessServices;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public class TrainRecBuilderRemoteCisFluent : ITrainRecBuilder
    {

        private readonly TrainRecService _trainRecService;



        #region prop

        private TrainTableRec TrainTableRec { get; set; }

        #endregion




        #region ctor

        public TrainRecBuilderRemoteCisFluent(TrainRecService trainRecService)
        {
            if (trainRecService == null)
                throw new ArgumentNullException(@"trainRecService не может быть Null");

            _trainRecService = trainRecService;
        }

        #endregion





        #region Methode

        public ITrainRecBuilder SetDefaultMain(int newId)
        {
            return this;
        }

        public ITrainRecBuilder SetExternalData(UniversalInputType uit)
        {
            return this;
        }

        public ITrainRecBuilder SetDefaultDaysOfGoing()
        {
            return this;
        }

        public ITrainRecBuilder SetDefaultTrainTypeAndActionsAndEmergency()
        {
            return this;
        }

        public ITrainRecBuilder SetActionTrainsByType(TrainTypeByRyle trainTypeByRyle)
        {
            return this;
        }

        public ITrainRecBuilder SetActionTrainsByTypeId(int typeId)
        {
            return this;
        }

        public ITrainRecBuilder SetEmergencysByType(TrainTypeByRyle trainTypeByRyle)
        {
            return this;
        }

        public ITrainRecBuilder SetDirectionByName(string name)
        {
            throw new NotImplementedException();
        }

        public ITrainRecBuilder SetStationsByCodeEsr(int codeEsrStationArrival, int codeEsrStationDeparture)
        {
            throw new NotImplementedException();
        }

        public ITrainRecBuilder SetStationsById(int idStationArrival, int idStationDeparture)
        {
            throw new NotImplementedException();
        }

        public ITrainRecBuilder SetAllByTypeId(int typeId)
        {
            throw new NotImplementedException();
        }

        public TrainTableRec Build()
        {
            return TrainTableRec;
        }

        #endregion
    }
}