﻿using DAL.Abstract.Entitys;


namespace MainExample.Infrastructure
{
    public static class StructCompare
    {
        public static bool SoundRecordComparer (ref SoundRecord sr1, ref SoundRecord sr2)
        {
            return (sr1.ВремяОтправления == sr2.ВремяОтправления) &&
                   (sr1.ВремяПрибытия == sr2.ВремяПрибытия) &&
                   (sr1.ВремяСтоянки == sr2.ВремяСтоянки) &&
                   (sr1.ВремяЗадержки == sr2.ВремяЗадержки) &&
                   (sr1.ОжидаемоеВремя == sr2.ОжидаемоеВремя) &&
                   (sr1.ВремяСледования == sr2.ВремяСледования) &&
                   (sr1.ДниСледования == sr2.ДниСледования) &&
                   (sr1.СтанцияНазначения == sr2.СтанцияНазначения) &&
                   (sr1.СтанцияОтправления == sr2.СтанцияОтправления) &&
                   (sr1.НазваниеПоезда == sr2.НазваниеПоезда) &&
                   (sr1.НомерПоезда == sr2.НомерПоезда) &&
                   (sr1.НомерПоезда2 == sr2.НомерПоезда2) &&
                   (sr1.Route.ToString() == sr2.Route.ToString()) &&
                   (sr1.РазрешениеНаОтображениеПути == sr2.РазрешениеНаОтображениеПути) &&
                   (sr1.Активность == sr2.Активность) &&
                   (sr1.Emergency == sr2.Emergency) &&
                   (sr1.НомерПути == sr2.НомерПути) &&
                   (sr1.Дополнение == sr2.Дополнение) &&
                   (sr1.Автомат == sr2.Автомат) &&
                   (sr1.Дополнение == sr2.Дополнение) &&
                   (sr1.ИспользоватьДополнение["табло"] == sr2.ИспользоватьДополнение["табло"]);
        }
    }
}