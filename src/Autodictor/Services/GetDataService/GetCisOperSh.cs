using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;


namespace MainExample.Services.GetDataService
{
    public class GetCisOperSh : GetSheduleAbstract
    {
        #region ctor

        public GetCisOperSh(BaseGetDataBehavior baseGetDataBehavior) 
            : base(baseGetDataBehavior)
        {

        }

        #endregion




        #region Methode

        /// <summary>
        /// Обработка полученных данных
        /// </summary>
        protected override async Task GetaDataRxEventHandler(Task<IEnumerable<UniversalInputType>> getDataTask)
        {
            if (!Enable)
                return;

            try
            {
                var data = await getDataTask;
                var inputDatas = data as IList<UniversalInputType> ?? data.ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
