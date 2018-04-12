using System;
using System.Collections.Generic;

namespace DAL.Abstract.Entitys
{
    public struct SoundRecord
    {
        public int Id;
        public IdTrain IdTrain;
        public bool Активность;
        public string НомерПоезда;
        public string НомерПоезда2;
        public string НазваниеПоезда;

        public string Направление;
        public Direction Direction { get; set; }             // NEW Направление (readonly)

        public string СтанцияОтправления;
        public Station StationDepart { get; set; }           // NEW станция отправления (not Copy. not Edit). 

        public string СтанцияНазначения;
        public Station StationArrival { get; set; }          // NEW станция прибытия (not Copy. not Edit). 

        public DateTime Время;
        public DateTime ВремяПрибытия;
        public DateTime ВремяОтправления;

        public Classification Classification { get; set; }   // NEW Классификация поезда (readonly)

        public DateTime? ВремяЗадержки;                      //время задержки в мин. относительно времени прибытия или отправелния
        public DateTime ОжидаемоеВремя;                      //вычисляется ВремяПрибытия или ВремяОтправления + ВремяЗадержки
        public DateTime? ВремяСледования;                    //время в пути
        public TimeSpan? ВремяСтоянки;                       //вычисляется для танзитов (ВремяОтправления - ВремяПрибытия)
        public DateTime? ФиксированноеВремяПрибытия;         // фиксированное время
        public DateTime? ФиксированноеВремяОтправления;      // фиксированное время + время стоянки

        public string Дополнение;                            //свободная переменная для ввода  
        public Dictionary<string, bool> ИспользоватьДополнение; //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public string ДниСледования;
        public string ДниСледованияAlias;                    // дни следования записанные в ручную

        public bool Автомат;                                 // true - поезд обрабатывается в автомате.
        public string ШаблонВоспроизведенияСообщений;

        public byte НумерацияПоезда;                         // 1 - с головы,  2 - с хвоста
        public WagonsNumbering WagonsNumbering { get; set; }  //NEW (Copy. Edit).
        public bool СменнаяНумерацияПоезда;                  // для транзитов

        public string НомерПути;
        public string НомерПутиБезАвтосброса;                //выставленные пути не обнуляются через определенное время
        public Pathway Pathway;                              //NEW Выставленный Путь

        public TrainTypeByRyle ТипПоезда;                    //TODO: при переходе SoundRecord на class, ТипПоезда сделать readOnly
        public string Примечание;                            //С остановками....
        public string Описание;
        public SoundRecordStatus Состояние;                  //????
        public SoundRecordType ТипСообщения;                 //????
        public byte БитыАктивностиПолей;                     //???? Убрать

        public string[] НазванияТабло;                       //Сделать тип 
        public TableRecordStatus СостояниеОтображения;      


        public PathPermissionType РазрешениеНаОтображениеПути;
        public string[] ИменаФайлов;
        public byte КоличествоПовторений;

        public List<СостояниеФормируемогоСообщенияИШаблон> СписокФормируемыхСообщений;  //УДАЛИТЬ
        public List<ActionTrainDynamic> ActionTrainDynamiсList;    //Шаблоны поезда
        public List<ActionTrain> EmergencyTrainStaticList { get; set; }  //4 нештатных шаблона взятых из TrainTypeByRyle и отредактированных для данного поезда
        public List<ActionTrainDynamic> EmergencyTrainDynamiсList { get; set; } //N нештатных шаблона, сгенерированных при АКТИВНОЙ нештатной ситуации.

        public List<СостояниеФормируемогоСообщенияИШаблон> СписокНештатныхСообщений;
        public byte СостояниеКарточки;
        public string ОписаниеСостоянияКарточки;

        public byte БитыНештатныхСитуаций; //Убрать!!  бит 0 - Отмена, бит 1 - задержка прибытия, бит 2 - задержка отправления, бит 3 - отправление по готовности
        public Emergency Emergency { get; set; } // Текущая нештатная ситуация

        public uint ТаймерПовторения;

        public bool ВыводНаТабло;     // Работает только при наличии Contrains "SendingDataLimit".
        public bool ВыводЗвука;       //True - разрешен вывод звуковых шаблонов.



        #region Methode

        public void AplyIdTrain()
        {
            IdTrain.НомерПоезда = НомерПоезда;
            IdTrain.НомерПоезда2 = НомерПоезда2;
            IdTrain.СтанцияОтправления = СтанцияОтправления;
            IdTrain.СтанцияНазначения = СтанцияНазначения;
            IdTrain.ДеньПрибытия = ВремяПрибытия.Date;
            IdTrain.ДеньОтправления = ВремяОтправления.Date;
        }

        #endregion
    };


    /// <summary>
    /// ИДЕНТИФИКАТОР ПОЕЗДА.
    /// для сопоставления поезда из распсиания.
    /// </summary>
    public struct IdTrain
    {
        public IdTrain(int scheduleId) : this()
        {
            ScheduleId = scheduleId;
        }

        public int ScheduleId { get; }                   //Id поезда в распсиании
        public DateTime ДеньПрибытия { get; set; }      //сутки в которые поезд ПРИБ.  
        public DateTime ДеньОтправления { get; set; }   //сутки в которые поезд ОТПР.
        public string НомерПоезда { get; set; }        //номер поезда 1
        public string НомерПоезда2 { get; set; }       //номер поезда 2
        public string СтанцияОтправления { get; set; }
        public string СтанцияНазначения { get; set; }
    }

    public struct СостояниеФормируемогоСообщенияИШаблон
    {
        public int Id;                            // порядковый номер шаблона
        public int SoundRecordId;                 // строка расписания к которой принадлежит данный шаблон
        public bool Активность;
        public Priority ПриоритетГлавный;
        public PriorityPrecise ПриоритетВторостепенный;
        public bool Воспроизведен;                //???
        public SoundRecordStatus СостояниеВоспроизведения;
        public int ПривязкаКВремени;              // 0 - приб. 1- отпр
        public int ВремяСмещения;
        public string НазваниеШаблона;
        public string Шаблон;
        public List<NotificationLanguage> ЯзыкиОповещения;
    };

    //ВМЕСТО "СостояниеФормируемогоСообщенияИШаблон"
    public class ActionTrainDynamic
    {
        public int Id { get; set; }                                       // Id шаблона
        public int SoundRecordId { get; set; }                            // Id строки расписания к которой принадлежит данный шаблон
        public bool Activity { get; set; }                                // Разрешение работы всего шаблона
        public Priority PriorityMain { get; set; }                        // Проритет данного типа шаблонов
        public SoundRecordStatus SoundRecordStatus { get; set; }          // Статус воспроизведения

        public ActionTrain ActionTrain { get; set; }                      // Действие (Шаблоны)
    };


    public struct СтатическоеСообщение
    {
        public int ID;
        public DateTime Время;
        public string НазваниеКомпозиции;
        public string ОписаниеКомпозиции;
        public SoundRecordStatus СостояниеВоспроизведения;
        public bool Активность;
    };

    public struct ОписаниеСобытия
    {
        public DateTime Время;
        public string Описание;
        public byte НомерСписка;            // 0 - Динамические сообщения, 1 - статические звуковые сообщения
        public string Ключ;
        public byte СостояниеСтроки;        // 0 - Выключена, 1 - движение поезда (динамика), 2 - статическое сообщение, 3 - аварийное сообщение, 4 - воспроизведение, 5 - воспроизведЕН
        public string ШаблонИлиСообщение;   //текст стат. сообщения, или номер шаблона в динам. сообщении (для Субтитров)
    };

}