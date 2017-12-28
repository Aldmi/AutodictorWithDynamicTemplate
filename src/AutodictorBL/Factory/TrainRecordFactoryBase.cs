using AutodictorBL.Builder.TrainRecordBuilder;
using Domain.Entitys;


namespace AutodictorBL.Factory
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