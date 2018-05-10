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
        /// <summary>
        /// Возвращает список дианмических шаблонов на базе ActionTrain.
        /// Учитывается Время активации шаблона.
        /// </summary>
        List<ActionTrainDynamic> СreateActionTrainDynamic(SoundRecord record, IEnumerable<ActionTrain> actions, double lowDelta4Cycle = -60, double hightDelta4Cycle = 60);

        List<TemplateItem> CalcTemplateItems(ActionTrain actionTrain, List<string> allowedLang);
        List<TemplateItem> CalcTemplateItemsByLang(Lang lang);
    }
}