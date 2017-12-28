using Domain.Entitys;

namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public abstract class TrainRecordBuilderBase
    {
        #region prop

        protected TrainTableRecord TrainTableRecord;
        public TrainTableRecord GetTrainRec => TrainTableRecord;

        #endregion




        #region Abstract

        public abstract void BuildBase();
        public abstract void BuildDaysFollowing();
        public abstract void BuildSoundTemplateByRules();

        #endregion
    }
}