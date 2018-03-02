using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace AutodictorBL.DataAccess
{
    public class TrainRecService : IDisposable
    {
        #region field

        private readonly ITrainTableRecRepository _repLocalMain;
        private readonly ITrainTableRecRepository _repRemoteCis;
        private readonly ITrainTypeByRyleRepository _repTypeByRyle;

        #endregion




        #region prop

        public TrainRecType SourceLoad { get; set; }

        #endregion




        #region ctor

        public TrainRecService(ITrainTableRecRepository repLocalMain, ITrainTableRecRepository repRemoteCis, ITrainTypeByRyleRepository repTypeByRyle)
        {
            _repLocalMain = repLocalMain;
            _repRemoteCis = repRemoteCis;
            _repTypeByRyle = repTypeByRyle;
        }

        #endregion




        #region Methode

        public TrainTableRec GetById(int id)
        {
            var rep= (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            return rep.GetById(id);
        }


        public IEnumerable<TrainTableRec> GetAll()
        {
            var rep= (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            return rep.List();
        }


        public void ReWriteAll(IEnumerable<TrainTableRec> list)
        {
            var rep= (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.Delete(t=> true);
            rep.AddRange(list);
        }


        public void DeleteItem(TrainTableRec item)
        {
            var rep= (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.Delete(item);
        }


        public void DeleteItem(Expression<Func<TrainTableRec, bool>> predicate)
        {
            var rep = (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.Delete(predicate);
        }

        #endregion



        public void Dispose()
        {
            
        }
    }
}