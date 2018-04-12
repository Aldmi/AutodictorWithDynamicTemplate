using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Abstract.Abstract;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;


namespace AutodictorBL.DataAccess
{
    public class SoundRecChangesService
    {
        private readonly IParticirovanieService<SoundRecordChangesDb> _particirovanieService;



        #region ctor

        public SoundRecChangesService(IParticirovanieService<SoundRecordChangesDb> particirovanieService)
        {
            _particirovanieService = particirovanieService;

            //DEBUG---
           // var rep = _particirovanieService.GetRepositoryOnCurrentDay();
           //var list=  rep.List();
           //rep.Add(new SoundRecordChangesDb {CauseOfChange = "dsdsd"});
            //DEBUG---
        }

        #endregion




        #region Methode

        //TODO:Методы для работы с партицированным репозиторием
        public void Add(SoundRecordChangesDb change)
        {
            var rep = _particirovanieService.GetRepositoryOnCurrentDay();
            rep.Add(change);
        }


        public IEnumerable<SoundRecordChangesDb> Get(Expression<Func<SoundRecordChangesDb, bool>> predicate)
        {
            var repOnYesterdayDay= _particirovanieService.GetRepositoryOnYesterdayDay();
            var repOnCurrentDay= _particirovanieService.GetRepositoryOnCurrentDay();
            var listOnYesterdayDay= repOnYesterdayDay.List();
            var listOnCurrentDay= repOnCurrentDay.List();

            var resultList= new List<SoundRecordChangesDb>();
            resultList.AddRange(listOnYesterdayDay);
            resultList.AddRange(listOnCurrentDay);
            return resultList.Where(predicate.Compile());
        }


        public IEnumerable<SoundRecordChangesDb> GetByDateRange(DateTime startDate, DateTime endDate, Expression<Func<SoundRecordChangesDb, bool>> predicate)
        {
            if (startDate > endDate)
                return null;

            var resultList = new List<SoundRecordChangesDb>();
            for (var day = startDate.Date; day <= endDate.Date; day= day.AddDays(1))
            {
               var repOnDay= _particirovanieService.GetRepositoryOnDay(day);
               if (repOnDay != null)
               {
                   var listOnDay = repOnDay.List();
                   resultList.AddRange(listOnDay);
               }
            }
            return resultList.Where(predicate.Compile());
        }

        #endregion
    }
}