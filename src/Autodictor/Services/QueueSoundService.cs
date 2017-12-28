using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using AutodictorBL.Entites;
using AutodictorBL.Sound;
using Domain.Entitys;
using Library.Logs;
using MainExample.Entites;
using WCFCis2AvtodictorContract.DataContract;


namespace MainExample.Services
{
    public enum StatusPlaying { Start, Playing, Stop }


    public class StaticChangeValue
    {
        public StatusPlaying StatusPlaying { get; set; }
        public ВоспроизводимоеСообщение SoundMessage { get; set; }
    }


    public class TemplateChangeValue
    {
        public StatusPlaying StatusPlaying { get; set; }
        public СостояниеФормируемогоСообщенияИШаблон Template { get; set; }
        public ВоспроизводимоеСообщение SoundMessage { get; set; }
    }



    public class QueueSoundService : IDisposable
    {
        #region Field

        private readonly ISoundPlayer _soundPlayer;
        private int _pauseTime;

        #endregion




        #region prop

        private Queue<ВоспроизводимоеСообщение> Queue { get; set; } = new Queue<ВоспроизводимоеСообщение>();
        public IEnumerable<ВоспроизводимоеСообщение> GetElements => Queue;
        public int Count => Queue.Count;

        public bool IsStaticSoundPlaying => (MainWindowForm.QueueSound.CurrentTemplatePlaying == null) &&
                                            (MainWindowForm.QueueSound.CurrentSoundMessagePlaying != null);


        public ВоспроизводимоеСообщение CurrentSoundMessagePlaying { get; private set; }
        public СостояниеФормируемогоСообщенияИШаблон? CurrentTemplatePlaying { get; private set; }


        public ВоспроизводимоеСообщение LastSoundMessagePlayed { get; private set; }
        //последнее проигранное звуковое сообщение

        private List<ВоспроизводимоеСообщение> ElementsOnTemplatePlaying { get; set; }
        public IEnumerable<ВоспроизводимоеСообщение> GetElementsOnTemplatePlaying => ElementsOnTemplatePlaying;


        public IEnumerable<ВоспроизводимоеСообщение> GetElementsWithFirstElem
        {
            get
            {
                if (CurrentSoundMessagePlaying == null)
                    return new List<ВоспроизводимоеСообщение>();

                var result = new List<ВоспроизводимоеСообщение>();
                if (IsStaticSoundPlaying)
                {
                    result.Add(CurrentSoundMessagePlaying);
                    result.AddRange(GetElements);
                }
                else
                {
                    result.AddRange(GetElements);
                }

                return result;
            }
        }


        public bool IsWorking { get; private set; }

        public bool IsPlayedCurrentMessage { get; private set; }  //доиграть последнее сообщение и остановить очередь

        #endregion




        #region ctor

        public QueueSoundService(ISoundPlayer soundPlayer)
        {
            _soundPlayer = soundPlayer;
        }


        #endregion




        #region Rx

        public Subject<StatusPlaying> QueueChangeRx { get; } = new Subject<StatusPlaying>();
        //Событие определния начала/конца проигрывания ОЧЕРЕДИ
        public Subject<StatusPlaying> SoundMessageChangeRx { get; } = new Subject<StatusPlaying>();
        //Событие определния начала/конца проигрывания ФАЙЛА
        public Subject<TemplateChangeValue> TemplateChangeRx { get; } = new Subject<TemplateChangeValue>();
        //Событие определния начала/конца проигрывания динамического ШАБЛОНА
        public Subject<StaticChangeValue> StaticChangeRx { get; } = new Subject<StaticChangeValue>();
        //Событие определния начала/конца проигрывания  статического ФАЙЛА

        #endregion




        #region Methode

        public void StartQueue()
        {
            IsWorking = true;
        }


        public void StopQueue()
        {
            IsWorking = false;
        }


        public void StartAndPlayedCurrentMessage()
        {
            IsPlayedCurrentMessage = false;
            StartQueue();
        }


        public void StopAndPlayedCurrentMessage()
        {
            IsPlayedCurrentMessage = true;
        }
    




        /// <summary>
        /// Добавить элемент в очередь
        /// </summary>
        public void AddItem(ВоспроизводимоеСообщение item)
        {
            if (item == null)
                return;

            //делать сортировку по приоритету.
            if (item.ПриоритетГлавный == Priority.Low)
            {
                Queue.Enqueue(item);
            }
            else
            {
                if (!Queue.Any())
                {
                    Queue.Enqueue(item);
                    return;
                }

                //сохранили 1-ый элемент, и удаили его
                var currentFirstItem = Queue.FirstOrDefault();
                Queue.Dequeue();

                //добавили новый элемент и отсортировали.
                Queue.Enqueue(item);
                var ordered = Queue.OrderByDescending(elem => elem.ПриоритетГлавный).ThenByDescending(elem=>elem.ПриоритетВторостепенный).ToList();  //ThenByDescending(s=>s.) упорядочевать дополнительно по времени добавления

                //Очистили и заполнили заново очередь
                Queue.Clear();
                if (currentFirstItem != null)
                {
                    Queue.Enqueue(currentFirstItem);
                }
                foreach (var elem in ordered)
                {
                    Queue.Enqueue(elem);
                }
            }
        }



        /// <summary>
        /// Очистить очередь
        /// </summary>
        public void Clear()
        {
            Queue?.Clear();
            ElementsOnTemplatePlaying?.Clear();
            CurrentTemplatePlaying = null;
            CurrentSoundMessagePlaying = null;
        }



        public ВоспроизводимоеСообщение FindItem(int rootId, int? parentId)
        {
            if (GetElementsWithFirstElem == null || !GetElementsWithFirstElem.Any())
                return null;

            return GetElementsWithFirstElem.FirstOrDefault(elem => elem.RootId == rootId && elem.ParentId == parentId);
        }



        public void PausePlayer()
        {
            StopQueue();
            _soundPlayer.Pause();
        }



        public void PlayPlayer()
        {
            StartQueue();
            _soundPlayer.Play();
        }



        public void Erase()
        {
           Clear();
            _soundPlayer.PlayFile(null);
        }



        /// <summary>
        /// Разматывание очереди, внешним кодом
        /// </summary>
        private bool _isAnyOldQueue;
        private bool _isEmptyRaiseQueue;
        public void Invoke()
        {
            if(!IsWorking)
                return;

            try
            {
                SoundPlayerStatus status = _soundPlayer.GetPlayerStatus();

                //Определение событий Начала проигрывания очереди и конца проигрывания очереди.-----------------
                if (Queue.Any() && !_isAnyOldQueue && CurrentSoundMessagePlaying == null)
                {
                    EventStartPlayingQueue();
                }

                if (!Queue.Any() && _isAnyOldQueue)
                {
                    _isEmptyRaiseQueue = true;
                }

                if ((CurrentSoundMessagePlaying != null) && (status != SoundPlayerStatus.Playing))
                {
                    EventEndPlayingSoundMessage(CurrentSoundMessagePlaying);
                }

                if (!Queue.Any() && _isEmptyRaiseQueue && (status != SoundPlayerStatus.Playing)) // ожидание проигрывания последнего файла.
                {
                    _isEmptyRaiseQueue = false;
                    CurrentSoundMessagePlaying = null;
                    EventEndPlayingQueue();
                }

                _isAnyOldQueue = Queue.Any();



                //Разматывание очереди. Определение проигрываемого файла-----------------------------------------------------------------------------
                if (status != SoundPlayerStatus.Playing)
                {
                    if (_pauseTime > 0)
                    {
                        _pauseTime--;
                        return;
                    }

                    if (Queue.Any())
                    {
                        var peekItem = Queue.Peek();
                        if (peekItem.ОчередьШаблона == null)               //Статическое сообщение
                        {
                            CurrentSoundMessagePlaying = Queue.Dequeue();
                            ElementsOnTemplatePlaying = null;
                        }
                        else                                              //Динамическое сообщение
                        {
                            if (peekItem.ОчередьШаблона.Any())
                            {
                                ElementsOnTemplatePlaying = peekItem.ОчередьШаблона.ToList();
                                var item = peekItem.ОчередьШаблона.Dequeue();

                                if ((CurrentSoundMessagePlaying == null) ||
                                    (CurrentSoundMessagePlaying.ParentId != item.ParentId &&
                                     CurrentSoundMessagePlaying.RootId != item.RootId))
                                {
                                    EventStartPlayingTemplate(peekItem);  //item
                                }
                                CurrentSoundMessagePlaying = item;
                            }
                            else
                            {
                                Queue.Dequeue();
                                EventEndPlayingTemplate(peekItem);
                                CurrentSoundMessagePlaying = null;
                                ElementsOnTemplatePlaying = null;
                            }
                        }


                        //Проигрывание фалйла-----------------------------------------------------------------------------------------------------------
                        if (CurrentSoundMessagePlaying == null)
                            return;

                        if (CurrentSoundMessagePlaying.ВремяПаузы.HasValue)                         //воспроизводимое сообщение - это ПАУЗА
                        {
                            _pauseTime = CurrentSoundMessagePlaying.ВремяПаузы.Value;
                            return;
                        }

                        if (_soundPlayer.PlayFile(CurrentSoundMessagePlaying))
                            EventStartPlayingSoundMessage(CurrentSoundMessagePlaying);
                    }
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Invoke = {ex.ToString()}");//DEBUG
                Log.log.Fatal($"Exception внутри очереди: {ex}");//DEBUG
            }

        }


        /// <summary>
        /// Событие НАЧАЛА проигрывания очереди.
        /// До этого момента очередь была пуста.
        /// </summary>
        private void EventStartPlayingQueue()
        {
            var firstElem= Queue.Peek();
            LastSoundMessagePlayed = (firstElem.ОчередьШаблона == null || !firstElem.ОчередьШаблона.Any()) ? firstElem : firstElem.ОчередьШаблона.FirstOrDefault(); //DEBUG
            //Debug.WriteLine($"EventStartPlayingQueue: {LastSoundMessagePlayed.ИмяВоспроизводимогоФайла}");//DEBUG  //ТолькоПоВнутреннемуКаналу={LastSoundMessagePlayed.НастройкиВыводаЗвука.ТолькоПоВнутреннемуКаналу}


            //Debug.WriteLine("НАЧАЛО ПРОИГРЫВАНИЯ ОЧЕРЕДИ *********************");//DEBUG
            QueueChangeRx.OnNext(StatusPlaying.Start);
        }



        /// <summary>
        /// Событие КОНЦА проигрывания очереди.
        /// До этого момента из очереди проигрывался последний файл.
        /// </summary>
        private void EventEndPlayingQueue()
        {
            //Debug.WriteLine($"EventEndPlayingQueue: {LastSoundMessagePlayed.ИмяВоспроизводимогоФайла}");//DEBUG   ТолькоПоВнутреннемуКаналу={LastSoundMessagePlayed.НастройкиВыводаЗвука.ТолькоПоВнутреннемуКаналу}

            //Debug.WriteLine("КОНЕЦ ПРОИГРЫВАНИЯ ОЧЕРЕДИ *********************");//DEBUG
            QueueChangeRx.OnNext(StatusPlaying.Stop);
        }



        /// <summary>
        /// Событие НАЧАЛА проигрывания элемента из очереди.
        /// </summary>
        private void EventStartPlayingSoundMessage(ВоспроизводимоеСообщение soundMessage)
        {
            SoundMessageChangeRx.OnNext(StatusPlaying.Start);

            if(IsStaticSoundPlaying)
                EventStartPlayingStatic(soundMessage);

            //Log.log.Fatal($"начало проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
            // Debug.WriteLine($"начало проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }



        /// <summary>
        /// Событие КОНЦА проигрывания элемента из очереди.
        /// </summary>
        private void EventEndPlayingSoundMessage(ВоспроизводимоеСообщение soundMessage)
        {
            LastSoundMessagePlayed = soundMessage;
            SoundMessageChangeRx.OnNext(StatusPlaying.Stop);

            if (IsStaticSoundPlaying)
                EventEndPlayingStatic(soundMessage);

            //Log.log.Fatal($"конец проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
            //Debug.WriteLine($"конец проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }



        /// <summary>
        /// Событие НАЧАЛА проигрывания шаблона.
        /// </summary>
        private void EventStartPlayingTemplate(ВоспроизводимоеСообщение soundMessage)
        {
            //шаблон АВАРИЯ
            if (soundMessage.ParentId.HasValue && soundMessage.ТипСообщения == ТипСообщения.ДинамическоеАварийное)
            {
                СостояниеФормируемогоСообщенияИШаблон шаблон = new СостояниеФормируемогоСообщенияИШаблон();
                шаблон.Id = soundMessage.ParentId.Value;
                шаблон.SoundRecordId = soundMessage.RootId;
                шаблон.НазваниеШаблона = soundMessage.ИмяВоспроизводимогоФайла;
                шаблон.ПриоритетГлавный = soundMessage.ПриоритетГлавный;

                CurrentTemplatePlaying = шаблон;
                TemplateChangeRx.OnNext(new TemplateChangeValue { StatusPlaying = StatusPlaying.Start, Template = шаблон, SoundMessage = soundMessage });
                //Debug.WriteLine($"-------------НАЧАЛО проигрывания ШАБЛОНА: НазваниеШаблона= {шаблон.НазваниеШаблона}-----------------");//DEBUG
                return;
            }

            //шаблон ДИНАМИКИ
            if (soundMessage.ParentId.HasValue && soundMessage.ТипСообщения == ТипСообщения.Динамическое)
            {
                var soundRecord = MainWindowForm.SoundRecords.FirstOrDefault(rec => rec.Value.ID == soundMessage.RootId).Value;
                if (soundRecord.СписокФормируемыхСообщений != null && soundRecord.СписокФормируемыхСообщений.Any())
                {
                    var template = soundRecord.СписокФормируемыхСообщений.FirstOrDefault(sm => sm.Id == soundMessage.ParentId.Value);
                    if (!string.IsNullOrEmpty(template.НазваниеШаблона))
                    {
                        CurrentTemplatePlaying = template;
                        TemplateChangeRx.OnNext(new TemplateChangeValue { StatusPlaying = StatusPlaying.Start, Template = template, SoundMessage = soundMessage});
                        //Debug.WriteLine($"-------------НАЧАЛО проигрывания ШАБЛОНА: НазваниеШаблона= {template.НазваниеШаблона}-----------------");//DEBUG
                    }
                }
                return;
            }

            //шаблон ДИНАМИКИ техническое сообщение
            if (soundMessage.ParentId.HasValue && soundMessage.ТипСообщения == ТипСообщения.ДинамическоеТехническое)
            {
                СостояниеФормируемогоСообщенияИШаблон шаблон = new СостояниеФормируемогоСообщенияИШаблон();
                шаблон.Id = soundMessage.ParentId.Value;
                шаблон.SoundRecordId = soundMessage.RootId;
                шаблон.НазваниеШаблона = soundMessage.ИмяВоспроизводимогоФайла;
                шаблон.ПриоритетГлавный = soundMessage.ПриоритетГлавный;

                CurrentTemplatePlaying = шаблон;
                TemplateChangeRx.OnNext(new TemplateChangeValue { StatusPlaying = StatusPlaying.Start, Template = шаблон, SoundMessage = soundMessage });
               // Debug.WriteLine($"-------------НАЧАЛО проигрывания ШАБЛОНА: НазваниеШаблона= {шаблон.НазваниеШаблона}-----------------");//DEBUG
                return;
            }
        }



        /// <summary>
        /// Событие КОНЦА проигрывания шаблона.
        /// </summary>
        private void EventEndPlayingTemplate(ВоспроизводимоеСообщение soundMessage)
        {
            if (IsPlayedCurrentMessage)
                Erase();               //Очистить очередь после проигрывания

            //шаблон АВАРИЯ (Id сообщения > 1000)
            if ((soundMessage.RootId > 0) && (soundMessage.ParentId.HasValue) && (soundMessage.ТипСообщения == ТипСообщения.ДинамическоеАварийное))
            {
                СостояниеФормируемогоСообщенияИШаблон шаблон = new СостояниеФормируемогоСообщенияИШаблон();
                шаблон.Id = soundMessage.ParentId.Value;
                шаблон.SoundRecordId = soundMessage.RootId;
                шаблон.НазваниеШаблона = soundMessage.ИмяВоспроизводимогоФайла;
                шаблон.ПриоритетГлавный = soundMessage.ПриоритетГлавный;

                CurrentTemplatePlaying = шаблон;
                TemplateChangeRx.OnNext(new TemplateChangeValue { StatusPlaying = StatusPlaying.Stop, Template = шаблон, SoundMessage = soundMessage});
                //Debug.WriteLine($"-------------НАЧАЛО проигрывания ШАБЛОНА: НазваниеШаблона= {шаблон.НазваниеШаблона}-----------------");//DEBUG
                CurrentTemplatePlaying = null;
                return;
            }

            //шаблон ДИНАМИКИ
            if (soundMessage.ParentId.HasValue && soundMessage.ТипСообщения == ТипСообщения.Динамическое)
            {
                var soundRecord = MainWindowForm.SoundRecords.FirstOrDefault(rec => rec.Value.ID == soundMessage.RootId).Value;
                if ((soundRecord.ID) > 0 && (soundMessage.ParentId.HasValue))
                {
                    var template = soundRecord.СписокФормируемыхСообщений.FirstOrDefault(sm => sm.Id == soundMessage.ParentId.Value);
                    if (!string.IsNullOrEmpty(template.НазваниеШаблона))
                    {
                        CurrentTemplatePlaying = null;
                        TemplateChangeRx.OnNext(new TemplateChangeValue { StatusPlaying = StatusPlaying.Stop, Template = template, SoundMessage = soundMessage });
                        //Debug.WriteLine($"--------------КОНЕЦ проигрывания ШАБЛОНА: НазваниеШаблона= {template.НазваниеШаблона}-----------------");//DEBUG
                    }
                }
                CurrentTemplatePlaying = null;
                return;
            }

            //шаблон ДИНАМИКИ техническое сообщение
            if (soundMessage.ParentId.HasValue && soundMessage.ТипСообщения == ТипСообщения.ДинамическоеТехническое)
            {
                СостояниеФормируемогоСообщенияИШаблон шаблон = new СостояниеФормируемогоСообщенияИШаблон();
                шаблон.Id = soundMessage.ParentId.Value;
                шаблон.SoundRecordId = soundMessage.RootId;
                шаблон.НазваниеШаблона = soundMessage.ИмяВоспроизводимогоФайла;
                шаблон.ПриоритетГлавный = soundMessage.ПриоритетГлавный;

                CurrentTemplatePlaying = шаблон;
                TemplateChangeRx.OnNext(new TemplateChangeValue { StatusPlaying = StatusPlaying.Stop, Template = шаблон, SoundMessage = soundMessage });
                //Debug.WriteLine($"-------------КОНЕЦ проигрывания ШАБЛОНА: НазваниеШаблона= {шаблон.НазваниеШаблона}-----------------");//DEBUG
                CurrentTemplatePlaying = null;
                return;
            }   
        }



        /// <summary>
        /// Событие НАЧАЛА проигрывания статического файла.
        /// </summary>
        private void EventStartPlayingStatic(ВоспроизводимоеСообщение soundMessage)
        {
             StaticChangeRx.OnNext(new StaticChangeValue {StatusPlaying = StatusPlaying.Start, SoundMessage = soundMessage});
             //Debug.WriteLine($"^^^^^^^^^^^СТАТИКА НАЧАЛО {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }



        /// <summary>
        /// Событие КОНЦА проигрывания статического файла.
        /// </summary>
        private void EventEndPlayingStatic(ВоспроизводимоеСообщение soundMessage)
        {
            if (IsPlayedCurrentMessage)
                Erase();               //Очистить очередь после проигрывания

            StaticChangeRx.OnNext(new StaticChangeValue { StatusPlaying = StatusPlaying.Stop, SoundMessage = soundMessage });
            //Debug.WriteLine($"^^^^^^^^^^^СТАТИКА КОНЕЦ: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }

        #endregion




        #region IDisposable

        public void Dispose()
        {
            if (!QueueChangeRx.IsDisposed)
                QueueChangeRx.Dispose();

            if (!SoundMessageChangeRx.IsDisposed)
                SoundMessageChangeRx.Dispose();

            if (!TemplateChangeRx.IsDisposed)
                TemplateChangeRx.Dispose();
        }

        #endregion
    }
}