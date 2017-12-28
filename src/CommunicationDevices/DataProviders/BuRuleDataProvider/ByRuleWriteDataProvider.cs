using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using Communication.Annotations;
using Communication.Interfaces;
using CommunicationDevices.Rules.ExchangeRules;


namespace CommunicationDevices.DataProviders.BuRuleDataProvider
{
    public class ByRuleWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {

        #region Prop

        public int CountGetDataByte { get; }
        public int CountSetDataByte { get; }

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; set; }

        public bool IsOutDataValid { get; private set; }
        public Subject<byte> OutputDataChangeRx { get; } = null;
        public string ProviderName { get; set; }

        public RequestRule RequestRule { get; set; }
        public ResponseRule ResponseRule { get; set; }

        public string Format { get; set; }

        #endregion




        #region ctor

        public ByRuleWriteDataProvider(BaseExchangeRule baseExchangeRule)
        {
            RequestRule = baseExchangeRule.RequestRule;
            ResponseRule = baseExchangeRule.ResponseRule;
            Format = baseExchangeRule.Format;

            CountSetDataByte = ResponseRule.MaxLenght ?? ResponseRule.Body.Length;
        }

        #endregion





        public byte[] GetDataByte()
        {
            try
            {
                var requestFillBody = RequestRule.GetFillBody(InputData, null);
                string matchString = null;

                var requestFillBodyWithoutConstantCharacters = requestFillBody.Replace("STX", string.Empty).Replace("ETX", string.Empty);


                //ВЫЧИСЛЯЕМ NByte---------------------------------------------------------------------------
                int lenght = 0;
               
                if (Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Success)            //вычислили длинну строгки между Nbyte и CRC
                {
                    matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*){CRC(.*)}").Groups[2].Value;
                    lenght = matchString.Length;
                }
                else
                if (Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*)").Success)                     //вычислили длинну строки от Nbyte до конца строки
                {
                    matchString = Regex.Match(requestFillBodyWithoutConstantCharacters, "{Nbyte(.*)}(.*)").Groups[1].Value;
                    lenght = matchString.Length;                   
                }


                //ОГРАНИЧНИЕ ДЛИННЫ ПОСЫЛКИ------------------------------------------------------------------
                var limetedStr = requestFillBodyWithoutConstantCharacters;
                if (RequestRule.MaxLenght.HasValue && lenght >= RequestRule.MaxLenght)
                {
                    var removeCount = lenght - RequestRule.MaxLenght.Value;
                    limetedStr = matchString.Remove(RequestRule.MaxLenght.Value, removeCount);
                    lenght = limetedStr.Length;
                    requestFillBodyWithoutConstantCharacters =requestFillBodyWithoutConstantCharacters.Replace(matchString, limetedStr);   
                }




                //ЗАПОНЯЕМ ВСЕ СЕКЦИИ ДО CRC
                var subStr = requestFillBodyWithoutConstantCharacters.Split('}');
                StringBuilder resStr = new StringBuilder();
                foreach (var s in subStr)
                {
                    var replaseStr = (s.Contains("{")) ? (s + "}") : s;
                    if (replaseStr.Contains("Nbyte"))
                    {
                        var formatStr = string.Format(replaseStr.Replace("Nbyte", "0"), lenght);
                        resStr.Append(formatStr);
                    }
                    else
                    {
                        resStr.Append(replaseStr);
                    }
                }

                //ВЫЧИСЛЯЕМ CRC---------------------------------------------------------------------------
                var resultStr = resStr.ToString();  
                matchString = Regex.Match(resultStr, "(.*){CRC(.*)}").Groups[1].Value;

                byte[] xorBytes;
                if (Format == "HEX")
                {
                    xorBytes = new byte[10]; //DEBUG

                    //Распарсить строку matchString в масив байт как она есть. 0203АА96 ...
                }
                else
                {
                    xorBytes = Encoding.GetEncoding(Format).GetBytes(matchString);
                }

                
                //вычислить CRC по правилам XOR
                if (resultStr.Contains("CRCXor"))
                {
                    byte xor = CalcXor(xorBytes);
                    resultStr = string.Format(resultStr.Replace("CRCXor", "0"), xor);
                }


                //Преобразовываем КОНЕЧНУЮ строку в массив байт
                List<byte> resultBuffer;
                if (Format == "HEX")
                {
                    resultBuffer = new List<byte>(); //DEBUG
                    //Распарсить строку в масив байт как она есть. 0203АА96 ...
                }
                else
                {
                    resultBuffer = Encoding.GetEncoding(Format).GetBytes(resultStr).ToList();
                }


                //ВСТАВКА КОНСТАНТНЫХ СИМВОЛОВ--------------------------------------------------------------------------
                if (Regex.Match(requestFillBody, "^STX").Success)
                    resultBuffer.Insert(0, 0x02); //STX

                if (Regex.Match(requestFillBody, "ETX").Success)
                    resultBuffer.Add(0x03);       //ETX

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



        public bool SetDataByte(byte[] data)
        {
            if (data == null || data.Length != CountSetDataByte)
            {
                IsOutDataValid = false;
                return false;
            }


            var responseFillBody = ResponseRule.GetFillBody(InputData, null);
            string matchString = null;

            var requestFillBodyWithoutConstantCharacters = responseFillBody.Replace("STX", string.Empty).Replace("ETX", string.Empty);
            var resultStr = requestFillBodyWithoutConstantCharacters;

            //Преобразовываем КОНЕЧНУЮ строку в массив байт
            List<byte> resultBuffer;
            if (Format == "HEX")
            {
                resultBuffer = new List<byte>(); //DEBUG
                                                 //Распарсить строку в масив байт как она есть. 0203АА96 ...
            }
            else
            {
                resultBuffer = Encoding.GetEncoding(Format).GetBytes(resultStr).ToList();
            }

            //ВСТАВКА КОНСТАНТНЫХ СИМВОЛОВ--------------------------------------------------------------------------
            if (Regex.Match(responseFillBody, "^STX").Success)
                resultBuffer.Insert(0, 0x02); //STX

            if (Regex.Match(responseFillBody, "ETX").Success)
                resultBuffer.Add(0x03);       //ETX


            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != resultBuffer[i])
                {
                    IsOutDataValid = false;
                    return false;
                }
            }

            IsOutDataValid = true;
            return true;
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