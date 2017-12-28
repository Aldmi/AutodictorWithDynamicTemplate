using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Convertion
{
   public class Numeric2ListStringConverter
    {
        #region prop

        private readonly string _eof = "X"; //Символ интонации завершения

        private string[] HundredsNames { get; } = { "0", "100", "200", "300", "400", "500", "600", "700", "800", "900" };
        private string[] DozensNames { get; } = { "0", string.Empty, "20", "30", "40", "50", "60", "70", "80", "90" };
        private string[] UnitsNames { get; } = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19" };

        #endregion




        #region ctor

        public Numeric2ListStringConverter(string eof)
        {
            _eof = eof;
        }

        #endregion




        #region Methode

        public List<string> Convert(string num)
        {
            if (string.IsNullOrEmpty(num))
                return null;

            //Дополним число до 3 разрядов
            if (num.Length != 3)
            {
                int intNum;
                if (int.TryParse(num, out intNum))
                {
                    num = intNum.ToString("D3");
                }
                else
                {
                    return null;
                }
            }

            var chars = num.ToCharArray();
            if (chars.Length != 3)
                return null;


            int hundreds = 0;    //Сотни
            int dozens = 0;      //Десятки
            int units = 0;       //Единицы

            if (int.TryParse(chars[0].ToString(), out hundreds) &&
                int.TryParse(chars[1].ToString(), out dozens) &&
                int.TryParse(chars[2].ToString(), out units))
            {
                List<string> outList = new List<string>();

                //СОТНИ ИМЯ ФАЙЛА
                var hundredName = HundredsNames[hundreds];
                if (dozens == 0 && units == 0)
                {
                    hundredName += _eof;
                    outList.Add(hundredName);
                    return outList;
                }
                outList.Add(hundredName);

                //ДЕСЯТКИ ИМЯ ФАЙЛА
                if (!(dozens == 0 && hundreds != 0 && units != 0)) //  десятки не могу быть 0, если сотни и единицы не 0
                {
                    string dozensName = String.Empty;
                    if (dozens == 1) //10,11,12,13,14,15,16,17,18,19
                    {
                        dozensName = "1" + UnitsNames[units] + _eof;
                        outList.Add(dozensName);
                        return outList;
                    }
                    dozensName = DozensNames[dozens];
                    if (units == 0)
                    {
                        dozensName += _eof;
                    }
                    outList.Add(dozensName);
                }

                //ЕДИНИЦЫ ИМЯ ФАЙЛА
                if (units != 0)
                {
                    var unitsName = UnitsNames[units];
                    unitsName += _eof;                          //все единицы завершаюшие
                    outList.Add(unitsName);
                }

                return outList;
            }
            return null;
        }

        #endregion
    }
}
