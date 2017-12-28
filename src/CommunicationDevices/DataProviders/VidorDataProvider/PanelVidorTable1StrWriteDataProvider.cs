using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using Communication.Annotations;


namespace CommunicationDevices.DataProviders.VidorDataProvider
{
    public class PanelVidorTable1StrWriteDataProvider : ILineByLineDrawingTableDataProvider
    {
        #region Prop

        public byte CurrentRow { get; set; }

        public int CountGetDataByte { get; private set; } //вычисляется при отправке
        public int CountSetDataByte { get; } = 8;

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; set; }

        public bool IsOutDataValid { get; private set; }
        public Subject<byte> OutputDataChangeRx { get; } = null;
        public string ProviderName { get; set; }

        #endregion




        /// <summary>
        /// Данные запроса по записи строки на табло. Пакет формируется в строковом типе, затем переводится в массив байт в кодировке win-1251.
        /// байт[0]= STX   (0x02)
        /// байт[1]=       адресс
        /// байт[2]=       адресс
        /// байт[3]=       N байт пакета (без CRC и ETX)
        /// байт[4]=       N байт пакета (без CRC и ETX)
        /// байт[..]=       формат1  (% ... )
        /// байт[..]=       строка1  (% ... )
        /// байт[..]=       формат2  (% ... )
        /// байт[..]=       строка2  (% ... )
        /// байт[..]=       формат3  (% ... )
        /// байт[..]=       строка3  (% ... )
        /// байт[..]=       CRC
        /// байт[..]=       CRC
        /// байт[..]=       ETX   (0x03)
        /// </summary>
        public byte[] GetDataByte()
        {
            try
            {
                //STX                
                //2D - аддр. 45
                //85
                //%000020470224 - 3 координты, Х1 = 002,  X2 = 047, Y = 022, формат = 4(горизонт.перемещ)
                //%10$00$60$t3$12Э / П - текст, $00$60$t3$12Э / П, -код.табл по умолч, не мигать, по центру.  (Э / П)
                //%000481650224 - 3 координты, Х1 = 048,  X2 = 165, Y = 022, формат = 4(горизонт.перемещ)
                //%10$00$60$t3$12ПОДСОЛНЕЧНАЯ - текст, $00$60$t3$12Э / П, -код.табл по умолч, не мигать, по центру.  (ПОДСОЛНЕЧНАЯ)
                //%001712070224 - 3 координты, Х1 = 171,  X2 = 207, Y = 022, формат = 4(горизонт.перемещ)
                //%10$00$60$t3$1216:27 - текст, $00$60$t3$12Э / П, -код.табл по умолч, не мигать, по центру.  (16:27)
                //%002312520224 - 3 координты, Х1 = 231,  X2 = 252, Y = 022, формат = 4(горизонт.перемещ)
                //%10$00$60$t3$124 - текст, $00$60$t3$124, -код.табл по умолч, не мигать, по центру.           (4)
                //A1               -  СRC
                //ETX    


                byte address = byte.Parse(InputData.AddressDevice);

                string numberOfTrain = string.IsNullOrEmpty(InputData.NumberOfTrain) ? " " : InputData.NumberOfTrain;
                string numberOfPath = string.IsNullOrEmpty(InputData.PathNumber) ? " " : InputData.PathNumber;
                string stations = string.IsNullOrEmpty(InputData.Stations) ? " " : InputData.Stations;
                string time = (InputData.Time == DateTime.MinValue) ? " " : InputData.Time.ToShortTimeString();
                string rowNumber = (11 * CurrentRow).ToString("D3");


                string result1, result2, result3, result4;
                if (CurrentRow == 0xFF)
                {
                    // %30 - синхр часов
                    // [3..8] - 5байт (hex) время в сек.   
                    var timeNow = DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2");
                    string format1 = "%30";
                    string message1 = $"{timeNow}";
                    result1 = format1 + message1;
                    result2 = result3 = result4 = string.Empty;
                }
                else
                {
                    // %00 - задание формата вывода НАЗВАНИЯ ПОЕЗДА
                    // 001 - Х1
                    // 047 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format1 = $"%00002047{rowNumber}4";
                    string message1 = $"%10$00$60$t3$12{numberOfTrain}";
                    result1 = format1 + message1;

                    // %01 - задание формата вывода СТАНЦИИ
                    // 048 - Х1
                    // 165 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format2 = $"%00048165{rowNumber}4";
                    string message2 = $"%10$00$60$t3$12{stations}";
                    result2 = format2 + message2;

                    // %01 - задание формата вывода ВРЕМЕНИ
                    // 171 - Х1
                    // 207 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format3 = $"%00171207{rowNumber}4";
                    string message3 = $"%10$00$60$t3$12{time}";
                    result3 = format3 + message3;

                    // %01 - задание формата вывода ПУТИ
                    // 231 - Х1
                    // 252 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format4 = $"%00231252{rowNumber}4";
                    string message4 = $"%10$00$60$t3$12{numberOfPath}";
                    result4 = format4 + message4;
                }


                //формируем КОНЕЧНУЮ строку
                var sumResult = result1 + result2 + result3 + result4;
                var resultstring = address.ToString("X2") + sumResult.Length.ToString("X2") + sumResult;

                //вычисляем CRC
                byte[] xorBytes = Encoding.GetEncoding("Windows-1251").GetBytes(resultstring);
                byte xor = CalcXor(xorBytes);
                resultstring += xor.ToString("X2");

                //Преобразовываем КОНЕЧНУЮ строку в массив байт
                var resultBuffer = Encoding.GetEncoding("Windows-1251").GetBytes(resultstring).ToList();
                resultBuffer.Insert(0, 0x02); //STX
                resultBuffer.Add(0x03); //ETX

                return resultBuffer.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public Stream GetStream()
        {
            throw new NotImplementedException();
        }


        public bool SetStream(Stream stream)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Данные ответа по записи строки на табло.
        /// байт[0]= 
        /// байт[1]= 
        /// байт[2]= 
        /// байт[3]=
        /// байт[4]= 
        /// </summary>
        public bool SetDataByte(byte[] data)
        {
            if (data == null || data.Length != CountSetDataByte)
            {
                IsOutDataValid = false;
                return false;
            }

            if (data[0] == 0x02 &&
                data[1] == 0x30 &&
                data[2] == 0x32 &&
                data[3] == 0x30 &&
                data[4] == 0x30 &&
                data[5] == 0x46 &&
                data[6] == 0x44 &&
                data[7] == 0x03)
            {
                IsOutDataValid = true;
                return true;
            }

            IsOutDataValid = false;
            return false;
        }



        private byte CalcXor(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
            xor ^= 0xFF;

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