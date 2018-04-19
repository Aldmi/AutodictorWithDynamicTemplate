﻿using System.Collections.Generic;
using System.Linq;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;


namespace MainExample.Services.GetDataService
{
    class GetCisOperSh : GetSheduleAbstract
    {
        #region ctor

        public GetCisOperSh(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords) 
            : base(baseGetDataBehavior, soundRecords)
        {

        }

        #endregion




        #region Methode

        /// <summary>
        /// Обработка полученных данных
        /// </summary>
        protected override void GetaDataRxEventHandler(IEnumerable<UniversalInputType> data)
        {
            if (!Enable)
                return;

            var universalInputTypes = data as IList<UniversalInputType> ?? data.ToList();
            if (universalInputTypes.Any())
            {
                foreach (var tr in universalInputTypes)
                {
                    
                }
            }
        }

        #endregion
    }
}
