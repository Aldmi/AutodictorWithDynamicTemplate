using System;
using System.Collections.Generic;
using DAL.Abstract.Entitys;

namespace DAL.NoSqlLiteDb.Entityes
{

    public class SoundRecordDb : EntityBase
    {
        public string НомерПоезда { get; set; }
        public string НомерПоезда2 { get; set; }
        public string НазваниеПоезда { get; set; }
        public string Направление { get; set; }
        public string СтанцияОтправления { get; set; }
        public string СтанцияНазначения { get; set; }
        public DateTime Время { get; set; }
        public DateTime ВремяПрибытия { get; set; }
        public DateTime ВремяОтправления { get; set; }
        public DateTime? ВремяЗадержки { get; set; }                      //время задержки в мин. относительно времени прибытия или отправелния
        public DateTime ОжидаемоеВремя { get; set; }                      //вычисляется ВремяПрибытия или ВремяОтправления + ВремяЗадержки
        public DateTime? ВремяСледования { get; set; }                    //время в пути
        public TimeSpan? ВремяСтоянки { get; set; }                       //вычисляется для танзитов (ВремяОтправления - ВремяПрибытия)
        public DateTime? ФиксированноеВремяПрибытия { get; set; }         // фиксированное время
        public DateTime? ФиксированноеВремяОтправления { get; set; }      // фиксированное время + время стоянки
        public string Дополнение { get; set; }                            //свободная переменная для ввода  
        public Dictionary<string, bool> ИспользоватьДополнение { get; set; } //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public string ДниСледования { get; set; }
        public string ДниСледованияAlias { get; set; }                    // дни следования записанные в ручную
        public bool Активность { get; set; }
        public bool Автомат { get; set; }                                 // true - поезд обрабатывается в автомате.
        public string ШаблонВоспроизведенияСообщений { get; set; }
        public byte НумерацияПоезда { get; set; }
        public string НомерПути { get; set; }
        public string НомерПутиБезАвтосброса { get; set; }                //выставленные пути не обнуляются через определенное время
        public int IdТипПоезда { get; set; }                              // В БД хранится Id типа TrainTypeByRyle.
        public string Примечание { get; set; }                            //С остановками....
        public string Описание { get; set; }
        public SoundRecordStatus Состояние { get; set; }
        public SoundRecordType ТипСообщения { get; set; }
        public byte БитыАктивностиПолей { get; set; }
        public string[] НазванияТабло { get; set; }
        public TableRecordStatus СостояниеОтображения { get; set; }
        public PathPermissionType РазрешениеНаОтображениеПути { get; set; }
        public string[] ИменаФайлов { get; set; }
        public byte КоличествоПовторений { get; set; }
        public byte СостояниеКарточки { get; set; }
        public string ОписаниеСостоянияКарточки { get; set; }
        public Emergency Emergency { get; set; }
        public uint ТаймерПовторения { get; set; }
    }
}