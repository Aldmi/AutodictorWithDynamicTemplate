﻿using AutodictorBL.Builder.TrainRecordBuilder;
using DAL.Abstract.Entitys;


namespace AutodictorBL.Factory.TrainRecordFactory
{
    public abstract class TrainRecordFactoryBase
    {
        protected readonly TrainRecordBuilderBase Builder;



        protected TrainRecordFactoryBase(TrainRecordBuilderBase builder)
        {
            Builder = builder;
        }


        public abstract TrainTableRecord Construct();
    }
}