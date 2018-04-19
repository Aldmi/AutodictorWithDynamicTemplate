using CommunicationDevices.DataProviders;


namespace CommunicationDevices.Services
{
    public class UitPreprocessingTimezone : IUitPreprocessing
    {
        private int DeltaTimezoneMinute { get; set; }
        //public Dictionary<TypeTrain, int> TypeTrainTimeZoneDictionary { get; set; }



        //public UitPreprocessingTimezone(Dictionary<TypeTrain, int> typeTrainTimeZoneDictionary)
        //{
        //    TypeTrainTimeZoneDictionary = typeTrainTimeZoneDictionary;
        //}



        public void StartPreprocessing(UniversalInputType uit)
        {
            //switch (uit.TypeTrain)
            //{
            //    case TypeTrain.Passenger:
            //        DeltaTimezoneMinute = TypeTrainTimeZoneDictionary.ContainsKey(TypeTrain.Passenger) ? TypeTrainTimeZoneDictionary[TypeTrain.Passenger] : 0;
            //        break;

            //    case TypeTrain.Suburban:
            //        DeltaTimezoneMinute = TypeTrainTimeZoneDictionary.ContainsKey(TypeTrain.Suburban) ? TypeTrainTimeZoneDictionary[TypeTrain.Suburban] : 0;
            //        break;

            //    case TypeTrain.Corporate:
            //        DeltaTimezoneMinute = TypeTrainTimeZoneDictionary.ContainsKey(TypeTrain.Corporate) ? TypeTrainTimeZoneDictionary[TypeTrain.Corporate] : 0;
            //        break;

            //    case TypeTrain.Express:
            //        DeltaTimezoneMinute = TypeTrainTimeZoneDictionary.ContainsKey(TypeTrain.Express) ? TypeTrainTimeZoneDictionary[TypeTrain.Express] : 0;
            //        break;

            //    case TypeTrain.HighSpeed:
            //        DeltaTimezoneMinute = TypeTrainTimeZoneDictionary.ContainsKey(TypeTrain.HighSpeed) ? TypeTrainTimeZoneDictionary[TypeTrain.HighSpeed] : 0;
            //        break;

            //    case TypeTrain.Swallow:
            //        DeltaTimezoneMinute = TypeTrainTimeZoneDictionary.ContainsKey(TypeTrain.Swallow) ? TypeTrainTimeZoneDictionary[TypeTrain.Swallow] : 0;
            //        break;

            //    case TypeTrain.Rex:
            //        DeltaTimezoneMinute = TypeTrainTimeZoneDictionary.ContainsKey(TypeTrain.Rex) ? TypeTrainTimeZoneDictionary[TypeTrain.Rex] : 0;
            //        break;
            //}

            //uit.Time = uit.Time.AddMinutes(DeltaTimezoneMinute);
            //uit.ExpectedTime = uit.ExpectedTime.AddMinutes(DeltaTimezoneMinute);
            //if (uit.TransitTime.ContainsKey("приб"))
            //{
            //    uit.TransitTime["приб"]= uit.TransitTime["приб"].AddMinutes(DeltaTimezoneMinute);
            //}
            //if (uit.TransitTime.ContainsKey("отпр"))
            //{
            //    uit.TransitTime["отпр"] = uit.TransitTime["отпр"].AddMinutes(DeltaTimezoneMinute);
            //}
        }
    }
}