using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using Communication.Annotations;
using Communication.Interfaces;


namespace CommunicationDevices.DataProviders.DisplaySysDataProvider
{
    public class PanelDispSysWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
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
                byte address = byte.Parse(InputData.AddressDevice);

                string numberOfTrain = string.IsNullOrEmpty(InputData.NumberOfTrain) ? " " : InputData.NumberOfTrain;
                string numberOfPath = string.IsNullOrEmpty(InputData.PathNumber) ? " " : InputData.PathNumber;
                string ev = string.IsNullOrEmpty(InputData.Event) ? " " : InputData.Event;
                string stations = string.IsNullOrEmpty(InputData.Stations) ? " " : InputData.Stations;
                string note = string.IsNullOrEmpty(InputData.Note) ? " " : InputData.Note;
                string time = (InputData.Time == DateTime.MinValue) ? " " : InputData.Time.ToShortTimeString();


                string format1, format2, format3, format4, format5, format6, format7, format8;
                string message1, message2, message3, message4, message5, message6, message7, message8;
                string result1, result2, result3, result4, result5, result6, result7, result8;



                // %30 - синхр часов
                // [3..8] - 5байт (hex) время в сек.   
                var timeNow = DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2");
                format1 = "%30";
                message1 = $"{timeNow}";
                result1 = format1 + message1;



                //ВЫВОД НА ПРИГОРОД
                //02 - Аддр.
                //BB - 187 байт данных
                //% 30143525 - синхр часов(14:35)                                                       [СИНХР.ВРЕМ.]            

                //% 000011920144 - 3 координты, Х1 = 001,  X2 = 192, Y = 014, формат = 4(горизонт.перемещ)
                //% 10$18$00$60$t2ПУТЬ №2 - текст, $18$00$60$t2ПУТЬ №2                                  [ПУТЬ]

                //% 42001192016016 - гор.линия, Х1 = 001,  X2 = 192, Y1 = 016, Y2 = 016,                [ГОР.ЛИН.]

                //% 000011500314 - 3 координты, Х1 = 001,  X2 = 150, Y = 031, формат = 4(горизонт.перемещ)
                //% 10$18$00$60$t3КРЮКОВО - ЛАСТОЧКА - текст, $18$00$60$t3КРЮКОВО - ЛАСТОЧКА            [СТАНЦИИ]

                //% 000011920464 - 3 координты, Х1 = 001,  X2 = 192, Y = 046, формат = 4(горизонт.перемещ)
                //% 10$18$00$60$t3с остановками:  Химки - текст, $18$00$60$t3с остановками:  Химки      [ПРИМЕЧАНИЕ]

                //% 001521920314 - 3 координты, Х1 = 152,  X2 = 192, Y = 031, формат = 4(горизонт.перемещ)
                //% 10$18$00$60$t315: 10 - текст, $18$00$60$t315: 10                                    [ВРЕМЯ]
                if (InputData.TypeTrain == "Пригородный")
                {
                    // %01 - задание формата вывода ПУТИ
                    // 001 - Х1
                    // 146 - X2
                    // 014 - Y
                    // аттриб = 4 (бег.стр.)
                    format2 = "%000011460144";
                    message2 = $"%10$18$00$60$t2ПУТЬ №{numberOfPath}";
                    result2 = format2 + message2;

                    // %42 - вывод гор. линии
                    // 001 - Х1
                    // 192 - Y1
                    // 016 - Y2
                    // 016 - ширина
                    message3 = "%42001192016016";
                    result3 = message3;

                    // %01 - задание формата вывода станции
                    // 077 - Х1
                    // 150 - X2
                    // 031 - Y1
                    // аттриб = 4 (бег.стр.)
                    //format4 = "%000771460314";
                    format4 = "%000011460314";                                //!!!
                    message4 = $"%10$18$00$60$t3{stations}";
                    result4 = format4 + message4;


                    // %01 - задание формата вывода времени
                    // 152 - Х1
                    // 192 - X2
                    // 046 - Y1
                    // аттриб = 4 (бег.стр.)
                    format5 = "%001521920314";
                    message5 = $"%10$18$00$60$t3{time}";
                    result5 = format5 + message5;


                    // %01 - задание формата вывода примечания
                    // 001 - Х1
                    // 192 - X2
                    // 046 - Y1
                    // аттриб = 4 (бег.стр.)
                    format6 = "%000011920464";                                 //!!!
                    message6 = $"%10$18$00$60$t3{note}";
                    result6 = format6 + message6;

                    result7 = String.Empty;
                    result8 = String.Empty;
                }

                //ВЫВРД ДАЛЬНЕГО СЛЕДОВАНИЯ
                // 05 - Аддр.
                //DA - 218 байт данных
                //% 30161903 - синхр часов                                                               [СИНХР.ВРЕМ.]   

                //% 000011460144               - 3 координты, Х1 = 001,  X2 = 146, Y = 014, формат = 4(горизонт.перемещ)
                //% 10$18$00$60$t2ПУТЬ №5      - текст, $18$00$60$t2ПУТЬ №5,                              [ПУТЬ]

                //%001521920144                - 3 координты, Х1=152,  X2= 192, Y= 014, формат=4 (горизонт.перемещ)
                //%10$18$00$60$t3              - текст, $18$00$60$t3   ,(строка 3пробела)                 [СОБЫТИЕ]

                //%000010750314                - 3 координты, Х1=001,  X2= 075, Y= 031, формат=4 (горизонт.перемещ)
                //%10$18$00$60$t3Поезд №       - текст, $18$00$60$t3Поезд №,                                

                //%42001192016016              - гор.линия, Х1=001,  X2= 192, Y1= 016, Y2= 016,            [ГОР.ЛИН]

                //%000771460314                - 3 координты, Х1=077,  X2= 146, Y= 031, формат=4 (горизонт.перемещ)
                //%10$18$00$60$t3              - текст, $18$00$60$t3   ,(строка 3пробела)                  [НОМЕР ПОЕЗДА dec]

                //%000011460464                - 3 координты, Х1=001,  X2= 146, Y= 046, формат=4 (горизонт.перемещ)
                //%10$18$00$60$t3              - текст, $18$00$60$t3,   (строка 3пробела)                  [СТАНЦИИ]

                //%001521920464                - 3 координты, Х1=152,  X2= 192, Y= 046, формат=4 (горизонт.перемещ)
                //%10$18$00$60$t3              - текст, $18$00$60$t3,  (строка 3пробела)                  [ВРЕМЯ]
                else
                {
                    // %01 - задание формата вывода ПУТИ
                    // 001 - Х1
                    // 146 - X2
                    // 014 - Y
                    // аттриб = 4 (бег.стр.)
                    format2 = "%000011460144";
                    message2 = $"%10$18$00$60$t2ПУТЬ №{numberOfPath}";
                    result2 = format2 + message2;

                    // %01 - задание формата вывода события
                    // 152 - Х1
                    // 192 - X2
                    // 014 - Y
                    // аттриб = 4 (бег.стр.)
                    format3 = "%001521920144";
                    message3 = $"%10$18$00$60$t3{ev}";
                    result3 = format3 + message3;

                    // %01 - задание формата вывода слова ПОЕЗД№
                    // 152 - Х1
                    // 192 - X2
                    // 014 - Y
                    // аттриб = 4 (бег.стр.)
                    format4 = "%000010750314";
                    message4 = "%10$18$00$60$t3Поезд №";
                    result4 = format4 + message4;

                    // %42 - вывод гор. линии
                    // 001 - Х1
                    // 192 - Y1
                    // 016 - Y2
                    // 016 - ширина
                    message5 = "%42001192016016";
                    result5 = message5;

                    // %01 - задание формата номера поезда
                    // 077 - Х1
                    // 146 - X2
                    // 031 - Y1
                    // аттриб = 4 (бег.стр.)
                    format6 = "%000771460314";
                    message6 = $"%10$18$00$60$t3{numberOfTrain}";
                    result6 = format6 + message6;

                    // %01 - задание формата вывода времени
                    // 152 - Х1
                    // 192 - X2
                    // 046 - Y1
                    // аттриб = 4 (бег.стр.)
                    format7 = "%001521920314";
                    message7 = $"%10$18$00$60$t3{time}";
                    result7 = format7 + message7;


                    // %01 - задание формата вывода станции
                    // 077 - Х1
                    // 146 - X2
                    // 046 - Y1
                    // аттриб = 4 (бег.стр.)
                    //format7 = "%000011460464";
                    format8 = "%000011920464";                         //!!!
                    message8 = $"%10$18$00$60$t3{stations}";
                    result8 = format8 + message8;
                }


                //формируем КОНЕЧНУЮ строку
                var sumResult = result1 + result2 + result3 + result4 + result5 + result6 + result7 + result8;

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


            IsOutDataValid = true;
            return true;


            //if (data[0] == byte.Parse(InputData.Address) &&
            //    data[1] == CountSetDataByte)
            //{
            //    if (data[2] == 0x83)                         //успешно приняты
            //    {
            //        IsOutDataValid = true;
            //        return true;
            //    }

            //    if (data[2] == 0x80)                          //ошибка приема
            //    {
            //        OutputData.ErrorCode = data[3];
            //    }
            //}

            //IsOutDataValid = false;
            //return false;
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