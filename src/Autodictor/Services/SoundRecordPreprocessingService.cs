using System.Collections.Generic;
using DAL.Abstract.Entitys;


namespace MainExample.Services
{
    public interface ISoundRecordPreprocessing
    {
        void StartPreprocessing(ref SoundRecord rec);
    }




    public class SoundRecordPreprocessingTimezone : ISoundRecordPreprocessing
    {
        private int DeltaTimezoneMinute { get; set; }



        public void StartPreprocessing(ref SoundRecord rec)
        {
            //switch (rec.ТипПоезда)
            //{
            //    case ТипПоезда.Пассажирский:
            //        DeltaTimezoneMinute = Program.Настройки.TimeZoneНаПассажирскийПоезд;
            //        break;

            //    case ТипПоезда.Пригородный:
            //        DeltaTimezoneMinute = Program.Настройки.TimeZoneНаПригородныйЭлектропоезд;
            //        break;

            //    case ТипПоезда.Фирменный:
            //        DeltaTimezoneMinute = Program.Настройки.TimeZoneНаФирменный;
            //        break;

            //    case ТипПоезда.Скорый:
            //        DeltaTimezoneMinute = Program.Настройки.TimeZoneНаСкорыйПоезд;
            //        break;

            //    case ТипПоезда.Скоростной:
            //        DeltaTimezoneMinute = Program.Настройки.TimeZoneНаСкоростнойПоезд;
            //        break;

            //    case ТипПоезда.Ласточка:
            //        DeltaTimezoneMinute = Program.Настройки.TimeZoneНаЛасточку;
            //        break;

            //    case ТипПоезда.РЭКС:
            //        DeltaTimezoneMinute = Program.Настройки.TimeZoneНаРЭКС;
            //        break;
            //}

            rec.ВремяОтправления = rec.ВремяОтправления.AddMinutes(DeltaTimezoneMinute);
            rec.ВремяПрибытия = rec.ВремяПрибытия.AddMinutes(DeltaTimezoneMinute);
            rec.Время = rec.Время.AddMinutes(DeltaTimezoneMinute);
        }
    }



    public class SoundRecordPreprocessingChangeTrainPathDirection4Transit : ISoundRecordPreprocessing
    {
        private readonly ActionTrain _шаблон;



        public SoundRecordPreprocessingChangeTrainPathDirection4Transit(Dictionary<string, dynamic> option)
        {
            if (option.ContainsKey("формируемоеСообщение"))
            {
                _шаблон = (ActionTrain)option["формируемоеСообщение"];
            }
        }




        public void StartPreprocessing(ref SoundRecord rec)
        {
            if (_шаблон == null)
                return;

            //для ТРНАЗИТОВ
            if ((rec.БитыАктивностиПолей & 0x1F) != 0x00)
            {
                // var привязкаКВремени = _шаблон.Value.ПривязкаКВремени;
                //var шаблонОтпр  =_шаблон.Value.НазваниеШаблона.StartsWith("[ОТПР]");
                //if (rec.СменнаяНумерацияПоезда && шаблонОтпр)
                //{
                //    switch (rec.НумерацияПоезда)
                //    {
                //        case 1:
                //            rec.НумерацияПоезда = 2;
                //            break;
                //        case 2:
                //            rec.НумерацияПоезда = 1;
                //            break;
                //    }
                //}
            }
        }
    }




    public class SoundRecordPreprocessingService
    {
        public IEnumerable<ISoundRecordPreprocessing> PreprocessingList { get; set; }



        public SoundRecordPreprocessingService(IEnumerable<ISoundRecordPreprocessing> preprocessingList)
        {
            PreprocessingList = preprocessingList;
        }



        public void StartPreprocessing(ref SoundRecord rec)
        {
            foreach (var item in PreprocessingList)
            {
                item.StartPreprocessing(ref rec);
            }
        }
    }
}