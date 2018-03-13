using System.Collections.Generic;
using System.Linq;

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

        public int Id { get; }                    //Id действия
        public string Name { get; set; }
        public ActionType ActionType { get; set; }
        public int Priority { get; set; }
        public int Repeat { get; set; }
        public bool Transit { get; set; }
        public Emergency Emergency { get; set; }
        public ActionTime Time { get; set; }
        public List<Lang> Langs { get; set; }      //Шаблоны на разных языках

        #endregion




        #region ctor

        public ActionTrain(string id, string name, string actionType, string priority, string repeat, string transit, string emergency, string times, List<Lang> langs)
        {
            Id = int.Parse(id);
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
            Repeat = int.Parse(repeat);
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
    /// </summary>
    public class ActionTime
    {
        #region

        public int? CycleTime { get; }     // Если стоит CycleTime, то DeltaTime игнорируется
        public int? DeltaTime { get; }     // Если стоит DeltaTime, то CycleTime игнорируется

        #endregion




        #region ctor

        public ActionTime(string time)
        {
            if (string.IsNullOrEmpty(time))
                return;

            if (time.StartsWith("~"))
            {
                DeltaTime = null;
                CycleTime = int.Parse(time.Remove(0, 1));
            }
            else
            {
                CycleTime = null;
                DeltaTime = int.Parse(time);
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

        public int Id { get; }              //Id языка
        public string Name { get; set; }
        public bool IsEnable { get; set; }  // Вкл/Выкл язык
        public List<string> TemplateSoundStart { get; }
        public List<string> TemplateSoundBody { get; }
        public List<string> TemplateSoundEnd { get; }

        #endregion




        #region ctor

        public Lang(string id, string name, string templateSoundStart, string templateSoundBody, string templateSoundEnd)
        {
            Id = int.Parse(id);
            Name = name;
            TemplateSoundStart = string.IsNullOrEmpty(templateSoundStart) ? null : templateSoundStart.Split('|').ToList();
            TemplateSoundBody = string.IsNullOrEmpty(templateSoundBody) ? null : templateSoundBody.Split('|').ToList();
            TemplateSoundEnd = string.IsNullOrEmpty(templateSoundEnd) ? null : templateSoundEnd.Split('|').ToList();
        }

        #endregion
    }
}