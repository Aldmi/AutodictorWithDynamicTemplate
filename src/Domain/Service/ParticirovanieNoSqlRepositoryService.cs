using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Concrete.Generic;
using Domain.Concrete.NoSqlReposutory;
using Domain.Entitys;


namespace Domain.Service
{
    /// <summary>
    /// Партицирует NoSql репозитории по датам.
    /// Каждые новые сутки идет запись в новый файл БД.
    /// </summary>
    public class ParticirovanieNoSqlRepositoryService<T> where T : EntityBase
    {
        private const string BaseFileName = @"NoSqlDb\Main_";


        public IRepository<T> GetRepositoryOnCurrentDay()
        {
            var postFix = DateTime.Now.Date.ToString("ddMMyyyy") +".db";
            var connection = BaseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }


        public IRepository<T> GetRepositoryOnYesterdayDay()
        {
            var postFix = DateTime.Now.AddDays(-1).Date.ToString("ddMMyyyy") + ".db";
            var connection = BaseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }


        public IRepository<T> GetRepositoryOnDay(DateTime date)
        {
            var postFix = date.ToString("ddMMyyyy") + ".db";
            var connection = BaseFileName + postFix;
            return new RepositoryNoSql<T>(connection);
        }
    }
}
