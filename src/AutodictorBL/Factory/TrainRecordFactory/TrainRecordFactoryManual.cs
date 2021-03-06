﻿using AutodictorBL.Builder.TrainRecordBuilder;
using AutodictorBL.Factory.TrainRecordFactory;
using DAL.Abstract.Entitys;


namespace AutodictorBL.Factory.TrainRecordFactory
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