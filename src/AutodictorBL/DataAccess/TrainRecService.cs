using System.Collections.Generic;
using System.Linq;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;

namespace AutodictorBL.DataAccess
{
    public class TrainRecService
    {
        private readonly ITrainTableRecRepository _repLocalMain;
        private readonly ITrainTableRecRepository _repRemoteCis;
        private readonly ITrainTypeByRyleRepository _repTypeByRyle;



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
            var rep = (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            return rep.GetById(id);
        }


        public IEnumerable<TrainTableRec> GetAll()
        {
            var rep = (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            return rep.List();
        }


        public void SaveAll(IEnumerable<TrainTableRec> list)
        {
            var rep = (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.AddRange(list);
        }

        public void DeleteItem(TrainTableRec item)
        {
            var rep = (SourceLoad == TrainRecType.LocalMain) ? _repLocalMain : _repRemoteCis;
            rep.Delete(item);
        }

        #endregion
    }
}