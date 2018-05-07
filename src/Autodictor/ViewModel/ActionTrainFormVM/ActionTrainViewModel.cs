﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DAL.Abstract.Entitys;

namespace MainExample.ViewModel.ActionTrainFormVM
{
    public enum ActionTypeViewModel
    {
      [Display(Name = "НЕТ")]
      None,

      [Display(Name = "ПРИБ.")]
      Arrival,

      [Display(Name = "ОТПР.")]
      Departure
    }


    public class ActionTrainViewModel
    {
        #region prop

        [Display(AutoGenerateField = false)]      //Не отображать в таблице
        public int IdTrainType { get; set; }      //IdTrainType

        [Display(AutoGenerateField = false)]      //Не отображать в таблице
        public bool IsActiveBase { get; set; }    //Базовая активность шаблона

        [Display(Name = "Id")]
        [Editable(false)]
        public int Id { get; set; }              //Id

        [Display(Name = "Название")]
        [Editable(false)]
        public string Name { get; set; }         //Имя

        [Display(Name = "дельта T (мин.)")]
        [Required(ErrorMessage = "Требуется поле дельта T")]
        public string ActionTimeDelta { get; set; }      //Дельта времени  (строковое представление ActionTime)

        [Display(Name = "Тип поезда")]
        [Required(ErrorMessage = "Требуется поле Тип поезда")]
        public ActionTypeViewModel ActionTypeViewModel { get; set; }         //тип действия относительно которого отсчитывается ActionTimeDelta

        [Display(Name = "Приоритет")]
        [Required(ErrorMessage = "Требуется поле Приоритет")]
        public int Priority { get; set; }        //Приоритет

        [Display(AutoGenerateField = false)]    //Не отображать в таблице
        public bool Transit { get; set; }

        [DisplayName("Шаблоны")]
        public List<LangViewModel> Langs { get; set; }      //Шаблоны на разных языках

        [Display(AutoGenerateField = false)]      //Не отображать в таблице
        public Emergency Emergency { get; set; }

        #endregion
    }


    public class LangViewModel
    {
        #region prop

        [Display(Name = "Id")]
        [Editable(false)]
        public int Id { get; set; }             //Id языка

        [Display(Name = "Язык")]
        [Editable(false)]
        public string Name { get; set; }       //Название языка

        [Display(Name = "Разрешение")]
        public bool IsEnable { get; set; }     // Вкл/Выкл язык

        [Display(Name = "Повторы ТЕЛА")]
        public int RepeatSoundBody { get; set; }     // кол-во повторов для Тела

        [DisplayName("НАЧАЛО")]
        public List<string> TemplateSoundStart { get; set; }

        [DisplayName("ТЕЛО")]       
        public List<string> TemplateSoundBody { get; set; }

        [DisplayName("КОНЕЦ")]
        public List<string> TemplateSoundEnd { get; set; }

        #endregion
    }
}