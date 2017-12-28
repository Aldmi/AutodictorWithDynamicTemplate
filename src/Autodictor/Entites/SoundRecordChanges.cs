using System;
using Domain.Entitys;

namespace MainExample.Entites
{
    public class SoundRecordChanges
    {
        public int ScheduleId { get; set; }            //Id поезда основного расписания на базе которого был сделанн данный поезд
        public DateTime TimeStamp { get; set; }      //Время фиксации изменений
        public SoundRecord Rec { get; set; }         //До 
        public SoundRecord NewRec { get; set; }      //После  

        public string UserInfo { get; set; }           //Под кем работаем имя(роль)
        public string CauseOfChange { get; set; }      //Причина изменения (изменил пользоватекль UserInfo или Автоматика)
    }
}