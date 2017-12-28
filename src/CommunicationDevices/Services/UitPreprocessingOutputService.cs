using System.Collections.Generic;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Services
{
    public interface IUitPreprocessing
    {
        void StartPreprocessing(UniversalInputType uit);
    }





    public class UitPreprocessingOutputService
    {
        public IEnumerable<IUitPreprocessing> PreprocessingList { get; set; }



        public UitPreprocessingOutputService(IEnumerable<IUitPreprocessing> preprocessingList)
        {
            PreprocessingList = preprocessingList;
        }



        public void StartPreprocessing(UniversalInputType uit)
        {
            foreach (var item in PreprocessingList)
            {
                item.StartPreprocessing(uit);
            }
        }
    }
}