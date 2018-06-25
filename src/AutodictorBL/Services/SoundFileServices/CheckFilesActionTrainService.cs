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


        public async Task<IEnumerable<string>> CheckExistsActionTrainFiles()
        {
            var res = await Task.Run(async () =>
            {
                  await Task.Delay(5000);
                  return new List<string> { "sdfsds", "hytuyt" };
              });

            return res;
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
}