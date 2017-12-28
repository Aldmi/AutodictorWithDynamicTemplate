using System.Collections.Generic;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Services;

namespace MainExample.Services.FactoryServices
{
    public static class PreprocessingOutputFactory
    {
        public static UitPreprocessingOutputService CreateUitPreprocessingOutputService()
        {
            //Dictionary<TypeTrain, int> typeTrainTimeZoneDictionary = new Dictionary<TypeTrain, int>
            //{
            //    [TypeTrain.Passenger] = Program.Настройки.TimeZoneНаПассажирскийПоезд,
            //    [TypeTrain.Suburban] = Program.Настройки.TimeZoneНаПригородныйЭлектропоезд,
            //    [TypeTrain.Corporate] = Program.Настройки.TimeZoneНаФирменный,
            //    [TypeTrain.Express] = Program.Настройки.TimeZoneНаСкорыйПоезд,
            //    [TypeTrain.HighSpeed] = Program.Настройки.TimeZoneНаСкоростнойПоезд,
            //    [TypeTrain.Swallow] = Program.Настройки.TimeZoneНаЛасточку,
            //    [TypeTrain.Rex] = Program.Настройки.TimeZoneНаРЭКС
            //};
            return new UitPreprocessingOutputService(new List<IUitPreprocessing>
            {
                new UitPreprocessingChnagePathDirection4Transit()
            });
        }



        public static  SoundRecordPreprocessingService CreateSoundRecordPreprocessingService(Dictionary<string, dynamic> option)
        {
            //return new SoundRecordPreprocessingService(new List<ISoundRecordPreprocessing>());

            return new SoundRecordPreprocessingService(new List<ISoundRecordPreprocessing>
            {
               new SoundRecordPreprocessingChangeTrainPathDirection4Transit(option)
            });
        }
    }
}