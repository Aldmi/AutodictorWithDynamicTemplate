using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Communication.Annotations;
using Communication.Interfaces;
using Domain.Entitys;
using Library.Extensions;
using Library.Logs;


namespace CommunicationDevices.DataProviders.XmlDataProvider
{
    public class StreamWriteDataProvider : IExchangeDataProvider<UniversalInputType, Stream>
    {
        #region Prop

        public int CountGetDataByte { get; }
        public int CountSetDataByte { get; }

        public UniversalInputType InputData { get; set; }
        public Stream OutputData { get; set; }

        public bool IsOutDataValid { get; private set; }

        public IFormatProvider FormatProvider { get; set; }

        public Subject<Stream> OutputDataChangeRx { get; } = new Subject<Stream>();
        public string ProviderName { get; set; }

        #endregion





        #region ctor

        public StreamWriteDataProvider(IFormatProvider formatProvider)
        {
            FormatProvider = formatProvider;
            ProviderName = formatProvider.GetType().Name;
        }

        #endregion






        public byte[] GetDataByte()
        {
            throw new NotImplementedException();
        }



        public Stream GetStream()
        {
            try
            {
                var xmlRequest = FormatProvider.CreateDoc(InputData?.TableData);
                if (xmlRequest != null)
                {
                    var xmlVersion = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
                    var resultXmlDoc = xmlVersion + xmlRequest;
                    return resultXmlDoc.GenerateStreamFromString();
                }
            }
            catch (Exception ex)
            {
                Log.log.Fatal($"Исключение в методе GetStream {ex.Message}");
                return null;
            }

            return null;
        }



        public bool SetStream(Stream stream) 
        {
            OutputData = stream;
            OutputDataChangeRx.OnNext(stream);

            return (stream != null);
        }



        public bool SetDataByte(byte[] data)
        {
            return false;
        }




        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}