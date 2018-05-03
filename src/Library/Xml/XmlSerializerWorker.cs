using System.IO;
using System.Xml.Serialization;

namespace Library.Xml
{
    public static class XmlSerializerWorker
    {
        /// <summary>
        /// Сериализация объекта в XML
        /// Сериализуемый объект должен иметь конструктор по умолчанию
        /// </summary>
        public static void Save<T>(T obj, string folderName, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"{folderName}");
            if (!Directory.Exists(path))
            {
                throw new FileNotFoundException($"папка для хранения XML файл не НАЙДЕННА!!!  \"{folderName} \"");
            }
            path = Path.Combine(Directory.GetCurrentDirectory(), $"{folderName}\\{fileName}");
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(fs, obj);
            }
        }


        /// <summary>
        /// Сериализация объекта в XML
        /// Сериализуемый объект должен иметь конструктор по умолчанию
        /// </summary>
        public static T Load<T>(string folderName, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"{folderName}\\{fileName}");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"файл XML файл не НАЙДЕННА!!!  \"{path} \"");
            }
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
               return (T)formatter.Deserialize(fs);
            }
        }
    }
}