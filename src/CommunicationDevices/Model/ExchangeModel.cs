using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using AutoMapper;
using Castle.Windsor;
using Communication.Interfaces;
using Communication.SerialPort;
using Communication.Settings;
using CommunicationDevices.Behavior;
using CommunicationDevices.Behavior.BindingBehavior.ToChange;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;
using CommunicationDevices.Behavior.BindingBehavior.ToGetData;
using CommunicationDevices.Behavior.BindingBehavior.ToPath;
using CommunicationDevices.Behavior.BindingBehavior.ToStatic;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.Behavior.ExhangeBehavior.HttpBehavior;
using CommunicationDevices.Behavior.ExhangeBehavior.PcBehavior;
using CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior;
using CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior.ChannelManagement;
using CommunicationDevices.Behavior.ExhangeBehavior.TcpIpBehavior;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;
using CommunicationDevices.DataProviders.ChannelManagementDataProvider;
using CommunicationDevices.DataProviders.VidorDataProvider;
using CommunicationDevices.DataProviders.XmlDataProvider;
using CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders;
using CommunicationDevices.Devices;
using CommunicationDevices.DI;
using CommunicationDevices.Rules.ExchangeRules;
using CommunicationDevices.Settings;
using CommunicationDevices.Settings.XmlCisSettings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings;
using Domain.Entitys;
using Library.Logs;
using Library.Xml;
using WCFAvtodictor2PcTableContract.DataContract;


namespace CommunicationDevices.Model
{
    /// <summary>
    /// ОСНОВНОЙ КЛАСС БИЗНЕСС ЛОГИКИ.
    /// СОДЕРЖИТ ВСЕ УСТРОЙСТВА, СЕРВИСЫ, ПОВЕДЕНИЯ НАД УСТРОЙСТВАМИ
    /// </summary>
    public class ExchangeModel : IDisposable
    {
        #region field

        public static Station NameRailwayStation; // Название текущего вокзала

        private readonly IWindsorContainer _container = new WindsorContainer();

        #endregion





        #region Prop

        public CisClient CisClient { get; private set; }

        public List<MasterSerialPort> MasterSerialPorts { get; set; } = new List<MasterSerialPort>();

        public List<Device> DeviceTables { get; set; } = new List<Device>();
        public Device DeviceSoundChannelManagement { get; set; } 
        

        public ICollection<IBinding2PathBehavior> Binding2PathBehaviors { get; set; } = new List<IBinding2PathBehavior>();
        public ICollection<IBinding2GeneralSchedule> Binding2GeneralSchedules { get; set; } = new List<IBinding2GeneralSchedule>();
        public ICollection<IBinding2StaticFormBehavior> Binding2StaticFormBehaviors { get; set; } = new List<IBinding2StaticFormBehavior>();
        public ICollection<IBinding2ChangesBehavior> Binding2ChangesSchedules { get; set; } = new List<IBinding2ChangesBehavior>();
        public ICollection<IBinding2ChangesEventBehavior> Binding2ChangesEvent { get; set; } = new List<IBinding2ChangesEventBehavior>();
        public ICollection<IBinding2GetData> Binding2GetData{ get; set; } = new List<IBinding2GetData>();


        private string _errorString;
        public string ErrorString
        {
            get { return _errorString; }
            set
            {
                if (value == _errorString) return;
                _errorString = value;
                //сработка события
            }
        }

        public List<Task> BackGroundTasks { get; set; } = new List<Task>();

        public IDisposable ApkDkVolgogradStreamChangeRxDispose { get; set; }

        #endregion






        #region ctor

        public ExchangeModel()
        {
            //РЕГИСТРАЦИЯ DI
            _container.Install(new WindsorConfig());

            //РЕГИСТРАЦИЯ МАППИНГА
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UniversalInputType, UniversalDisplayType>();
                cfg.CreateMap<UniversalDisplayType, UniversalInputType>();
            });
        }

        #endregion





        #region Methode

        public void StartCisClient()
        {
            CisClient?.Start();
        }


        public void StopCisClient()
        {
            CisClient?.Stop();
        }


        public async Task ReOpenMasterSerialPorts(params byte[] numberPorts)
        {
            if (numberPorts == null || !numberPorts.Any())
                return;

            var serialPorts = MasterSerialPorts.Where(sp => numberPorts.Contains(sp.PortNumber)).ToList();
            if (serialPorts.Any())
            {
                var result = await Task.WhenAll(serialPorts.Select(sp => sp.CycleReConnect()));
            }
        }



        public async void LoadSetting()
        {
            //ЗАГРУЗКА НАСТРОЕК----------------------------------------------------------------------------------------------------------------------------
            List<XmlSerialSettings> xmlSerialPorts;
            List<XmlSpSetting> xmlDeviceSpSettings;
            List<XmlPcSetting> xmlDevicePcSettings;
            List<XmlTcpIpSetting> xmlDeviceTcpIpSettings;
            List<XmlHttpSetting> xmlDeviceHttpSettings;
            XmlCisSetting xmlCisSetting;

            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Settings", "Setting.xml"); //все настройки в одном файле
                if (xmlFile == null)
                    return;

                xmlSerialPorts = XmlSerialSettings.LoadXmlSetting(xmlFile);
                xmlDeviceSpSettings = XmlSettingFactory.CreateXmlSpSetting(xmlFile);
                xmlDevicePcSettings = XmlSettingFactory.CreateXmlPcSetting(xmlFile);
                xmlDeviceTcpIpSettings = XmlSettingFactory.CreateXmlTcpIpSetting(xmlFile);
                xmlDeviceHttpSettings = XmlSettingFactory.CreateXmlHttpSetting(xmlFile);
                xmlCisSetting = XmlCisSetting.LoadXmlSetting(xmlFile);
            }
            catch (FileNotFoundException ex)
            {
                ErrorString = "Файл Setting.xml не найденн";
                Log.log.Error(ErrorString);
                return;
            }
            catch (Exception ex)
            {
                ErrorString = "ОШИБКА в узлах дерева XML файла настроек:  " + ex;
                Console.WriteLine(ex.Message);
                Log.log.Error(ErrorString);
                return;
            }


            //СОЗДАНИЕ КЛИЕНТА ЦИС---------------------------------------------------------------------------------------------------------
            NameRailwayStation = new Station
            {
                NameRu = xmlCisSetting.Name,
                NameEng = xmlCisSetting.NameEng,
                NameCh = xmlCisSetting.NameCh
            };
            CisClient = new CisClient(new EndpointAddress(xmlCisSetting.EndpointAddress), DeviceTables);



            //СОЗДАНИЕ ПОСЛЕДОВАТЕЛЬНЫХ ПОРТОВ----------------------------------------------------------------------------------------------------------
            foreach (var sp in xmlSerialPorts.Select(xmlSp => new MasterSerialPort(xmlSp)))
            {
                MasterSerialPorts.Add(sp);
            }


            #region СОЗДАНИЕ УСТРОЙСТВ С ПОСЛЕДОВАТЕЛЬНЫМ ПОРТОМ
     
            foreach (var xmlDeviceSp in xmlDeviceSpSettings)
            {
                IExhangeBehavior behavior;
                byte maxCountFaildRespowne;

                XmlBindingSetting binding = null;
                XmlConditionsSetting contrains = null;
                XmlPagingSetting paging = null;
                XmlCountRowSetting countRow = null;
                XmlPathPermissionSetting pathPermission = null;
                XmlProviderTypeSetting providerType = null;
                List<XmlExchangeRule> xmlExchangeRules = null;

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("Binding"))
                {
                    binding = xmlDeviceSp.SpecialDictionary["Binding"] as XmlBindingSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("Contrains"))
                {
                    contrains = xmlDeviceSp.SpecialDictionary["Contrains"] as XmlConditionsSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("Paging"))
                {
                    paging = xmlDeviceSp.SpecialDictionary["Paging"] as XmlPagingSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("CountRow"))
                {
                    countRow = xmlDeviceSp.SpecialDictionary["CountRow"] as XmlCountRowSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("PathPermission"))
                {
                    pathPermission = xmlDeviceSp.SpecialDictionary["PathPermission"] as XmlPathPermissionSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("ProviderType"))
                {
                    providerType = xmlDeviceSp.SpecialDictionary["ProviderType"] as XmlProviderTypeSetting;
                }

                var setting = new DeviceSetting
                {
                    PathPermission = pathPermission?.Enable ?? true
                };

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("ExchangeRules"))
                {
                    xmlExchangeRules = xmlDeviceSp.SpecialDictionary["ExchangeRules"] as List<XmlExchangeRule>;
                }


                //привязка обязательный параметр
                if (binding == null)
                {
                    MessageBox.Show($"Не указанны настройки привязки у ус-ва {xmlDeviceSp.Id}");
                    return;
                }

                switch (xmlDeviceSp.Name)
                {
                    case "DispSys":
                        maxCountFaildRespowne = 3;
                        behavior = new DisplSysExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne);
                        DeviceTables.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions);
                            Binding2PathBehaviors.Add(bindingBeh);
                            bindingBeh.InitializeDevicePathInfo();                       //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        DeviceTables.Last().AddCycleFunc();
                        break;


                    case "Vidor":
                        maxCountFaildRespowne = 3;
                        behavior = new VidorExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne);
                        DeviceTables.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions);
                            Binding2PathBehaviors.Add(bindingBeh);
                            bindingBeh.InitializeDevicePathInfo();                      //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к форме статических сообщений
                        if (binding.BindingType == BindingType.ToStatic)
                            Binding2StaticFormBehaviors.Add(new Binding2StaticFormBehavior(DeviceTables.Last()));

                        //добавим все функции циклического опроса
                        DeviceTables.Last().AddCycleFunc();
                        break;


                    case "VidorTableStr1":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (countRow == null)
                        {
                            MessageBox.Show($"Не указанны кол-во строк у многострочного табло {xmlDeviceSp.Id}");
                            return;
                        }

                        var behTable8 = new VidorTableLineByLineExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, countRow.CountRow, true, 1000)
                        {
                            ForTableViewDataProvider = new PanelVidorTable1StrWriteDataProvider()
                        };
                        DeviceTables.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behTable8, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions));

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(DeviceTables.Last(), binding.SourceLoad, contrains?.Conditions, paging?.CountPage ?? 0, paging?.TimePaging ?? 0));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                            break;
                        }

                        //создание поведения привязка табло к форме статических сообщений
                        if (binding.BindingType == BindingType.ToStatic)
                            Binding2StaticFormBehaviors.Add(new Binding2StaticFormBehavior(DeviceTables.Last()));

                        //добавим все функции циклического опроса
                        DeviceTables.Last().AddCycleFunc();
                        break;


                    case "VidorTableStr2":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (countRow == null)
                        {
                            MessageBox.Show($"Не указанны кол-во строк у многострочного табло {xmlDeviceSp.Id}");
                            return;
                        }

                        var behTableMin2 = new VidorTableLineByLineExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, countRow.CountRow, true, 10000)
                        {
                            ForTableViewDataProvider = new PanelVidorTable2StrWriteDataProvider()
                        };
                        DeviceTables.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behTableMin2, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions));


                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(DeviceTables.Last(), binding.SourceLoad, contrains?.Conditions, paging?.CountPage ?? 0, paging?.TimePaging ?? 0));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                            break;
                        }

                        //создание поведения привязка табло к форме статических сообщений
                        if (binding.BindingType == BindingType.ToStatic)
                            Binding2StaticFormBehaviors.Add(new Binding2StaticFormBehavior(DeviceTables.Last()));

                        //добавим все функции циклического опроса
                        DeviceTables.Last().AddCycleFunc();
                        break;


                    case "ChannelManagement":
                        maxCountFaildRespowne = 3;
                        if (providerType?.ProviderType != null)
                        {
                            IExchangeDataProvider<UniversalInputType, byte> provider = null;
                            switch (providerType.ProviderType.Value)
                            {
                                case ProviderType.ChMan10:
                                    provider = new ChannelManagement10ChWriteDataProvider();
                                    break;

                                case ProviderType.ChMan20:
                                    provider = new ChannelManagement20ChWriteDataProvider();
                                    break;

                                case ProviderType.ChManOnOff:
                                    provider = new ChannelManagementOnOffWriteDataProvider();
                                    break;
                            }
       
                            behavior = new ChannelManagementExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, provider);
                            DeviceSoundChannelManagement = new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, binding.BindingType, null);
                        }

                        break;


                    default:
                        // правила обмена обязательный параметр
                        if (xmlExchangeRules == null || !xmlExchangeRules.Any())
                        {
                            MessageBox.Show($"Не указанно правило обмена для устройсва: {xmlDeviceSp.Id}");
                            return;
                        }

                        //Создание списка правил обмена.
                        List<BaseExchangeRule> excangeRules= new List<BaseExchangeRule>();
                        foreach (var xmlExchangeRule in xmlExchangeRules)
                        {
                            //Запрос---------------------
                            RequestRule request = null;
                            if ((!string.IsNullOrEmpty(xmlExchangeRule.RequestBody)))
                            {
                                request = new RequestRule { MaxLenght = xmlExchangeRule.RequestMaxLenght, Body = xmlExchangeRule.RequestBody };
                            }
                            else
                            {
                                MessageBox.Show($"В правилах обмена для {xmlDeviceSp.Name} не верно заданна секция Request");
                                return;
                            }

                            //Ответ----------------------
                            ResponseRule response = null;
                            if ((xmlExchangeRule.ResponseMaxLenght > 0) || (!string.IsNullOrEmpty(xmlExchangeRule.ResponseBody)))
                            {
                                response = new ResponseRule() { MaxLenght = xmlExchangeRule.ResponseMaxLenght, Body = xmlExchangeRule.ResponseBody, Time = xmlExchangeRule.TimeResponse};
                            }

                            //Повтор--------------------
                            RepeatRule repeat = null;
                            if (xmlExchangeRule.RepeatCount.HasValue)
                            {
                                repeat = new RepeatRule { Count = xmlExchangeRule.RepeatCount.Value, DeltaX = xmlExchangeRule.RepeatDeltaX, DeltaY = xmlExchangeRule.RepeatDeltaY };
                            }

                            excangeRules.Add(new BaseExchangeRule(request, response, repeat, xmlExchangeRule.Format, xmlExchangeRule?.Conditions?.Conditions));
                        }

                        //Создание главного правила обмена
                        var mainRules = new MainRule
                        {
                            ExchangeRules = excangeRules,
                            ViewType =
                                new ViewType
                                {
                                    Type = xmlExchangeRules.FirstOrDefault()?.ViewType,
                                    TableSize = xmlExchangeRules.FirstOrDefault()?.TableSize,
                                    FirstTableElement = xmlExchangeRules.FirstOrDefault()?.FirstTableElement,
                                }
                        };

                        maxCountFaildRespowne = 3;
                        switch (mainRules.ViewType.Type)
                        {
                            case "":
                                behavior= new ByRulesExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, excangeRules);
                                break;

                            case "Table":
                                behavior= new ByRulesTableExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber),xmlDeviceSp.TimeRespone, maxCountFaildRespowne, mainRules);
                                break;

                            default:
                                MessageBox.Show($"Тип отображения {mainRules.ViewType.Type} не поддерживается");
                                continue;
                        }

                        DeviceTables.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions);
                            Binding2PathBehaviors.Add(bindingBeh);
                            // bindingBeh.InitializeDevicePathInfo();                      //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(DeviceTables.Last(), binding.SourceLoad, contrains?.Conditions, paging?.CountPage ?? 0, paging?.TimePaging ?? 0));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                            break;
                        }

                        //создание поведения привязка табло к форме статических сообщений
                        if (binding.BindingType == BindingType.ToStatic)
                            Binding2StaticFormBehaviors.Add(new Binding2StaticFormBehavior(DeviceTables.Last()));
                            


                        //добавим все функции циклического опроса
                        DeviceTables.Last().AddCycleFunc();
                        break;
                }
            }

            #endregion




            #region СОЗДАНИЕ УСТРОЙСТВ С PC

            foreach (var xmlDevicePc in xmlDevicePcSettings)
            {
                IExhangeBehavior behavior;
                byte maxCountFaildRespowne;

                XmlBindingSetting binding = null;
                XmlConditionsSetting contrains = null;
                XmlPagingSetting paging = null;
                XmlCountRowSetting countRow = null;
                XmlPathPermissionSetting pathPermission = null;

                if (xmlDevicePc.SpecialDictionary.ContainsKey("Binding"))
                {
                    binding = xmlDevicePc.SpecialDictionary["Binding"] as XmlBindingSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("Contrains"))
                {
                    contrains = xmlDevicePc.SpecialDictionary["Contrains"] as XmlConditionsSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("Paging"))
                {
                    paging = xmlDevicePc.SpecialDictionary["Paging"] as XmlPagingSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("CountRow"))
                {
                    countRow = xmlDevicePc.SpecialDictionary["CountRow"] as XmlCountRowSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("PathPermission"))
                {
                    pathPermission = xmlDevicePc.SpecialDictionary["PathPermission"] as XmlPathPermissionSetting;
                }

                var setting = new DeviceSetting
                {
                    PathPermission = pathPermission?.Enable ?? true
                };

                //привязка обязательный параметр
                if (binding == null)
                {
                    MessageBox.Show($"Не указанны настройки привязки у ус-ва {xmlDevicePc.Id}");
                    return;
                }

                switch (xmlDevicePc.Name)
                {
                    case "PcTable":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (paging == null)
                        {
                            MessageBox.Show($"Не указанны настройки paging у PcTable табло {xmlDevicePc.Id}");
                            return;
                        }

                        behavior = new ArivDepartExhangePcBehavior(xmlDevicePc.Address, maxCountFaildRespowne);
                        DeviceTables.Add(new Device(xmlDevicePc.Id, xmlDevicePc.Address, xmlDevicePc.Name, xmlDevicePc.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions));
                            DeviceTables.Last().AddCycleFunc(); //добавим все функции циклического опроса
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(DeviceTables.Last(), binding.SourceLoad, contrains?.Conditions, paging.CountPage, paging.TimePaging));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                        }

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
                            ;


                        break;


                    default:
                        ErrorString = $" Устройсвто с именем {xmlDevicePc.Name} не найденно";
                        Log.log.Error(ErrorString);
                        break;
                        //throw new Exception(ErrorString);
                }
            }

            #endregion




            #region СОЗДАНИЕ УСТРОЙСТВ С TcpIp

            foreach (var xmlDeviceTcpIp in xmlDeviceTcpIpSettings)
            {
                IExhangeBehavior behavior;
                byte maxCountFaildRespowne;

                XmlBindingSetting binding = null;
                XmlConditionsSetting contrains = null;
                XmlPagingSetting paging = null;
                XmlCountRowSetting countRow = null;
                XmlPathPermissionSetting pathPermission = null;
                List<XmlExchangeRule> xmlExchangeRules = null;

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("Binding"))
                {
                    binding = xmlDeviceTcpIp.SpecialDictionary["Binding"] as XmlBindingSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("Contrains"))
                {
                    contrains = xmlDeviceTcpIp.SpecialDictionary["Contrains"] as XmlConditionsSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("Paging"))
                {
                    paging = xmlDeviceTcpIp.SpecialDictionary["Paging"] as XmlPagingSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("CountRow"))
                {
                    countRow = xmlDeviceTcpIp.SpecialDictionary["CountRow"] as XmlCountRowSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("PathPermission"))
                {
                    pathPermission = xmlDeviceTcpIp.SpecialDictionary["PathPermission"] as XmlPathPermissionSetting;
                }

                var setting = new DeviceSetting
                {
                    PathPermission = pathPermission?.Enable ?? true
                };

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("ExchangeRules"))
                {
                    xmlExchangeRules = xmlDeviceTcpIp.SpecialDictionary["ExchangeRules"] as List<XmlExchangeRule>;
                }

                //привязка обязательный параметр
                if (binding == null)
                {
                    MessageBox.Show($"Не указанны настройки привязки у ус-ва {xmlDeviceTcpIp.Id}");
                    return;
                }


                switch (xmlDeviceTcpIp.Name)
                {
                    case "VidorTableStr1":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (countRow == null)
                        {
                            MessageBox.Show($"Не указанны кол-во строк у многострочного табло {xmlDeviceTcpIp.Id}");
                            return;
                        }

                        behavior = new VidorTableLineByLineExchangeTcpIpBehavior(xmlDeviceTcpIp.Address, xmlDeviceTcpIp.DeviceAdress, maxCountFaildRespowne, xmlDeviceTcpIp.TimeRespone, countRow.CountRow, true, 1000)
                        {
                            ForTableViewDataProvider = new PanelVidorTable1StrWriteDataProvider()
                        };

                        DeviceTables.Add(new Device(xmlDeviceTcpIp.Id, xmlDeviceTcpIp.Address, xmlDeviceTcpIp.Name, xmlDeviceTcpIp.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions));

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(DeviceTables.Last(), binding.SourceLoad, contrains?.Conditions, paging?.CountPage ?? 0, paging?.TimePaging ?? 0));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                            break;
                        }

                        //создание поведения привязка табло к форме статических сообщений
                        if (binding.BindingType == BindingType.ToStatic)
                            Binding2StaticFormBehaviors.Add(new Binding2StaticFormBehavior(DeviceTables.Last()));

                        //добавим все функции циклического опроса
                        DeviceTables.Last().AddCycleFunc();
                        break;


                    default:
                        // правила обмена обязательный параметр
                        if (xmlExchangeRules == null || !xmlExchangeRules.Any())
                        {
                            MessageBox.Show($"Не верно заданно правило обмена для устройства: {xmlDeviceTcpIp.Id}");
                            return;
                        }

                        //Создание списка правил обмена.
                        List<BaseExchangeRule> excangeRules = new List<BaseExchangeRule>();
                        foreach (var xmlExchangeRule in xmlExchangeRules)
                        {
                            //Запрос---------------------
                            RequestRule request = null;
                            if ((!string.IsNullOrEmpty(xmlExchangeRule.RequestBody)))
                            {
                                request = new RequestRule { MaxLenght = xmlExchangeRule.RequestMaxLenght, Body = xmlExchangeRule.RequestBody };
                            }
                            else
                            {
                                MessageBox.Show($"В правилах обмена для {xmlDeviceTcpIp.Name} не верно заданна секция Request");
                                return;
                            }

                            //Ответ----------------------
                            ResponseRule response = null;
                            if ((xmlExchangeRule.ResponseMaxLenght > 0) || (!string.IsNullOrEmpty(xmlExchangeRule.ResponseBody)))
                            {
                                response = new ResponseRule() { MaxLenght = xmlExchangeRule.ResponseMaxLenght, Body = xmlExchangeRule.ResponseBody, Time = xmlExchangeRule.TimeResponse };
                            }

                            //Повтор--------------------
                            RepeatRule repeat = null;
                            if (xmlExchangeRule.RepeatCount.HasValue)
                            {
                                repeat = new RepeatRule { Count = xmlExchangeRule.RepeatCount.Value, DeltaX = xmlExchangeRule.RepeatDeltaX, DeltaY = xmlExchangeRule.RepeatDeltaY };
                            }

                            excangeRules.Add(new BaseExchangeRule(request, response, repeat, xmlExchangeRule.Format, xmlExchangeRule?.Conditions?.Conditions));
                        }

                        //Создание главного правила обмена
                        var mainRule = new MainRule
                        {
                            ExchangeRules = excangeRules,
                            ViewType =
                                new ViewType
                                {
                                    Type = xmlExchangeRules.FirstOrDefault()?.ViewType,
                                    TableSize = xmlExchangeRules.FirstOrDefault()?.TableSize,
                                    FirstTableElement = xmlExchangeRules.FirstOrDefault()?.FirstTableElement,
                                }
                        };

                        maxCountFaildRespowne = 3;
                        switch (mainRule.ViewType.Type)
                        {
                            case "":
                                behavior = null;//new ByRulesTableExchangeTcpIpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, excangeRules);
                                break;

                            case "Table":
                                behavior = new ByRulesTableExchangeTcpIpBehavior(xmlDeviceTcpIp.Address, xmlDeviceTcpIp.DeviceAdress, maxCountFaildRespowne, xmlDeviceTcpIp.TimeRespone, mainRule, 1000);
                                break;

                            default:
                                MessageBox.Show($"Тип отображения {mainRule.ViewType.Type} не поддерживается");
                                continue;
                        }

                        DeviceTables.Add(new Device(xmlDeviceTcpIp.Id, xmlDeviceTcpIp.Address, xmlDeviceTcpIp.Name, xmlDeviceTcpIp.Description, behavior, binding.BindingType, setting));


                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions);
                            Binding2PathBehaviors.Add(bindingBeh);
                            // bindingBeh.InitializeDevicePathInfo();                      //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(DeviceTables.Last(), binding.SourceLoad, contrains?.Conditions, paging?.CountPage ?? 0, paging?.TimePaging ?? 0));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                            break;
                        }

                        //создание поведения привязка табло к форме статических сообщений
                        if (binding.BindingType == BindingType.ToStatic)
                            Binding2StaticFormBehaviors.Add(new Binding2StaticFormBehavior(DeviceTables.Last()));

                        //добавим все функции циклического опроса
                        DeviceTables.Last().AddCycleFunc();
                        break;
                }
            }

            #endregion




            #region СОЗДАНИЕ УСТРОЙСТВ С HTTP

            foreach (var xmlDeviceHttp in xmlDeviceHttpSettings)
            {
                XmlBindingSetting binding = null;
                XmlConditionsSetting contrains = null;
                XmlPagingSetting paging = null;
                XmlPathPermissionSetting pathPermission = null;
                XmlProviderTypeSetting providerType = null;

                if (xmlDeviceHttp.SpecialDictionary.ContainsKey("Binding"))
                {
                    binding = xmlDeviceHttp.SpecialDictionary["Binding"] as XmlBindingSetting;
                }

                if (xmlDeviceHttp.SpecialDictionary.ContainsKey("Contrains"))
                {
                    contrains = xmlDeviceHttp.SpecialDictionary["Contrains"] as XmlConditionsSetting;
                }

                if (xmlDeviceHttp.SpecialDictionary.ContainsKey("Paging"))
                {
                    paging = xmlDeviceHttp.SpecialDictionary["Paging"] as XmlPagingSetting;
                }

                if (xmlDeviceHttp.SpecialDictionary.ContainsKey("ProviderType"))
                {
                    providerType = xmlDeviceHttp.SpecialDictionary["ProviderType"] as XmlProviderTypeSetting;
                }

                if (xmlDeviceHttp.SpecialDictionary.ContainsKey("PathPermission"))
                {
                    pathPermission = xmlDeviceHttp.SpecialDictionary["PathPermission"] as XmlPathPermissionSetting;
                }

                var setting = new DeviceSetting
                {
                    PathPermission = pathPermission?.Enable ?? true
                };

                //привязка обязательный параметр
                if (binding == null)
                {
                    MessageBox.Show($"Не указанны настройки привязки у ус-ва {xmlDeviceHttp.Id}");
                    return;
                }

                //заголовок обязательный параметр
                if (!xmlDeviceHttp.Headers.Any())
                {
                    MessageBox.Show($"Не указан заголовок HTTP протокола {xmlDeviceHttp.Id}");
                    return;
                }


                BaseGetDataBehavior getDataBehavior = null;
                byte maxCountFaildRespowne = 3;
                if (providerType?.ProviderType != null)
                {
                    IExchangeDataProvider<UniversalInputType, Stream> provider = null;
                    var httpBeh= new XmlExhangeHttpBehavior(xmlDeviceHttp.Address, xmlDeviceHttp.Headers, maxCountFaildRespowne, xmlDeviceHttp.TimeRespone, xmlDeviceHttp.Period, provider);
                    switch (providerType.ProviderType.Value)
                    {
                        case ProviderType.XmlTlist:
                            provider = new StreamWriteDataProvider(new XmlTListFormatProvider());
                            break;

                        case ProviderType.XmlMainWindow:
                            provider = new StreamWriteDataProvider(new XmlMainWindowFormatProvider(providerType.DateTimeFormat, providerType.TransitSortFormat));
                            break;

                        case ProviderType.XmlSheduleWindow:
                            provider = new StreamWriteDataProvider(new XmlSheduleWindowFormatProvider(providerType.DateTimeFormat, providerType.TransitSortFormat));
                            break;

                        case ProviderType.XmlStaticWindow:
                            provider = new StreamWriteDataProvider(new XmlStaticWindowFormatProvider());
                            break;

                        case ProviderType.XmlChange:
                            provider = new StreamWriteDataProvider(new XmlChangesFormatProvider(providerType.DateTimeFormat));
                            break;

                        case ProviderType.XmlApkDkMoscow:
                            provider = new StreamWriteDataProvider(new XmlApkDkMoscowFormatProvider(providerType.Login, providerType.Password, providerType.EcpCode));
                            break;

                        case ProviderType.XmlApkDkGet:
                            provider = new StreamWriteDataProvider(new XmlGetFormatProvider());
                            IInputDataConverter dataConverter = null;
                            switch (xmlDeviceHttp.Name)
                            {
                                case "HttpApkDkVolgograd":
                                    dataConverter = new ApkDkVolgogradSheduleDataConverter();
                                    break;

                                case "HttpDispatcher":
                                    dataConverter = new DispatcherControlDataConverter();
                                    break;

                                case "HttpCisRegSh":
                                    dataConverter = new CisRegularShDataConverter();
                                    break;

                                case "HttpCisOperSh":
                                    dataConverter = new CisOperativeShDataConverter();
                                    break;
                            }

                            getDataBehavior = new BaseGetDataBehavior(xmlDeviceHttp.Name, httpBeh.IsConnectChange, httpBeh.IsDataExchangeSuccessChange, provider.OutputDataChangeRx, dataConverter);
                            break;

                    }

                    httpBeh.XmlExcangeDataProvider = provider;
                    httpBeh.XmlExcangeDataProvider.ProviderName = provider.ProviderName;
                    DeviceTables.Add(new Device(xmlDeviceHttp.Id, xmlDeviceHttp.Address, xmlDeviceHttp.Name, xmlDeviceHttp.Description, httpBeh, binding.BindingType, setting));
                }


                //создание поведения привязка табло к пути.
                if (binding.BindingType == BindingType.ToPath)
                {
                    var bindingBeh = new Binding2PathBehavior(DeviceTables.Last(), binding.PathNumbers, contrains?.Conditions);
                    Binding2PathBehaviors.Add(bindingBeh);
                    bindingBeh.InitializeDevicePathInfo();     //Вывод номера пути в пустом сообщении
                    DeviceTables.Last().AddCycleFunc();        //Добавим все функции циклического опроса          
                }

                //создание поведения привязка табло к главному расписанию
                if (binding.BindingType == BindingType.ToGeneral)
                {
                    Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(DeviceTables.Last(), binding.SourceLoad, contrains?.Conditions, paging?.CountPage ?? 0, paging?.TimePaging ?? 0));
                    //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                    if (!Binding2GeneralSchedules.Last().IsPaging)
                    {
                        DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                    }
                }

                //создание поведения привязка табло к форме статических сообщений
                if (binding.BindingType == BindingType.ToStatic)
                    Binding2StaticFormBehaviors.Add(new Binding2StaticFormBehavior(DeviceTables.Last()));

                //создание поведения привязка табло к Изменениям
                if (binding.BindingType == BindingType.ToChange)
                {
                    Binding2ChangesSchedules.Add(new Binding2ChangesBehavior(DeviceTables.Last(),  binding.HourDepth, contrains?.Conditions, paging?.CountPage ?? 0, paging?.TimePaging ?? 0));
                    //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                    if (!Binding2ChangesSchedules.Last().IsPaging)
                    {
                        DeviceTables.Last().AddCycleFunc();//добавим все функции циклического опроса
                    }
                }

                //создание поведения привязка табло к отпрвки изменения по событию
                if (binding.BindingType == BindingType.ToChangeEvent)
                {
                    Binding2ChangesEvent.Add(new Binding2ChangesEventBehavior(DeviceTables.Last()));
                }

                //создание поведения привязка ус-ва к серверу получения информации
                if (binding.BindingType == BindingType.ToGetData)
                {
                    Binding2GetData.Add(new Binding2GetData(DeviceTables.Last(), getDataBehavior));
                    DeviceTables.Last().AddCycleFunc();
                }
            }

            #endregion





            //ЗАПУСТИМ ФОНОВЫЕ ЗАДАЧИ ПО ПОДКЛЮЧЕНИЮ К УСТРО-ВАМ
            //Защита от повторного открытия одного и тогоже порта разными ус-вами.   
            var serialPortDev = DeviceTables.Where(d => d.ExhBehavior is BaseExhangeSpBehavior).ToList();
            foreach (var devSp in serialPortDev.GroupBy(d => d.ExhBehavior.NumberPort).Select(g => g.First()))
            {
                devSp.ExhBehavior.CycleReConnect(BackGroundTasks);
            }

            if (DeviceSoundChannelManagement != null)
            {
                //Если ус-во настройки каналов звука подключенно на неиспользумый другими ус-вами порт, то запустим порт на обмен данными.
                var findDevSp = serialPortDev.FirstOrDefault(spDev => spDev.ExhBehavior.NumberPort == DeviceSoundChannelManagement.ExhBehavior.NumberPort);
                if (findDevSp == null)
                    DeviceSoundChannelManagement.ExhBehavior.CycleReConnect(BackGroundTasks);
            }


            var otherDev = DeviceTables.Except(serialPortDev).ToList();
            foreach (var devSp in otherDev)
            {
                devSp.ExhBehavior.CycleReConnect(BackGroundTasks);
            }
        }




        public void InitializeDeviceSoundChannelManagement()
        {
            if (DeviceSoundChannelManagement == null)
                return;

            //Команда инициализации 1
            var soundChUit = new UniversalInputType { ViewBag = new Dictionary<string, dynamic>() };
            soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "InitSoundChanelDevice_step1";
            DeviceSoundChannelManagement.AddOneTimeSendData(soundChUit);

            //Команда инициализации 2
            soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "InitSoundChanelDevice_step2";
            DeviceSoundChannelManagement.AddOneTimeSendData(soundChUit);
        }


        #endregion





        public void Dispose()
        {
            CisClient?.Dispose();
            MasterSerialPorts?.ForEach(s => s.Dispose());
            ApkDkVolgogradStreamChangeRxDispose?.Dispose();
        }
    }
}
