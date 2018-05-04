using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace Library.Xml
{
    public static class ExtendedXmlSerializerWorker
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

            var serializer = new ConfigurationContainer()
                .UseOptimizedNamespaces() //If you want to have all namespaces in root element
                .Create();

            var xml = serializer.Serialize(new XmlWriterSettings { Indent = true }, obj);
            File.WriteAllText(path, xml);

            //XmlSerializer formatter = new XmlSerializer(typeof(T));
            //using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            //{
            //    formatter.Serialize(fs, obj);
            //}
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
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (T)formatter.Deserialize(fs);
            }
        }
    }
}