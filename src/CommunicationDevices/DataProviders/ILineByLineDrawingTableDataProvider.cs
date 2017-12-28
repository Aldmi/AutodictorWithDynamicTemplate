using Communication.Interfaces;

namespace CommunicationDevices.DataProviders
{
    //Отрисовка таблицы построчно, один запрос - одна строка.
    public interface ILineByLineDrawingTableDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        byte CurrentRow { get; set; }              //Текущая строка для отрисовки
    }
}