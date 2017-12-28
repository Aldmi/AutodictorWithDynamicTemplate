using System;
using System.Collections;
using System.Collections.Generic;
using MainExample.Entites;
using NLog.LayoutRenderers.Wrappers;

namespace MainExample.Services
{
    public class TaskSound
    {
        public DateTime Время;
        public string Описание;
        public byte НомерСписка;            // 0 - Динамические сообщения, 1 - статические звуковые сообщения, 3 - нештатное сообщение
        public string Ключ;
        public int? ParentId { get; set; }  //Id родителя, стастика- null, динамика- СостояниеФормируемогоСообщенияИШаблон.Id
        public byte СостояниеСтроки;        // 0 - Выключена, 1 - движение поезда (динамика), 2 - статическое сообщение, 3 - аварийное сообщение, 4 - воспроизведение
        public string ШаблонИлиСообщение;   //текст стат. сообщения, или номер шаблона в динам. сообщении (для Субтитров)
    };





    public class TaskManagerService
    {
        public SortedDictionary<string, TaskSound> Tasks { get; private set; } = new SortedDictionary<string, TaskSound>();


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