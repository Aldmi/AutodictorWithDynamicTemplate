using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using Domain.Entitys;
using MainExample.Entites;
using MainExample.Extension;

namespace MainExample.Services.GetDataService
{

    public abstract class GetSheduleAbstract : IDisposable
    {
        #region field

        private readonly ISubject<IEnumerable<UniversalInputType>> _sheduleGetRx;
        protected readonly SortedDictionary<string, SoundRecord> _soundRecords;

        #endregion




        #region prop

        public ISubject<IExhangeBehavior> ConnectChangeRx { get; }
        public ISubject<IExhangeBehavior> DataExchangeSuccessRx { get; }

        public IDisposable DispouseSheduleGetRx { get; set; }
        public IDisposable DispouseConnectChangeRx { get; set; }
        public IDisposable DispouseDataExchangeSuccessChangeRx { get; set; }

        public bool Enable { get; set; }

        public ISubject<SoundRecordChanges> SoundRecordChangesRx { get; } = new Subject<SoundRecordChanges>();

        #endregion




        #region ctor

        protected GetSheduleAbstract(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords)
        {
            _sheduleGetRx = baseGetDataBehavior.ConvertedDataChangeRx;
            ConnectChangeRx = baseGetDataBehavior.ConnectChangeRx;
            DataExchangeSuccessRx = baseGetDataBehavior.DataExchangeSuccessRx;
            _soundRecords = soundRecords;
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
                DispouseSheduleGetRx = _sheduleGetRx?.Subscribe(GetaDataRxEventHandler);
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