using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using AutodictorBL;
using AutodictorBL.DataAccess;
using AutodictorBL.Sound;
using Autofac;
using Autofac.Core;
using Communication.Annotations;
using DAL.Abstract.Abstract;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;
using DAL.NoSqlLiteDb.Repository;
using Library.Logs;
using Library.Xml;
using MainExample.Services;
using MainExample.Utils;


namespace MainExample
{

    static class Program
    {
        static Mutex m_mutex;

        public static List<string> FilesFolder = null;
        public static List<string> NumbersFolder = null;
        public static List<string> СписокСтатическихСообщений = null;
        public static List<string> СписокДинамическихСообщений = null;
        public static List<string> НомераПоездов = new List<string>();

        public static string ИнфСтрокаНаТабло = "";
        public static IDirectionRepository DirectionRepository; // Направления. хранилище XML
        public static IPathwaysRepository PathWaysRepository; //Пути. хранилище XML

        //TODO: IGenericDataRepository НЕ использовать напрямую
        public static IGenericDataRepository<SoundRecordChangesDb> SoundRecordChangesDbRepository; //Изменения в SoundRecord хранилище NoSqlDb
        public static IGenericDataRepository<User> UsersDbRepository; //Пользователи, хранилище NoSqlDb

        public static Настройки Настройки;


        public static string[] ТипыОповещения = new string[] { "Не определено", "На Х-ый путь", "На Х-ом пути", "С Х-ого пути" };
        public static string[] ТипыВремени = new string[] { "Прибытие", "Отправление" };

        public static List<string> ШаблоныОповещения = new List<string>();

        public static string[] ШаблонОповещенияОбОтменеПоезда = new string[] { "", "Отмена пассажирского поезда", "Отмена пригородного электропоезда", "Отмена фирменного поезда", "Отмена скорого поезда", "Отмена скоростного поезда", "Отмена ласточки", "Отмена РЭКСа" };
        public static string[] ШаблонОповещенияОЗадержкеПрибытияПоезда = new string[] { "", "Задержка прибытия пассажирского поезда", "Задержка прибытия пригородного электропоезда", "Задержка прибытия фирменного поезда", "Задержка прибытия скорого поезда", "Задержка прибытия скоростного поезда", "Задержка прибытия ласточки", "Задержка прибытия РЭКСа" };
        public static string[] ШаблонОповещенияОЗадержкеОтправленияПоезда = new string[] { "", "Задержка отправления пассажирского поезда", "Задержка отправления пригородного электропоезда", "Задержка отправления фирменного поезда", "Задержка отправления скорого поезда", "Задержка отправления скоростного поезда", "Задержка отправления ласточки", "Задержка отправления РЭКСа" };
        public static string[] ШаблонОповещенияООтправлениеПоГотовностиПоезда = new string[] { "", "Отправление по готовности пассажирского поезда", "Отправление по готовности пригородного электропоезда", "Отправление по готовности фирменного поезда", "Отправление по готовности скорого поезда", "Отправление по готовности скоростного поезда", "Отправление по готовности ласточки", "Отправление по готовности РЭКСа" };


        public static AuthenticationService AuthenticationService { get; set; } = new AuthenticationService();

        public static AutodictorModel AutodictorModel { get; set; }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (InstanceExists())
                return;

            AutofacConfig.ConfigureContainer();


            //DEBUG-------------
            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var repResolve = scope.Resolve<ITrainTypeByRyleRepository>();
                var acc = new TrainTypeByRyleService(repResolve);
                var listRules = acc.GetAll();
            }
            //DEBUG-------------


            ЗагрузкаНазванийПутей();
            ЗагрузкаНазванийНаправлений();
            ОкноНастроек.ЗагрузитьНастройки();

            string connection = @"NoSqlDb\Main.db";
            SoundRecordChangesDbRepository = new RepositoryNoSql<SoundRecordChangesDb>(connection);

            connection = @"NoSqlDb\Users.db";
            UsersDbRepository = new RepositoryNoSql<User>(connection);

            AuthenticationService.UsersDbInitialize();//не дожидаемся окончания Task-а загрузки БД


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
            }
            catch (Exception ex) { };


            AutodictorModel= new AutodictorModel();
            AutodictorModel.LoadSetting(Настройки.ВыборУровняГромкости, GetFileName);


            //DEBUG-------------
            //IRepository<TrainTypeByRyle> rep = new RepositoryXmlTrainTypeByRyle();
            //var acc= new TrainTypeByRyleService(rep);
            //var listRules = acc.GetAll();
            //DEBUG-------------


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ОБРАБОТКА НЕ ПЕРЕХВАЧЕННЫХ ИСКЛЮЧЕНИЙ
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new MainForm());

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



        //public static ISoundPlayer LoadSettings()
        //{
        //   // return new PlayerDirectX(Настройки.ВыборУровняГромкости, GetFileName);


        //    var player = new PlayerOmneo("192.168.1.44", 9407, "admin", "admin", "oll", 3000, 5000);
        //    var task = player.ReConnect();   //выполняется фоновая задача, пока не подключится к контроллеру усилителя.
        //    BackGroundTasks?.Add(task);
        //    return player;
        //}


        static bool InstanceExists()
        {
            bool createdNew;
            m_mutex = new Mutex(false, "AutodictorOneInstanceApplication", out createdNew);
            return (!createdNew);
        }



        public static string ByteArrayToHexString(byte[] data, int begin, int count)
        {
            int i;
            StringBuilder sb = new StringBuilder(count * 3);
            for (i = 0; i < count; i++)
                sb.Append(Convert.ToString(data[begin + i], 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }



        public static string GetFileName(string track, NotificationLanguage lang = NotificationLanguage.Ru)
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



        public static void ЗагрузкаНазванийНаправлений()
        {
            try
            {
                //DEBUG-------------
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var repResolve = scope.Resolve<IDirectionRepository>();
                    var dirServ = new DirectionService(repResolve);
                    var listDirections = dirServ.GetAll();
                }
                //DEBUG-------------



                //var xmlFile = XmlWorker.LoadXmlFile(string.Empty, "Stations.xml"); //все настройки в одном файле
                //if (xmlFile == null)
                //    return;

                //DirectionRepository = new RepositoryXmlDirection(xmlFile);                 //хранилище XML
                ////directionRep = new RepositoryEf<Direction>(dbContext);                   //хранилище БД
            }
            catch (Exception ex)
            {
                MessageBox.Show($"файл \"Stations.xml\" не загружен. Исключение: {ex.Message}");
            }
        }



        public static void ЗагрузкаНазванийПутей()
        {
            try
            {
                //DEBUG-------------
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var repResolve = scope.Resolve<IPathwaysRepository>();
                    var pathWaysServ = new PathwaysService(repResolve);
                    var listPathwayses = pathWaysServ.GetAll();
                }
                //DEBUG-------------


                //var xmlFile = XmlWorker.LoadXmlFile(string.Empty, "PathNames.xml"); //все настройки в одном файле
                //if (xmlFile == null)
                //    return;

                //PathWaysRepository = new RepositoryXmlPathways(xmlFile);                 //хранилище XML
                ////var directionRep = new RepositoryEf<Pathways>(dbContext);              //хранилище БД
            }
            catch (Exception ex)
            {
                MessageBox.Show($"файл \"PathNames.xml\" не загружен. Исключение: {ex.Message}");
            }
        }



        public static List<Station> ПолучитьСтанцииНаправления(string имяНаправления)
        {
            var direction = DirectionRepository.List().FirstOrDefault(d => d.Name == имяНаправления);
            var станцииНаправления = direction?.Stations.ToList();
            return станцииНаправления;
        }



        public static Station ПолучитьСтанциюНаправления(string имяНаправления, string названиеСтанцииRu)
        {
            return ПолучитьСтанцииНаправления(имяНаправления)?.FirstOrDefault(st => st.NameRu == названиеСтанцииRu);
        }



        public static bool ПроеритьНаличиеСтанцииВНаправлении(string названиеСтанцииRu, string имяНаправления)
        {
            var станция = ПолучитьСтанцииНаправления(имяНаправления)?.FirstOrDefault(st => st.NameRu == названиеСтанцииRu);
            return станция != null;
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
            AutodictorModel?.Dispose();
        }

        #endregion
    }
}
