﻿using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public interface ITrainRecBuilder
    {
        ITrainRecBuilder SetDefaultMain(int newId);
        ITrainRecBuilder SetExternalData(UniversalInputType uit);
        ITrainRecBuilder SetDefaultDaysOfGoing();
        ITrainRecBuilder SetDefaultTrainTypeAndActionsAndEmergency();
        ITrainRecBuilder SetActionTrainsByType(TrainTypeByRyle trainTypeByRyle);
        ITrainRecBuilder SetActionTrainsByTypeId(int typeId);
        ITrainRecBuilder SetEmergencysByType(TrainTypeByRyle trainTypeByRyle);
        TrainTableRec Build();
    }
}