using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Changes;
using MainExample.Extension;

namespace MainExample.Services.GetDataService
{

    public abstract class GetSheduleAbstract : IDisposable
    {
        #region field

        private readonly BaseGetDataBehavior _baseGetDataBehavior;

        #endregion




        #region prop

        protected ISubject<IEnumerable<UniversalInputType>> SheduleGetRx { get; }  //Входное событие "Получение данных"
        protected ISubject<IExhangeBehavior> ConnectChangeRx { get; }              //Входное событие "Состояние соединения"
        protected ISubject<IExhangeBehavior> DataExchangeSuccessRx { get; }        //Входное событие "Состояние обмена данными"

        protected IDisposable DispouseSheduleGetRx { get; set; }
        protected IDisposable DispouseConnectChangeRx { get; set; }
        protected IDisposable DispouseDataExchangeSuccessChangeRx { get; set; }

        public bool Enable { get; set; }

        #endregion




        #region ctor

        protected GetSheduleAbstract(BaseGetDataBehavior baseGetDataBehavior)
        {
            _baseGetDataBehavior = baseGetDataBehavior;
            SheduleGetRx = baseGetDataBehavior.ConvertedDataChangeRx;
            ConnectChangeRx = baseGetDataBehavior.ConnectChangeRx;
            DataExchangeSuccessRx = baseGetDataBehavior.DataExchangeSuccessRx;
        }

        #endregion





        #region Methode

        /// <summary>
        /// Подписать все события и запустить
        /// </summary>
        public void SubscribeAndStart(Control control)
        {
            try
            {
                DispouseSheduleGetRx = SheduleGetRx?.Subscribe(GetaDataRxEventHandler);
                control.Enabled = _baseGetDataBehavior.IsConnect;
                DispouseConnectChangeRx = ConnectChangeRx.Subscribe(behavior =>                       //контролл не активен, если нет связи
                {              
                    control.InvokeIfNeeded(() =>
                    {
                        control.Enabled = behavior.IsConnect;
                    });    
                }); 

                DispouseDataExchangeSuccessChangeRx = DataExchangeSuccessRx.Subscribe(behavior =>
                {
                    var colorYes = Color.GreenYellow;
                    var colorError = Color.Red;
                    var colorNo = Color.White;
                    control.InvokeIfNeeded(() =>
                    {
                        control.BackColor = (behavior.DataExchangeSuccess) ? colorYes : colorError;
                    });
                    Task.Delay(1000).ContinueWith(task =>
                    {
                        control.InvokeIfNeeded(() =>
                        {
                            control.BackColor = colorNo;
                        });
                    });
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }



        /// <summary>
        /// Обработка полученных данных.
        /// </summary>
        protected abstract void GetaDataRxEventHandler(IEnumerable<UniversalInputType> data);

        #endregion




        #region Disposable

        public void Dispose()
        {
            DispouseSheduleGetRx?.Dispose();
            DispouseConnectChangeRx?.Dispose();
            DispouseDataExchangeSuccessChangeRx?.Dispose();
        }

        #endregion
    }
}