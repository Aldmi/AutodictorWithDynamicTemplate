using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using Force.DeepCloner;


namespace DAL.InMemory.Repository
{
    /// <summary>
    /// Хранение в памяти списка расписания. 
    /// При отдаче элементов происходит копирование через DeepClone().
    /// </summary>
    public class InMemoryTrainRecRepository : ITrainTableRecRepository, IDisposable
    {
        private readonly string _key;
        private  List<TrainTableRec> TrainTableRecs { get; set; }




        #region ctor

        public InMemoryTrainRecRepository(string key)
        {
            _key = key;
            TrainTableRecs = new List<TrainTableRec>();
        }

        #endregion




        #region Methode

        public TrainTableRec GetById(int id)
        {
            var findItem = TrainTableRecs.FirstOrDefault(t => t.Id == id);
            return findItem.DeepClone();
        }


        public IEnumerable<TrainTableRec> List()
        {
            //DEBUG----------
            //var time = TrainTableRecs.FirstOrDefault()?.ActionTrains.FirstOrDefault(at => at.Id == 5)?.Time;
            //var cloneTime = time.DeepClone();

            //var firstTrainRec = TrainTableRecs.FirstOrDefault();
            //var trainTableRecClone = firstTrainRec.DeepClone();
            //DEBUG----------

            return TrainTableRecs.Select(itemRec=> itemRec.DeepClone()).ToList();
        }


        public IEnumerable<TrainTableRec> List(Expression<Func<TrainTableRec, bool>> predicate)
        {
            return TrainTableRecs.Where(predicate.Compile()).Select(itemRec => itemRec.DeepClone());
        }


        public void Add(TrainTableRec entity)
        {
            TrainTableRecs.Add(entity);
        }


        public void AddRange(IEnumerable<TrainTableRec> entitys)
        {
            TrainTableRecs.AddRange(entitys);
        }


        public void Delete(TrainTableRec entity)
        {
            TrainTableRecs.Remove(entity);
        }


        public void Delete(Expression<Func<TrainTableRec, bool>> predicate)
        {
            TrainTableRecs.RemoveAll(predicate.Compile().Invoke);
        }


        public void Edit(TrainTableRec entity)
        {
           var findItem= GetById(entity.Id);
           if (findItem != null)
           {
               var index = TrainTableRecs.IndexOf(findItem);
               TrainTableRecs[index] = entity;
           }
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            TrainTableRecs.Clear();
            TrainTableRecs = null;
        }

        #endregion
    }

}