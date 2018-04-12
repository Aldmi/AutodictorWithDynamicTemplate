using System.Linq;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public interface ITrainRecBuilder
    {
        TrainRecBuilderFluent SetDefaultMain(int newId);
        TrainRecBuilderFluent SetDefaultDaysOfGoing();
        TrainRecBuilderFluent SetDefaultTrainTypeAndActionsAndEmergency();
        TrainRecBuilderFluent SetActionTrainsByType(TrainTypeByRyle trainTypeByRyle);
        TrainRecBuilderFluent SetActionTrainsByTypeId(int typeId);
        TrainRecBuilderFluent SetEmergencysByType(TrainTypeByRyle trainTypeByRyle);
        TrainTableRec Build();
    }
}