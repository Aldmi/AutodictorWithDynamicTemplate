using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using LedScreenLibNetWrapper;


namespace Communication.SibWayApi
{
    /// <summary>
    /// Класс настроек окна для отображения инофрмации
    /// </summary>
    public class WindowSett
    {
        public readonly byte Number;  //Номер окна
        public readonly string ColumnName; //Название поля для вывода, т.е. имя переменной отображаемой в этом окне
        public readonly int FontSize;        //Размер шрифта 8/12/16/24/32 пикселя в высоту. DisplayTextHeight для каждого окна берется соответсвующий.
        public readonly int Width;  //Ширина окна
        public readonly int Height;  //Высота окна
        public readonly DisplayEffect Effect; //Эффект отображения
        public readonly DisplayTextHAlign TextHAlign;//Выравнивание по горизонтали
        public readonly DisplayTextVAlign TextVAlign; //Выравнивание по вертикали
        public readonly ushort DisplayTime; //???
        public readonly int DelayBetweenSending; // задержка времени в мсек, на отправку инфы между экранами.
        public readonly byte[] ColorBytes;//Цвет { color.B, color.G, color.R, 0x00 }
        public readonly string Format; //Формат для формирования строки вывода.


        public WindowSett(string number, string columnName, string fontSize, string width, string height, string effect, string textHAlign, string textVAlign, string displayTime, string delayBetweenSending, string colorBytes, string format)
        {
            Number = byte.Parse(number);
            ColumnName = columnName;
            FontSize = int.Parse(fontSize);
            Width = int.Parse(width);
            Height = int.Parse(height);
            Effect = (DisplayEffect)Enum.Parse(typeof(DisplayEffect), effect);
            TextHAlign = (DisplayTextHAlign)Enum.Parse(typeof(DisplayTextHAlign), textHAlign);
            TextVAlign = (DisplayTextVAlign)Enum.Parse(typeof(DisplayTextVAlign), textVAlign);
            DisplayTime = ushort.Parse(displayTime);
            DelayBetweenSending = int.Parse(delayBetweenSending);

            var r = byte.Parse(colorBytes.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            var g = byte.Parse(colorBytes.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            var b = byte.Parse(colorBytes.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            ColorBytes = new byte[] { b, g, r, 0x00 };

            Format = format;
        }
    }




    public class SettingSibWay
    {
        public readonly Dictionary<int, string> Path2FontFileDictionary; //= Application.StartupPath + @"\LEDFont8px.xml";
        public readonly string Ip;
        public readonly ushort Port;
        public readonly int TimeRespown;
        public readonly int Time2Reconnect;
        public readonly int NumberTryingTakeData;


        public IEnumerable<WindowSett> WindowSett { get; set; }




        public SettingSibWay(string ip, string port, Dictionary<int, string> path2FontFileDictionary, string timeRespown, string time2Reconnect, string numberTryingTakeData)
        {
            Ip = ip;
            Port = ushort.Parse(port);
            Path2FontFileDictionary = path2FontFileDictionary;
            TimeRespown = int.Parse(timeRespown);
            Time2Reconnect = int.Parse(time2Reconnect);
            NumberTryingTakeData = int.Parse(numberTryingTakeData);
        }
    }
}