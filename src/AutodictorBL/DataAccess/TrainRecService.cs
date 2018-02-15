using DAL.Abstract.Concrete;

namespace AutodictorBL.DataAccess
{
    public class TrainRecService
    {
        private readonly ITrainTableRecRepository _repLocalMain;
        private readonly ITrainTableRecRepository _repRemoteCis;
        private readonly ITrainTypeByRyleRepository _repTypeByRyle;


        #region ctor

        public TrainRecService(ITrainTableRecRepository repLocalMain, ITrainTableRecRepository repRemoteCis, ITrainTypeByRyleRepository repTypeByRyle)
        {
            _repLocalMain = repLocalMain;
            _repRemoteCis = repRemoteCis;
            _repTypeByRyle = repTypeByRyle;
        }

        #endregion




        #region Methode

        //public Pathways GetById(int id)
        //{
        //    return _repLocalMain.GetById(id);
        //}


        //public IEnumerable<Pathways> GetAll()
        //{
        //    return _repLocalMain.List();
        //}

        #endregion
    }
}