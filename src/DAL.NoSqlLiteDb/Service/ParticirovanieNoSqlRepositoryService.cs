using System;
using DAL.Abstract.Abstract;
using DAL.Abstract.Entitys;
using DAL.NoSqlLiteDb.Repository;

namespace DAL.NoSqlLiteDb.Service
{
    /// <summary>
    /// Партицирует NoSql репозитории по датам.
    /// Каждые новые сутки идет запись в новый файл БД.
    /// </summary>
    public class ParticirovanieNoSqlRepositoryService<T> : IParticirovanieService<T> where T : EntityBase
    {
        private readonly string _baseFileName;



        #region ctor

        public ParticirovanieNoSqlRepositoryService(string baseFileName)
        {
            _baseFileName = baseFileName;
        }

        #endregion



        public IGenericDataRepository<T> GetRepositoryOnCurrentDay()
        {
            var postFix = DateTime.Now.Date.ToString("ddMMyyyy") +".db";
            var connection = _baseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }


        public IGenericDataRepository<T> GetRepositoryOnYesterdayDay()
        {
            var postFix = DateTime.Now.AddDays(-1).Date.ToString("ddMMyyyy") + ".db";
            var connection = _baseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }


        public IGenericDataRepository<T> GetRepositoryOnDay(DateTime date)
        {
            var postFix = date.ToString("ddMMyyyy") + ".db";
            var connection = _baseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }
    }
}
