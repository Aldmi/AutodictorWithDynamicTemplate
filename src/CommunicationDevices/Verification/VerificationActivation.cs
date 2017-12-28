using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Forms;


namespace CommunicationDevices.Verification
{
    public interface IVerificationActivation
    {
        bool IsBlock { get; set; }
        int GetDeltaDay();
        int GetDeltaDayBeforeBlocking();
        bool ResetActivation(string confirmationPassword);
    }




    public class VerificationActivation : IVerificationActivation
    {

        #region prop

        private const string PathSetings = @"Activation\Key.txt";

        public bool IsBlock { get; set; }
        public DateTime LastActivationDate { get; set; }
        public string Password { get; set; }

        #endregion




        #region ctor

        public VerificationActivation()
        {
            Load();
        }

        #endregion




        #region RxEvent

        public Subject<IVerificationActivation> WarningInvokeRx { get; } = new Subject<IVerificationActivation>(); // предупреждение, передаем дату последней активации

        #endregion





        #region Methode

        public void Load()
        {
            try
            {
                using (var sr = new StreamReader(PathSetings, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var arr = line.Split('=').Select(s=>s.Trim()).ToArray();
                        if (arr.Length == 2)
                        {
                            switch (arr[0])
                            {
                                case "LastActivationDate":
                                    LastActivationDate = DateTime.Parse(AsciiBytes2StringConverter(arr[1]));
                                    break;

                                case "Password":
                                   Password = AsciiBytes2StringConverter(arr[1]);
                                  break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //загрузить дату последней активации
            //LastActivationDate = new DateTime(2017, 09, 12);
            //Password = "123456";
        }


        /// <summary>
        /// Сохранить дату активации и ключ
        /// </summary>
        private void Save()
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), PathSetings);
            if (File.Exists(fullPath))
            {
                try
                {
                    using (var sw = new StreamWriter(fullPath, false, Encoding.Default))
                    {
                       var asciiBytes = Encoding.ASCII.GetBytes(LastActivationDate.ToString("d"));
                       var resultStr = asciiBytes.Aggregate(string.Empty, (current, b) => current + $"{b} ");
                       sw.WriteLine($"LastActivationDate = {resultStr}");

                       asciiBytes= Encoding.ASCII.GetBytes(Password);
                       resultStr = asciiBytes.Aggregate(string.Empty, (current, b) => current + $"{b} ");
                       sw.WriteLine($"Password = {resultStr}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }


        private string AsciiBytes2StringConverter(string asciiBytesOnStr)
        {
            var bytes = new List<byte>();
            foreach (var str in asciiBytesOnStr.Split(' '))
            {
                byte res;
                if (byte.TryParse(str, out res))
                {
                    bytes.Add(res);
                }
            }

           return Encoding.ASCII.GetString(bytes.ToArray());
        }


        /// <summary>
        /// кол-во дней от последней активации
        /// </summary>
        public int GetDeltaDay()
        {
            return (DateTime.Now - LastActivationDate).Days;
        }

        /// <summary>
        /// кол-во дней до блокировки
        /// </summary>
        public int GetDeltaDayBeforeBlocking()
        {
            var blockingDay = 90 - GetDeltaDay();
            if (blockingDay < 0)
                blockingDay = 0;

            return blockingDay;
        }


        public bool ResetActivation(string confirmationPassword)
        {
            if (Password == confirmationPassword)
            {
                IsBlock = false;
                LastActivationDate = DateTime.Now;
                Save();
                return true;
            }
            return false;
        }




        /// <summary>
        /// 60...80 дней
        ///  Вызвать раз в день
        /// </summary>
        public void CheckActivation_60To80()
        {
            var deltaDay = GetDeltaDay();
            if (deltaDay > 60 && deltaDay < 80)
            {
                WarningInvokeRx.OnNext(this);
            }
        }


        /// <summary>
        /// 80...89 дней
        /// раз в 8 часов (3р в день)
        /// </summary>
        public void CheckActivation_80To89()
        {
            var deltaDay = GetDeltaDay();
            if (deltaDay > 80 && deltaDay < 89)
            {
                WarningInvokeRx.OnNext(this);
            }
        }


        /// <summary>
        /// 89..90 дней
        /// раз в час
        /// более 90 дней
        /// блокировка
        /// </summary>
        public void CheckActivation_89To90()
        {
            var deltaDay = GetDeltaDay();
            if (deltaDay >= 89 && deltaDay <= 90)
            {
                WarningInvokeRx.OnNext(this);
            }
            else if (deltaDay > 90)
            {
                IsBlock = true;
                WarningInvokeRx.OnNext(this);
            }
        }

        #endregion

    }
}