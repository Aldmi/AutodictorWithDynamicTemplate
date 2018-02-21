using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutodictorBL.Sound.Converters;

namespace AutodictorBL.Sound.Services
{
    public class CopyWavFileService
    {
        private readonly IFileNameConverter _fileNameConverter;

        public CopyWavFileService(IFileNameConverter fileNameConverter)
        {
            _fileNameConverter = fileNameConverter;
        }




        public async Task CopyFile(string pathSource, string pathDest, Action<int> progressCallback)
        {
            var dict = new Dictionary<string, string>();
            DirSearch(pathSource, dict);
            if (dict.Any())
            {
                try
                {
                    for (var x = 0; x < dict.Count; x++)
                    {
                        var item = dict.ElementAt(x);
                        var from = item.Key;
                        var to = Path.Combine(pathDest, item.Value);

                        using (var outStream = new FileStream(to, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            using (var inStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                await inStream.CopyToAsync(outStream);
                            }
                        }

                        double prop = (x + 1.0) / dict.Count;
                        progressCallback((int)(prop * 100));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Исключение ПРИ КОПИРОВАНИИ ФАЙЛОВ: \"{ex.Message}\"");
                }

                await SaveDictionary2File(pathDest, dict);
            }
        }



        /// <summary>
        /// Рекурсивный поиск файлов в директории.
        /// </summary>
        private void DirSearch(string sDir, Dictionary<string, string> dict)
        {
            try
            {
                foreach (var f in Directory.GetFiles(sDir, "*.wav"))
                {
                    var fileName = Path.GetFileNameWithoutExtension(f);
                    var newFilename = _fileNameConverter.Convert(fileName) + @".wav";
                    dict.Add(f, newFilename);
                }

                foreach (var d in Directory.GetDirectories(sDir))
                {
                    DirSearch(d, dict);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Исключение ПРИ ПОИСКЕ ФАЙЛОВ: \"{ex.Message}\"");
            }
        }


        /// <summary>
        /// Сохранение списка файлов на диск
        /// </summary>
        private async Task SaveDictionary2File(string pathDest, Dictionary<string, string> dict)
        {
            try
            {
                var filePath = Path.Combine(pathDest, "List.txt");
                using (StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate)))
                {
                    foreach (var d in dict)
                    {
                        await sw.WriteLineAsync($"{d.Key} --->  {d.Value}");
                    }
                    await sw.WriteLineAsync($"ВСЕГО: {dict.Count} файлов");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Исключение сохранения списка в файл: \"{ex.Message}\"");
            }
        }

    }
}