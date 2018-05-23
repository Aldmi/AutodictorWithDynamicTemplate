using System;
using System.Collections.Generic;
using AutodictorBL.Models;


namespace MainExample.Services
{
    public enum StateTask { Disabled, Enable, Waiting }

    /// <summary>
    /// ТипСообщения- определяет список откуда пришло сообщение
    /// ParentId- Id родителя, стастика- null, динамика- СостояниеФормируемогоСообщенияИШаблон.Id
    /// Ключ- ключ root элемента в списке.
    /// StateString - состояние строки
    /// </summary>
    public class TaskSound
    {
        public DateTime Время;
        public string Описание;

        public MessageType MessageType { get; set; } // НомерСписка. Определяет в каком списке искать сообщение.

        public string Key;
        public int? ParentId { get; set; }  //Id родителя, стастика- null, динамика- ActionTrainDynamic.Id

        //public byte СостояниеСтроки;        //DELL  0 - Выключена, 1 - движение поезда (динамика), 2 - статическое сообщение, 3 - аварийное сообщение, 4 - воспроизведение
        public StateTask StateTask { get; set; }// Убрать СостояниеСтроки.   
    };





    public class TaskManagerService
    {
        public SortedDictionary<string, TaskSound> Tasks { get; } = new SortedDictionary<string, TaskSound>();


        public IEnumerable<TaskSound> GetElements => Tasks.Values;       //???
        public int Count => Tasks.Count;




        /// <summary>
        /// Добавить задачу
        /// </summary>
        public void AddItem(TaskSound taskSound)
        {
            int количествоПопыток = 0;
            while (количествоПопыток++ < 60)
            {
                var key = taskSound.Время.ToString("yy.MM.dd  HH:mm:ss");
                string[] parts = key.Split(':');
                if (parts[0].Length == 1) key = "0" + key;

                if (Tasks.ContainsKey(key) == false)
                {
                    Tasks.Add(key, taskSound);
                    break;
                }

                taskSound.Время= taskSound.Время.AddSeconds(1);
            }
        }


        public void Clear()
        {
            Tasks.Clear();
        }
    }
}