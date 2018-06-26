using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using AutodictorBL;
using AutodictorBL.Services.SoundFileServices;
using Autofac;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;
using Library.Logs;
using MainExample.Utils;



namespace MainExample
{

    static class Program
    {
        public static List<string> FilesFolder = null;
        public static List<string> NumbersFolder = null;
        public static List<string> СписокСтатическихСообщений = null;
        public static List<string> СписокДинамическихСообщений = null;
        public static List<string> НомераПоездов = new List<string>();
        public static string ИнфСтрокаНаТабло = "";
        public static Настройки Настройки;
        public static string[] ТипыОповещения = new string[] { "Не определено", "На Х-ый путь", "На Х-ом пути", "С Х-ого пути" };
        public static string[] ТипыВремени = new string[] { "Прибытие", "Отправление" };
        public static List<string> ШаблоныОповещения = new List<string>();
        public static DateTime StartTime { get; } = DateTime.Now;
        public static AutodictorModel AutodictorModel { get; set; }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AutofacConfig.ConfigureContainer();
            Mappers.Mapper.SetContainer(AutofacConfig.Container);//Передача контейнера в статический класс

            ОкноНастроек.ЗагрузитьНастройки();

            try
            {
                var dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Sounds\");
                FilesFolder = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    FilesFolder.Add(Path.GetFileNameWithoutExtension(file.FullName));

                dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Numbers\");
                NumbersFolder = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    NumbersFolder.Add(Path.GetFileNameWithoutExtension(file.FullName));

                dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Static message\");
                СписокСтатическихСообщений = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    СписокСтатическихСообщений.Add(Path.GetFileNameWithoutExtension(file.FullName));

                dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Dynamic message\");
                СписокДинамическихСообщений = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    СписокДинамическихСообщений.Add(Path.GetFileNameWithoutExtension(file.FullName));

                //DEBUG-------------------------------------------------
                var checkFilesActionTrainService = AutofacConfig.Container.Resolve<CheckFilesActionTrainService>();
                checkFilesActionTrainService.LoadStaticMessages();
                checkFilesActionTrainService.LoadDynamicMessages();
                checkFilesActionTrainService.LoadNumbers();
                checkFilesActionTrainService.LoadSounds();
                //DEBUG-------------------------------------------------
            }
            catch (Exception ex)
            {
                
            };

            AutodictorModel = new AutodictorModel();
            AutodictorModel.LoadSetting(Настройки.ВыборУровняГромкости, GetFileName);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ОБРАБОТКА НЕ ПЕРЕХВАЧЕННЫХ ИСКЛЮЧЕНИЙ
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var mainForm = AutofacConfig.Container.Resolve<MainForm>();
            Application.Run(mainForm);

            Dispose();
        }


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.log.Fatal($"Исключение из не UI потока {e.Exception.Message}");
        }



        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.log.Fatal($"Исключение основного UI потока {(e.ExceptionObject as Exception)?.Message}");
        }









        public static string GetFileName(string track, NotificationLanguage lang = NotificationLanguage.Rus)
        {
            string langPostfix = String.Empty;
            switch (lang)
            {
                case NotificationLanguage.Eng:
                    langPostfix = "_" + NotificationLanguage.Eng;
                    break;
            }
            track += langPostfix;
            string Path = Application.StartupPath + @"\";


            if (FilesFolder != null && FilesFolder.Contains(track))
                return Path + @"Wav\Sounds\" + track + ".wav";

            if (NumbersFolder != null && NumbersFolder.Contains(track))
                return Path + @"Wav\Numbers\" + track + ".wav";

            if (СписокСтатическихСообщений != null && СписокСтатическихСообщений.Contains(track))
                return Path + @"Wav\Static message\" + track + ".wav";

            if (СписокДинамическихСообщений != null && СписокДинамическихСообщений.Contains(track))
                return Path + @"Wav\Dynamic message\" + track + ".wav";

            foreach (var sound in StaticSoundForm.StaticSoundRecords)
                if (sound.Name == track)
                    return sound.Path;

            return "";
        }






        public static void ЗаписьЛога(string ТипСообщения, string Сообщение, User user)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Open("Logs\\" + DateTime.Now.ToString("yy.MM.dd") + ".log", FileMode.Append)))
                {
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": \t" + user.Login + ": \t" + ТипСообщения + "\t" + Сообщение);
                    sw.Close();
                }
            }
            catch (Exception ex) { };
        }






        #region Dispouse

        private static void Dispose()
        {
            AutofacConfig.Container.Dispose();
            AutodictorModel?.Dispose();
        }

        #endregion
    }
}
