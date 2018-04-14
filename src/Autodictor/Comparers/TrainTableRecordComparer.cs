using System.Collections.Generic;
using DAL.Abstract.Entitys;


namespace MainExample.Comparers
{
    /// <summary>
    /// Компаратор для 
    /// </summary>
    public class TrainTableRecordComparer4NumberTrainAndDirection : IEqualityComparer<TrainTableRec>
    {
        public bool Equals(TrainTableRec x, TrainTableRec y)
        {
            return x.Num == y.Num;// &&
                  // x.Num2 == y.Num2 &&
                 //  x.Direction == y.Direction;
        }

        public int GetHashCode(TrainTableRec obj)
        {
            return obj.GetHashCode();
        }
    }
}