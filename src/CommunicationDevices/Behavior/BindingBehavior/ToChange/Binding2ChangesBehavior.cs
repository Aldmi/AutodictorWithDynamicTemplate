using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationDevices.Behavior.BindingBehavior.Helpers;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using CommunicationDevices.Settings;



namespace CommunicationDevices.Behavior.BindingBehavior.ToChange
{
    class Binding2ChangesBehavior : IBinding2ChangesBehavior, IDisposable
    {
        #region fields

        private readonly Device _device;

        #endregion




        #region prop

        public int HourDepth { get; }
        public bool IsPaging { get; }
        public Conditions Conditions { get; }
        public PaggingHelper PagingHelper { get; set; }
        public IDisposable DispousePagingListSendRx { get; set; }

        public DeviceSetting GetDeviceSetting => _device.Setting;

        #endregion





        #region ctor

        public Binding2ChangesBehavior(Device device, int hourDepth, Conditions conditions, int countPage, int timePaging)
        {
            Conditions = conditions;
            _device = device;
            HourDepth = hourDepth;

            //если указанны настройки пагинатора.
            if (countPage > 0 && timePaging > 0)
            {
                IsPaging = true;
                PagingHelper = new PaggingHelper(timePaging * 1000, countPage * 2); // (countPage * 2) - т.к. в список попадает пара, значение до и после изменений 
                DispousePagingListSendRx = PagingHelper.PagingListSend.Subscribe(OnNext);     //подписка на отправку сообщений пагинатором
            }
        }


        public Binding2ChangesBehavior(Device device)
        {
            _device = device;
        }

        #endregion






        #region Metode

        private void OnNext(PagingList pagingList)
        {
            var inData = new UniversalInputType
            {
                TableData = pagingList.List,
                Note = pagingList.CurrentPage.ToString()
            };

            _device.ExhBehavior.AddOneTimeSendData(inData);
        }




        public void InitializePagingBuffer(UniversalInputType inData, Func<UniversalInputType, bool> checkContrains, int? countDataTake = null)
        {
            var query = inData.TableData.Where(checkContrains);
            if (countDataTake != null && countDataTake > 0)
            {
                query = query.Take(countDataTake.Value);
            }

            var filteredTable = query.ToList();
            if (IsPaging)
            {
                PagingHelper.PagingBuffer = filteredTable;
            }
            else
            {
                inData.TableData = filteredTable;
                inData.Note = String.Empty;
                _device.AddCycleFuncData(0, inData);
            }
        }



        /// <summary>
        /// Проверка ограничения привязки.
        /// </summary>
        public bool CheckContrains(UniversalInputType inData)
        {
            if (!inData.IsActive)
                return false;

            if (Conditions == null)
                return true;

            return Conditions.CheckContrains(inData);
        }


        /// <summary>
        /// Вернуть сколко первых элементов таблицы нужно взять
        /// </summary>
        public int? GetCountDataTake()
        {
            return Conditions?.LimitNumberRows;
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            DispousePagingListSendRx?.Dispose();
            PagingHelper?.Dispose();
        }

        #endregion
    }
}
