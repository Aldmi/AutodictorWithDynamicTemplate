using System.ComponentModel;
using System.Reactive.Subjects;


namespace Communication.Interfaces
{
    public interface IExchangeDataProvider<TInput, TOutput> : IExchangeDataProviderBase, INotifyPropertyChanged
    {
        TInput InputData { get; set; }     //передача входных даных внешним кодом.
        TOutput OutputData { get; set; }   //возврат выходных данных во внешний код.
        bool IsOutDataValid { get; }       // флаг валидности выходных данных (OutputData)


        Subject<TOutput> OutputDataChangeRx { get; }
        string ProviderName { get; set; }
    }
}