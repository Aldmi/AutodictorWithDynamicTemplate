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

        #endregion




        #region ctor

        public BaseGetDataBehavior(string name, ISubject<IExhangeBehavior> connectChangeRx,
                                                ISubject<IExhangeBehavior> dataExchangeSuccessRx,
                                                ISubject<Stream> getStreamRx,
                                                IInputDataConverter inputConverter)
        {
            Name = name;
            ConnectChangeRx = connectChangeRx;
            DataExchangeSuccessRx = dataExchangeSuccessRx;
            GetStreamRxHandlerDispose = getStreamRx.Subscribe(GetStreamRxHandler);      //подписка на событие получения потока данных
            InputConverter = inputConverter;
        }

        #endregion




        /// <summary>
        /// Обработчик события получения потока данных от сервера апк-дк ВОЛГОГРАД (GET запрос)
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
                Log.log.Error($"метод GetStreamRxHandler:  {ex.Message}");
            }
        }




        public void Dispose()
        {
            GetStreamRxHandlerDispose?.Dispose();
        }
    }
}