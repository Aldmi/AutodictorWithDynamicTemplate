using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Communication.Annotations;
using Communication.Interfaces;

namespace CommunicationDevices.DataProviders.ChannelManagementDataProvider
{
    public class ChannelManagement10ChWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        #region Prop

        public int CountGetDataByte { get; private set; } //вычисляется при отправке
        public int CountSetDataByte { get; } = 8;

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; set; }

        public bool IsOutDataValid { get; private set; }
        public Subject<byte> OutputDataChangeRx { get; } = null;
        public string ProviderName { get; set; }

        #endregion






        /// <summary>
        /// формат посылки<STX><ESC> КОМАНДА <ETX> CRC <US>
        /// CRC - контрольный код, считаемый XORом всех данных, расположенных среди<STX>...<ETX>.
        /// </summary>
        public byte[] GetDataByte()
        {
            if (InputData.ViewBag == null || !InputData.ViewBag.ContainsKey("SoundChanelManagmentEventPlaying"))
                return null;

            var ev = InputData.ViewBag["SoundChanelManagmentEventPlaying"] as string;

            //Вычисляем команду.
            byte[] command; 
            switch (ev)
            {
                case "InitSoundChanelDevice_step1":
                    command = new byte[1];
                    command[0] = 0x56;
                    break;


                case "InitSoundChanelDevice_step2":
                    command = new byte[1];
                    command[0] = 0x43;
                    break;


                case "StartPlaying":
                    var chanelFlags = InputData.SoundChanels.ToArray();
                    bool[] fullArr = new bool[16];

                    Array.Copy(chanelFlags, 0, fullArr, 0, 7);
                    fullArr[7] = true;
                    Array.Copy(chanelFlags, 7, fullArr, 8, 7);
                    fullArr[15] = true;

                    BitArray bitArray = new BitArray(fullArr);
                    command = new byte[3];
                    command[0] = 0x57;
                    bitArray.CopyTo(command, 1);

                    //Debug.WriteLine("command[0]" + command[0].ToString("X") + "  " + "command[1]" + command[1].ToString("X") + "  " + "command[2]" + command[2].ToString("X") + "  " + "command[3]" + command[3].ToString("X"));
                    break;



                case "StopPlaying":                   //<STX><ESC>#43<ETX>#58<US>
                    command = new byte[1];   
                    command[0] = 0x43;            
                    break;

                default:
                    return null;
            }


            //Вычислим CRC
            var xorBytes = new List<byte> {0x1B};
            xorBytes.AddRange(command);
            byte xor = CalcXor(xorBytes);

            //Сформируем конечный буффер
            byte[] bufer = new byte[5 + command.Length];
            bufer[0] = 0x02; //STX
            bufer[1] = 0x1B; //ESC
            Array.Copy(command, 0, bufer, 2, command.Length);
            bufer[2 + command.Length] = 0x03; //ETX
            bufer[3 + command.Length] = xor;
            bufer[4 + command.Length] = 0x1F; //US

            return bufer;
        }


        public Stream GetStream()
        {
            throw new NotImplementedException();
        }


        public bool SetStream(Stream stream)
        {
            throw new NotImplementedException();
        }


        public bool SetDataByte(byte[] data)
        {
            if (data == null || !data.Any())
                return false;


            if (data[0] == 0x06) //ASC
                return true;

            if (data.Length > 2)
            {
                if (data[0] == 0x53 && data[1] == 0x31) //  Ответ на запрос инициализации
                    return true;
            }


            return false;
        }



        private byte CalcXor(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
            return xor;
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