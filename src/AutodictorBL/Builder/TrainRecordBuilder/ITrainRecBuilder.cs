using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public interface ITrainRecBuilder
    {
        ITrainRecBuilder SetDefault();
        ITrainRecBuilder SetExternalData(UniversalInputType uit);
        ITrainRecBuilder SetDefaultDaysOfGoing();
        ITrainRecBuilder SetDefaultTrainTypeAndActions();
        ITrainRecBuilder SetActionTrainsByType(TrainTypeByRyle trainTypeByRyle);
        ITrainRecBuilder SetActionTrainsByTypeId(int typeId);
        ITrainRecBuilder SetEmergencysByType(TrainTypeByRyle trainTypeByRyle);
        ITrainRecBuilder SetDirectionByName(string name);
        ITrainRecBuilder SetStationsByCodeEsr(int codeEsrStationArrival, int codeEsrStationDeparture);
        ITrainRecBuilder SetStationsById(int idStationArrival, int idStationDeparture);
        ITrainRecBuilder SetAllByTypeId(int typeId);
        ITrainRecBuilder SetEmergencyByTrainEvent(Event? trainEvent = null);
        TrainTableRec Build();
    }
}