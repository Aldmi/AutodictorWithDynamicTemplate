using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace DAL.Abstract.Entitys
{
    public enum CategoryTrain { None, Suburb, LongDist }
    public enum ActionType { None, Arrival, Departure }
    public enum Emergency { None, DelayedArrival, DelayedDeparture, Cancel, DispatchOnReadiness }


    public interface IRuleByTrainType
    {

    }


    public class TrainTypeByRyle : IRuleByTrainType
    {
        #region prop

        public int Id { get; }              //Id типа.
        public CategoryTrain CategoryTrain { get; } //Принадлежность к типу (дальний/пригород)
        public string NameRu { get; }       //Имя и его Alias
        public string AliasRu { get; }
        public string NameEng { get; }
        public string AliasEng { get; }
        public string NameCh { get; }
        public string AliasCh { get; }

        public string ShowPathTimer { get; }//???
        public int WarningTimer { get; }  //окрашивать в главном окне в жёлтый за X минут до первого события.

        public List<ActionTrain> ActionTrains { get; }

        #endregion




        #region ctor

        public TrainTypeByRyle(string id, string typeTrain, string nameRu, string aliasRu, string nameEng, string aliasEng, string nameCh, string aliasCh, string showPathTimer, string warningTimer, List<ActionTrain> actionTrains)
        {
            int intVal;
            Id = int.Parse(id);
            switch (typeTrain)
            {
                case "Дальний":
                    CategoryTrain = CategoryTrain.LongDist;
                    break;

                case "Пригород":
                    CategoryTrain = CategoryTrain.Suburb;
                    break;

                default:
                    CategoryTrain = CategoryTrain.None;
                    break;
            }
            NameRu = nameRu;
            AliasRu = aliasRu;
            NameEng = nameEng;
            AliasEng = aliasEng;
            NameCh = nameCh;
            AliasCh = aliasCh;
            ShowPathTimer = showPathTimer;
            WarningTimer = int.TryParse(warningTimer, out intVal) ? intVal : 0;
            ActionTrains = actionTrains;
        }

        #endregion
    }



    /// <summary>
    /// действие (шаблон)
    /// </summary>
    public class ActionTrain
    {
        #region prop

        public int Id { get; set; }                    //Id действия
        public string Name { get; set; }
        public bool IsActiveBase { get; set; }        //Базовая активность шаблона, задается в XML файле    
        public ActionType ActionType { get; set; }
        public int Priority { get; set; }
        public bool Transit { get; set; }
        public Emergency Emergency { get; set; }
        public ActionTime Time { get; set; }
        public List<Lang> Langs { get; set; }         //Шаблоны на разных языках

        #endregion




        #region ctor

        public ActionTrain()
        {            
        }

        public ActionTrain(string id, string enable, string name, string actionType, string priority, string transit, string emergency, string times, List<Lang> langs)
        {
            Id = int.Parse(id);
            IsActiveBase = bool.Parse(enable);
            Name = name;

            switch (actionType)
            {
                case "ПРИБ":
                    ActionType = ActionType.Arrival;
                    break;

                case "ОТПР":
                    ActionType = ActionType.Departure;
                    break;

                default:
                    ActionType = ActionType.None;
                    break;
            }

            Priority = int.Parse(priority);
        
            Transit = bool.Parse(transit);

            switch (emergency)
            {
                case "Отмена":
                    Emergency = Emergency.Cancel;
                    break;

                case "ЗадПриб":
                    Emergency = Emergency.DelayedArrival;
                    break;

                case "ЗадОтпр":
                    Emergency = Emergency.DelayedDeparture;
                    break;

                case "ОтпрПоГотов":
                    Emergency = Emergency.DispatchOnReadiness;
                    break;

                default:
                    Emergency = Emergency.None;
                    break;
            }

            Time = new ActionTime(times);
            Langs = langs;
        }

        #endregion
    }


    /// <summary>
    /// Время воспроизведенния для шаблона.
    /// Если CycleTime == null, то указанн список временных дельт DeltaTimes.
    /// Если CycleTime != null, то указано циклическое время CycleTime.
    /// </summary>
    public class ActionTime
    {
        #region field

        private int? _cycleTime; //{ get; set; }
        private List<int> _deltaTimes; //{ get; set; }

        #endregion




        #region

        public bool IsDeltaTimes => (_deltaTimes != null && _cycleTime == null);


        public int? CycleTime
        {
            get { return _cycleTime; }
            set
            {
                _cycleTime = value;
                _deltaTimes = null;
            }
        }


        public List<int> DeltaTimes
        {
            get { return _deltaTimes; }
            set
            {
                _cycleTime = null;
                _deltaTimes = value;
            }
        }

        #endregion




        #region ctor

        public ActionTime()
        {
            
        }


        public ActionTime(string time)
        {
            if (string.IsNullOrEmpty(time))
                throw new ArgumentNullException(nameof(time));

            if (time.StartsWith("~"))
            {
                _deltaTimes = null;
                _cycleTime = int.Parse(time.Remove(0, 1));
            }
            else
            {
                _cycleTime = null;
                _deltaTimes = new List<int>();

                var deltaTimes= time.Split(',').ToList();
                foreach (var dt in deltaTimes)
                {
                    _deltaTimes.Add(int.Parse(dt));
                }
            }
        }

        #endregion
    }



    /// <summary>
    /// Язык и  шаблон для него
    /// </summary>
    public class Lang
    {
        #region prop

        public int Id { get; set; }              //Id языка
        public string Name { get; set; }
        public bool IsEnable { get; set; }  // Вкл/Выкл язык
        public int RepeatSoundBody { get; set; }   
        public List<string> TemplateSoundStart { get; set; }
        public List<string> TemplateSoundBody { get; set; }
        public List<string> TemplateSoundEnd { get; set; }

        #endregion




        #region ctor

        public Lang()
        { 
        }


        public Lang(string id, string name, string repeatSoundBody, string templateSoundStart, string templateSoundBody, string templateSoundEnd)
        {
            Id = int.Parse(id);
            Name = name;
            IsEnable = true;
            RepeatSoundBody = int.Parse(repeatSoundBody);
            TemplateSoundStart = string.IsNullOrEmpty(templateSoundStart) ? null : templateSoundStart.Split('|').ToList();
            TemplateSoundBody = string.IsNullOrEmpty(templateSoundBody) ? null : templateSoundBody.Split('|').ToList();
            TemplateSoundEnd = string.IsNullOrEmpty(templateSoundEnd) ? null : templateSoundEnd.Split('|').ToList();
        }

        #endregion
    }
}