using System.Windows.Forms;
using AutodictorBL.Services.SoundRecordServices;

namespace MainExample.UIHelpers
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Отображение фрагментов цветного текста на RichTextBox.
        /// </summary>
        /// <param name="rTb"></param>
        /// <param name="textFragments"></param>
        public static void ShowTextFragment(this RichTextBox rTb, TextFragment textFragments)
        {
            rTb.Text = string.Empty;
            rTb.AppendText(textFragments.StringBuilder.ToString());
            foreach (var option in textFragments.FragmentOptions)
            {
                rTb.SelectionStart = option.StartIndex;
                rTb.SelectionLength = option.Lenght;
                rTb.SelectionColor = option.Color;
            }
            rTb.SelectionLength = 0;
        }
    }
}