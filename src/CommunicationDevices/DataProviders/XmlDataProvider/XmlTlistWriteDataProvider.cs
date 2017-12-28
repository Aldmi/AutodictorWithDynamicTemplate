using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Communication.Annotations;
using Communication.Interfaces;


namespace CommunicationDevices.DataProviders.XmlDataProvider
{
    public class XmlTlistWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        #region Prop

        public int CountGetDataByte { get; }
        public int CountSetDataByte { get; }

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; }

        public bool IsOutDataValid { get; private set; }

        public string Format { get; set; }

        #endregion





        public byte[] GetDataByte()
        {
            throw new NotImplementedException();
        }



        public Stream GetStream()
        {
            try
            {
                var xmlRequest = CreateXmlRequest(InputData?.TableData);
                if (xmlRequest != null)
                {
                    var xmlVersion = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
                    var resultXmlDoc = xmlVersion + xmlRequest;
                    return GenerateStreamFromString(resultXmlDoc);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }



        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }



        public bool SetDataByte(byte[] data)
        {
            //TODO: преобразовать массив байт обратно в строку и проверить ответ
            if (data != null && data.Length == 15)
            {
                return (data[0] == 78) && (data[1] == 85);
            }

            return false;
        }



        private XDocument CreateXmlRequest(IEnumerable<UniversalInputType> tables)
        {
            if (tables == null || !tables.Any())
                return null;

            var xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("tlist"));
            foreach (var uit in tables)
            {
                string trainType = String.Empty;
                string typeName = String.Empty;
                string typeNameShort = String.Empty;
                switch (uit.TypeTrain)
                {
                    case TypeTrain.None:
                        trainType = String.Empty;
                        typeName = String.Empty;
                        break;

                    case TypeTrain.Suburban:
                        trainType = "0";
                        typeName = "Пригородный";
                        typeNameShort = "приг";
                        break;

                    case TypeTrain.Express:
                        trainType = "1";
                        typeName = "Экспресс";
                        typeNameShort = "экспресс";
                        break;

                    case TypeTrain.HighSpeed:
                        trainType = "2";
                        typeName = "Скорый";
                        typeNameShort = "скор";
                        break;

                    case TypeTrain.Corporate:
                        trainType = "3";
                        typeName = "Фирменный";
                        typeNameShort = "фирм";
                        break;

                    case TypeTrain.Passenger:
                        trainType = "4";
                        typeName = "Пассажирский";
                        typeNameShort = "пасс";
                        break;

                    case TypeTrain.Swallow:
                        trainType = "5";
                        typeName = "Скоростной";
                        typeNameShort = "скоростной";
                        break;

                    case TypeTrain.Rex:
                        trainType = "5";
                        typeName = "Скоростной";
                        typeNameShort = "скоростной";
                        break;
                }

                string startSt;
                string endSt;
                var stations = uit.Stations.Split('-').Select(s => s.Trim()).ToList();
                if (stations.Count == 2)
                {
                    startSt = stations[0];
                    endSt = stations[1];
                }
                else
                {
                    startSt = (uit.Event == "ОТПР.") ? stations[0] : " ";
                    endSt = (uit.Event == "ПРИБ.") ? stations[0] : " ";
                }

                var time = uit.Time.ToString("s");


                xDoc.Root?.Add(
                    new XElement("t",
                    new XElement("TrainNumber", uit.NumberOfTrain),
                    new XElement("TrainType", trainType),
                    new XElement("StartStation", startSt),
                    new XElement("EndStation", endSt),
                    new XElement("RecDateTime", time),
                    new XElement("SndDateTime", time),
                    new XElement("EvRecTime", time),
                    new XElement("EvSndTime", time),
                    new XElement("TrackNumber", uit.PathNumber),
                    new XElement("Direction", (uit.Event == "ПРИБ.") ? 0 : 1),
                    new XElement("EvTrackNumber", uit.PathNumber),
                    new XElement("State", 0),
                    new XElement("VagonDirection", (byte)uit.VagonDirection),
                    new XElement("Enabled", (uit.EmergencySituation & 0x01) == 0x01 ? 0 : 1),

                    new XElement("tt",
                    new XElement("TypeName", typeName),
                    new XElement("TypeAlias", typeNameShort))
                    ));
            }



            //DEBUG------------------------
            //string path = Application.StartupPath + @"/StaticTableDisplay" + @"/xDoc.info";
            //xDoc.Save(path);
            //-----------------------------

            return xDoc;
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