namespace DAL.Abstract.Entitys
{
    //НОВАЯ РЕАЛИЗАЦИЯ
    public class SoundRec
    {
        public int Id { get; set; }
        public IdTrain IdTrain { get; set; }                                   //Уникальный Id (связь)
        public string Num { get; set; }                                        //Номер поезда
        public string Num2 { get; set; }                                       //Номер поезда 2 (для транзита)
        public string Name { get; set; }                                       //Название поезда
        public Route Route { get; set; }                                       //Маршрут (список станций)
    }
}