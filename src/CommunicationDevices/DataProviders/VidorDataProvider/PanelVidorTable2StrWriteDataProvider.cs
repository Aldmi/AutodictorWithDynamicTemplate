using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using Communication.Annotations;
using Communication.Interfaces;

namespace CommunicationDevices.DataProviders.VidorDataProvider
{
    public class PanelVidorTable2StrWriteDataProvider : ILineByLineDrawingTableDataProvider
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
            {   // первые 2-е строки
                //STX                
                //01 - аддр. 01
                //85
                //%000011030114                          - 3 координты, Х1 = 001,  X2 = 103, Y = 011, формат = 4(горизонт.перемещ)
                //%10$12$00$60$t3НАХАБИНО                - текст, $00$60$t3$123, -код.табл по умолч, не мигать, по центру.  (НАХАБИНО)

                //%001051440114                          - 3 координты, Х1 = 105,  X2 = 144, Y = 011, формат = 4(горизонт.перемещ)
                //%10$12$00$60$t317:40                   - текст, $00$60$t3$1217:40, -код.табл по умолч, не мигать, по центру.  (17:40)

                //%001471760114                          - 3 координты, Х1 = 147,  X2 = 176, Y = 011, формат = 4(горизонт.перемещ)
                //%10$12$00$60$t3Путь                    - текст, $00$60$t3$1216:50, -код.табл по умолч, не мигать, по центру.

                //%001811900114                          - 3 координты, Х1 = 181,  X2 = 190, Y = 011, формат = 4(горизонт.перемещ)
                //%10$12$00$60$t33                       - текст, $00$60$t3$123, -код.табл по умолч, не мигать, по центру.  (3)

                //%000011920234                          - 3 координты, Х1 = 001,  X2 = 192, Y = 023, формат = 4(горизонт.перемещ)
                //%10$12$00$60$t3со всеми остановками    - текст, $12$00$60$t3со всеми остановками, -код.табл по умолч, не мигать, по центру.  (Э / П)
                //A1                                     - СRC
                //ETX    

                byte address = byte.Parse(InputData.AddressDevice);

                string numberOfPath = string.IsNullOrEmpty(InputData.PathNumber) ? " " : InputData.PathNumber;
                string stations = string.IsNullOrEmpty(InputData.Stations) ? " " : InputData.Stations;
                string time = (InputData.Time == DateTime.MinValue) ? " " : InputData.Time.ToShortTimeString();
                string followingStation = string.IsNullOrEmpty(InputData.Note) ? " " : InputData.Note;

                string result1, result2, result3, result4, result5;


                if (CurrentRow == 0xFF)
                {
                    // %30 - синхр часов
                    // [3..8] - 5байт (hex) время в сек.   
                    var timeNow = DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2");
                    string format1 = "%30";
                    string message1 = $"{timeNow}";
                    result1 = format1 + message1;
                    result2 = result3 = result4 = result5 = string.Empty;
                }
                else
                {
                    //первая надпись занмиает 2-е строки
                    int y1 = 11;   //Y1
                    int y2 = 23;   //Y2
                    if (CurrentRow > 1)
                    {
                        y1 = y1 + (24 * (CurrentRow - 1));
                        y2 = y2 + (24 * (CurrentRow - 1));
                    }

                    string y1Str = y1.ToString("D3");
                    string y2Str = y2.ToString("D3");


                    // %00 - задание формата вывода СТАНЦИИ (Отпр или Назн)
                    // 001 - Х1
                    // 103 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format1 = $"%00001103{y1Str}4";
                    string message1 = $"%10$12$00$60$t3{stations}";
                    result1 = format1 + message1;

                    // %01 - задание формата вывода ВРЕМЕНИ
                    // 105 - Х1
                    // 144 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format2 = $"%00105144{y1Str}4";
                    string message2 = $"%10$12$00$60$t3{time}";
                    result2 = format2 + message2;

                    // %01 - задание формата вывода слова ПУТЬ
                    // 147 - Х1
                    // 176 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format3 = $"%00147176{y1Str}4";
                    string message3 = "%10$12$00$60$t3Путь";
                    result3 = format3 + message3;

                    // %01 - задание формата вывода номера пути
                    // 181 - Х1
                    // 190 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format4 = $"%00181190{y1Str}4";
                    string message4 = $"%10$12$00$60$t3{numberOfPath}";
                    result4 = format4 + message4;

                    // %01 - задание формата вывода станций следования
                    // 001 - Х1
                    // 192 - X2
                    // вычисляется - Y
                    // аттриб = 4 (бег.стр.)
                    string format5 = $"%00001192{y2Str}4";
                    string message5 = $"%10$12$00$60$t3{followingStation}";
                    result5 = format5 + message5;
                }






                //формируем КОНЕЧНУЮ строку
                var sumResult = result1 + result2 + result3 + result4 + result5;

                //Обрежем конец строки если ее длинна превышает допустимые 254 символа.
                byte maxLenght = 0xFE;
                if (sumResult.Length >= maxLenght)
                {
                    var removeCount = sumResult.Length - maxLenght;
                    sumResult = sumResult.Remove(maxLenght, removeCount);
                }

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