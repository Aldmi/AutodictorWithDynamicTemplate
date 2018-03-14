﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainExample.ViewModel
{
    public enum ActionTypeViewModel
    {
      [Display(Name = "Id")]
      None,

      [Display(Name = "ПРИБ.")]
      Arrival,

      [Display(Name = "ОТПР.")]
      Departure
    }


    public class ActionTrainViewModel
    {
        #region prop

        [Display(Name = "Id")]
        [Editable(false)]
        public int Id { get; set; }              //Id

        [Display(Name = "Название")]
        [Editable(false)]
        public string Name { get; set; }         //Имя

        [Display(Name = "Время циклического повтора")]
        public int? ActionTimeCycle { get; set; }      //Время циклическго повтора

        [Display(Name = "Временная дельта")]
        [Required(ErrorMessage = "Требуется поле Временная дельта")]
        public int? ActionTimeDelta { get; set; }      //Дельта времени

        [Display(Name = "Тип поезда")]
        [Required(ErrorMessage = "Требуется поле Тип поезда")]
        public ActionTypeViewModel ActionTypeViewModel { get; set; }         //тип действия относительно которого отсчитывается ActionTimeDelta


        [Display(Name = "Приоритет")]
        [Required(ErrorMessage = "Требуется поле Приоритет")]
        public int Priority { get; set; }        //Приоритет

        [Display(Name = "Кол-во повторов")]
        [Required(ErrorMessage = "Требуется поле Кол-во повторов")]
        public int Repeat { get; set; }         //Кол-во повторов

        [Display(Name = "Параметры оповещения")]
        public List<LangViewModel> Langs { get; set; }      //Шаблоны на разных языках

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

        #endregion
    }
}