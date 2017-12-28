using System.IO;
using System.Xml.Linq;

namespace Library.Xml
{
    public static class XmlWorker
    {
        public static XElement LoadXmlFile(string folderName, string fileName)
        {
            var path = string.IsNullOrEmpty(folderName) ?
                       Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}") :
                       Path.Combine(Directory.GetCurrentDirectory(), $"{folderName}\\{fileName}");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"XML файл не НАЙДЕНН!!!   \"{path} \"");
            }

            return XElement.Load(path);
        }
    }
}