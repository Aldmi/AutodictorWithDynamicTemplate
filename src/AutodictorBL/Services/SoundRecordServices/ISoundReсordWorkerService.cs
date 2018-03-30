using System;
using System.Collections.Generic;
using DAL.Abstract.Entitys;

namespace AutodictorBL.Services.SoundRecordServices
{
    public interface ISoundReсordWorkerService
    {
        /// <summary>
        /// Вычислить время сработки шаблона с учетом смещения.
        /// </summary>
        DateTime CalcTimeWithShift(ref SoundRecord rec, ActionTrainDynamic actionTrainDyn);
        /// <summary>
        /// Возвращает шаблон (actionTrain) в оболочке TextFragment, в которой слова выделены цветом.
        /// </summary>
        TextFragment CalcTextFragment(ref SoundRecord rec, ActionTrain actionTrain);
        List<TemplateItem> CalcTemplateItems(ActionTrain actionTrain, List<string> allowedLang);
        List<TemplateItem> CalcTemplateItemsByLang(Lang lang);
    }
}