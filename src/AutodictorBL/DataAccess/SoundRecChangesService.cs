using System.Collections.Generic;
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
            var rep = _particirovanieService.GetRepositoryOnCurrentDay();
           var list=  rep.List();
           rep.Add(new SoundRecordChangesDb {CauseOfChange = "dsdsd"});
            //DEBUG---
        }

        #endregion




        #region Methode

        //TODO:Методы для работы с партицированным репозиторием


        #endregion
    }
}