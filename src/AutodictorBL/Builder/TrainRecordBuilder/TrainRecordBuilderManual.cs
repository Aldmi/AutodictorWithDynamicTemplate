using System;
using System.Linq;
using DAL.Abstract.Entitys;


namespace AutodictorBL.Builder.TrainRecordBuilder
{
    public class TrainRecordBuilderManual : TrainRecordBuilderBase
    {
        #region prop

        private string DaysFollowingFormat { get; }
        private TrainTypeByRyle Rule { get; }

        #endregion




        #region ctor

        public TrainRecordBuilderManual(TrainTableRecord trainTableBase, string daysFollowingFormat, TrainTypeByRyle rule)
        {
            TrainTableRecord = trainTableBase;
            DaysFollowingFormat = daysFollowingFormat;
            Rule = rule;
        }

        #endregion




        #region Methode

        public override void BuildBase()
        {
        }


        public override void BuildDaysFollowing()
        {
            if (string.IsNullOrEmpty(DaysFollowingFormat))
                return;

            //преобразовать из строки в масив байт.
        }


        public override void BuildSoundTemplateByRules()
        {
            var rule = Rule as TrainTypeByRyle; //TODO: вынести в интрефейс нужные члены
            TrainTableRecord.ActionTrains = rule?.ActionTrains;


            //DEBUG------------------------------------------------------------------------------------------------------------
            var templateStr = string.Empty;//@"Начало посадки на пассажирский поезд:10:1:Начало посадки на скорый поезд:15:0";

            //foreach (var act in rule.ActionTrains)
            //{
            //    if (act.Time != null)
            //    {
            //        templateStr += act.Name + ":";
            //        templateStr += act.Time.DeltaTime + ":";
            //        templateStr += act.ActionType == ActionType.Arrival ? 1 : 0;
            //    }
            //}

            TrainTableRecord.SoundTemplates = templateStr;
        }

        #endregion
    }
}