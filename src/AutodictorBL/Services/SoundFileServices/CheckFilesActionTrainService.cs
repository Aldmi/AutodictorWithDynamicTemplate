using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Abstract.Concrete;

namespace AutodictorBL.Services.SoundFileServices
{
    public class CheckFilesActionTrainService
    {
        #region field

        private readonly ITrainTypeByRyleRepository _trainTypeByRyleRepository;

        #endregion




        #region prop

        public List<FileInfo> SoundsFileInfoList { get; set; }
        public List<FileInfo> NumbersInfoList { get; set; }
        public List<FileInfo> StaticMessagesFileInfoList { get; set; }
        public List<FileInfo> DynamicMessagesFileInfoList { get; set; }

        #endregion





        #region ctor

        public CheckFilesActionTrainService(ITrainTypeByRyleRepository trainTypeByRyleRepository)
        {
            _trainTypeByRyleRepository = trainTypeByRyleRepository;
        }

        #endregion





        #region Methode

        public void LoadSounds(string path = null)
        {
            path = path ?? AppDomain.CurrentDomain.BaseDirectory + @"\Wav\Sounds\";
            SoundsFileInfoList = LoadFileInfoList(path);
        }

        public void LoadNumbers(string path = null)
        {
            path = path ?? AppDomain.CurrentDomain.BaseDirectory + @"\Wav\Numbers\";
            NumbersInfoList = LoadFileInfoList(path);
        }

        public void LoadStaticMessages(string path = null)
        {
            path = path ?? AppDomain.CurrentDomain.BaseDirectory + @"\Wav\Static message\";
            StaticMessagesFileInfoList = LoadFileInfoList(path);
        }

        public void LoadDynamicMessages(string path = null)
        {
            path = path ?? AppDomain.CurrentDomain.BaseDirectory + @"\Wav\Dynamic message\";
            DynamicMessagesFileInfoList = LoadFileInfoList(path);
        }


        public async Task<IEnumerable<SoundFileError>> CheckExistsActionTrainFiles()
        {
            var errorsList= new List<SoundFileError>();
            var res = await Task.Run( () =>
            {
                var rules= _trainTypeByRyleRepository.List();
                foreach (var rule in rules)
                {
                    var actionTrains = rule.ActionTrains;
                    foreach (var act in actionTrains)
                    {
                        foreach (var lang in act.Langs)
                        {
                            var listSounds= new List<string>();
                            if (lang.TemplateSoundStart != null)
                            {
                                listSounds.AddRange(lang.TemplateSoundStart);
                            }
                            if (lang.TemplateSoundBody != null)
                            {
                                listSounds.AddRange(lang.TemplateSoundBody);
                            }
                            if (lang.TemplateSoundEnd != null)
                            {
                                listSounds.AddRange(lang.TemplateSoundEnd);
                            }
                            foreach (var soundName in listSounds)
                            {
                                if (soundName.Length > 0 && !char.IsUpper(soundName[0]))
                                {
                                    if (!CheckDynamicExists(soundName))
                                    {
                                        errorsList.Add(new SoundFileError
                                        {
                                            Type = "Динимический",
                                            Name = rule.NameRu,
                                            NameAction = act.Name,
                                            NameLang = lang.Name,
                                            SoundName = soundName
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                return errorsList;
            });

            return res;
        }



        private bool CheckDynamicExists(string fileName)
        {
            if (SoundsFileInfoList.FirstOrDefault(file=> Path.GetFileNameWithoutExtension(file.FullName) == fileName) != null)
                return true;

            if (NumbersInfoList.FirstOrDefault(file=> Path.GetFileNameWithoutExtension(file.FullName) == fileName) != null)
                return true;

            if (DynamicMessagesFileInfoList.FirstOrDefault(file=> Path.GetFileNameWithoutExtension(file.FullName) == fileName) != null)
                return true;

            return false;
        } 



        private List<FileInfo> LoadFileInfoList(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Директория не найденна {path}");
            }

            return new DirectoryInfo(path)
                .GetFiles("*.wav", SearchOption.AllDirectories)
                .ToList();
        }

        #endregion
    }


    public class SoundFileError
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string NameAction { get; set; }
        public string NameLang { get; set; }
        public string SoundName { get; set; }
    }
}