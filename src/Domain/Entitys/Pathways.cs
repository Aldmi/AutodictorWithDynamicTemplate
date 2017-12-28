namespace Domain.Entitys
{
    public class Pathways : EntityBase
    {
        public string Name { get; set; }

        public string НаНомерПуть { get; set; }         //для переменной шаблона "НА НОМЕР ПУТЬ"
        public string НаНомерОмПути { get; set; }       //для переменной шаблона НА НОМЕРом ПУТИ
        public string СНомерОгоПути { get; set; }      //для переменной шаблона "С НОМЕРого ПУТИ"

        public string Addition { get; set; }           //Дополнение
        public string Addition2 { get; set; }          //Дополнение2
    }

}