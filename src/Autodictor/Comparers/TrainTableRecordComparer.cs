using System.Collections.Generic;
using Domain.Entitys;

namespace MainExample.Comparers
{
    /// <summary>
    /// Компаратор для 
    /// </summary>
    public class TrainTableRecordComparer4NumberTrainAndDirection : IEqualityComparer<TrainTableRecord>
    {
        public bool Equals(TrainTableRecord x, TrainTableRecord y)
        {
            return x.Num == y.Num;// &&
                  // x.Num2 == y.Num2 &&
                 //  x.Direction == y.Direction;
        }

        public int GetHashCode(TrainTableRecord obj)
        {
            return obj.GetHashCode();
        }
    }
}