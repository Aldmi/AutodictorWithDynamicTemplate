using AutodictorBL.Builder.TrainRecordBuilder;
using Domain.Entitys;


namespace AutodictorBL.Factory
{
    public class TrainRecordFactoryManual : TrainRecordFactoryBase
    {
        #region prop

        public TrainRecordFactoryManual(TrainRecordBuilderBase builder) : base(builder)
        {
        }

        #endregion




        public override TrainTableRecord Construct()
        {
            Builder.BuildDaysFollowing();
            Builder.BuildSoundTemplateByRules();

            return Builder.GetTrainRec;
        }
    }
}