using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Xml.Linq;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData;
using CommunicationDevices.DataProviders;
using Library.Logs;

namespace CommunicationDevices.Behavior.GetDataBehavior
{
    public class BaseGetDataBehavior : IDisposable
    {

        #region field

        private readonly IExhangeBehavior _exhangeBehavior;

        #endregion



        #region prop
        //название поведения получения данных

        public string Name { get; set; }     

        //издатель события "данные получены и преобразованны в IEnumerable<UniversalInputType>"
        public ISubject<IEnumerable<UniversalInputType>> ConvertedDataChangeRx { get; } = new Subject<IEnumerable<UniversalInputType>>();

        //издатель события "изменения состояния соединения с сервером"
        public ISubject<IExhangeBehavior> ConnectChangeRx { get; }

        //издатель события "изменения состояния обмена данными"
        public ISubject<IExhangeBehavior> DataExchangeSuccessRx { get; }

        //конвертер в XDocument -> IEnumerable<UniversalInputType>
        public IInputDataConverter InputConverter { get; }

        public IDisposable GetStreamRxHandlerDispose { get; set; }

        private object locker= new object();

        public bool IsConnect => _exhangeBehavior.IsConnect;

        #endregion




        #region ctor

        public BaseGetDataBehavior(string name, IExhangeBehavior exhangeBehavior,
                                                ISubject<Stream> getStreamRx,
                                                IInputDataConverter inputConverter)
        {
            _exhangeBehavior = exhangeBehavior;
            Name = name;
            ConnectChangeRx = exhangeBehavior.IsConnectChange;
            DataExchangeSuccessRx = exhangeBehavior.IsDataExchangeSuccessChange;
            GetStreamRxHandlerDispose = getStreamRx.Subscribe(GetStreamRxHandler);      //подписка на событие получения потока данных
            InputConverter = inputConverter;
        }

        #endregion




        /// <summary>
        /// Обработчик события получения ответа от сервера на GET запрос.
        /// </summary>
        private void GetStreamRxHandler(Stream stream)
        {
            try
            {
                lock (locker)
                {
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();
                    XDocument xDoc = XDocument.Parse(text);
                    var data = InputConverter.ParseXml2Uit(xDoc)?.ToList();
                    ConvertedDataChangeRx.OnNext(data);
                }
            }
            catch (Exception ex)
            {
                Log.log.Error($"BaseGetDataBehavior.GetStreamRxHandler(). Exception:  {ex.Message}");
            }
        }




        public void Dispose()
        {
            GetStreamRxHandlerDispose?.Dispose();
        }
    }
}