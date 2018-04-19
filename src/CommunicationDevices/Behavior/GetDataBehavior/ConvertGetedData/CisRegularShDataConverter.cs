using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{
    class CisRegularShDataConverter : IInputDataConverter
    {
        public IEnumerable<UniversalInputType> ParseXml2Uit(XDocument xDoc)
        {
            //Log.log.Trace("xDoc" + xDoc.ToString());//LOG;
            var shedules = new List<UniversalInputType>();

            var lines = xDoc.Element("changes")?.Elements().ToList() ?? xDoc.Element("users").Elements("user").ToList();
            if (lines != null)
            {
                for (var i = 0; i < lines.Count; i++)
                //foreach (var line in lines)
                {
                    var line = lines[i];
                    var uit = new UniversalInputType
                    {
                        ViewBag = new Dictionary<string, dynamic>(),
                        TransitTime = new Dictionary<string, DateTime>()
                    };

                    try
                    {
                        //TransitTime["приб"]-----
                        var prib = StringTrim(line, "InDateTime");

                        //TransitTime["отпр"]-----
                        var otpr = StringTrim(line, "OutDateTime");

                        //StartStation--------------------
                        //elem = line?.Element("StartStation")?.Value.Replace("\\", "/") ?? string.Empty;
                        int expressStart;
                        int.TryParse(StringTrim(line, "ExpressStart"), out expressStart);

                        int esrStart;
                        int.TryParse(StringTrim(line, "EsrStart"), out esrStart);

                        //EndStation-----------------------
                        int expressEnd;
                        int.TryParse(StringTrim(line, "ExpressEnd"), out expressEnd);

                        //EndStation-----------------------
                        int esrEnd;
                        int.TryParse(StringTrim(line, "EsrEnd"), out esrEnd);

                        // отсекаем промежуточные станции
                        //if ((prib == otpr) || (prib.Equals("-1") && endCode == 2006004) || (otpr.Equals("-1") && startCode == 2006004))
                        //    continue;

                        if (!prib.Equals(-1))
                        {
                            DateTime dtPrib;
                            DateTime.TryParse(ParseIntTimeToTextTime(prib), out dtPrib);
                            uit.TransitTime["приб"] = dtPrib;
                        }

                        if (!otpr.Equals(-1))
                        {
                            DateTime dtOtpr;
                            DateTime.TryParse(ParseIntTimeToTextTime(otpr), out dtOtpr);
                            uit.TransitTime["отпр"] = dtOtpr;
                        }

                        //string startStation = Regex.Replace(stationCisToLocal(startCode), "[\r\n\t]+", "");
                        uit.StationDeparture = new Station
                        {
                            CodeExpress = expressStart,
                            CodeEsr = esrStart
                        };

                        //string endStation = Regex.Replace(stationCisToLocal(endCode), "[\r\n\t]+", "");
                        uit.StationArrival = new Station
                        {
                            CodeExpress = expressEnd,
                            CodeEsr = esrEnd
                        };

                        //Stations------ возможно перенести после StartStation и EndStation и объединить их
                        // uit.Stations = uit.StationDeparture + " - " + uit.StationArrival;
                        //elem = line?.Element("Itenary")?.Value.Replace("\\", "/") ?? string.Empty; // Разве "//" встречаются в Itenary?
                        //var stations = Regex.Replace(elem, "[\r\n\t]+", "");
                        //uit.Stations = uit.StationDeparture.NameRu + " - " + uit.StationArrival.NameRu;

                        //Id----------
                        int id;
                        if (int.TryParse(StringTrim(line, "ID"), out id))
                        {
                            uit.ScheduleId = id; // парсим в ID
                        }

                        //NumberOfTrain------ NumberOfTrain - проверить, куда пишет значения в файл TableRecords
                        var numberOfTrain1 = StringTrim(line, "TrainNumber");
                        var numberOfTrain2 = StringTrim(line, "SecondTrainNumber");

                        uit.NumberOfTrain = (string.IsNullOrWhiteSpace(numberOfTrain2) || (numberOfTrain1 == numberOfTrain2))
                                             ? numberOfTrain1
                                             : numberOfTrain1 + "/" + numberOfTrain2; // хардкод для обхода неверных данных на серверах, только где нет транзитов


                        //StopTime--------------- 
                        TimeSpan stopTime;
                        if (TimeSpan.TryParse(StringTrim(line, "HereDateTime"), out stopTime))
                        {
                            uit.StopTime = stopTime;
                        }
                        else
                        {
                            //uit.StopTime = uit.TransitTime["отпр"] > uit.TransitTime["приб"] ? uit.TransitTime["отпр"] - uit.TransitTime["приб"] : default(TimeSpan);
                        }

                        //DaysFollowing------ самое сложное - для дальнего следования циклом проходим каждый день и записываем это в "АктивностьДня"
                        // для пригорода - преобразуем вариант "По рабочим" и т.д. в РаботаПоДням
                        var str = StringTrim(line, "DaysOfGoing").Split('-');
                        var dayStr = string.Empty;
                        if (str.Length == 3)
                        {
                            dayStr = $"{str[2]}.{str[1]}.{str[0]}";
                        }
                        uit.DaysFollowing = dayStr;

                        //Enabled------------
                        uit.ViewBag["Enabled"] = StringTrim(line, "Enabled");

                        //SoundTemplate------
                        uit.ViewBag["SoundTemplate"] = StringTrim(line, "SoundTemplate");

                        //VagonDirection------ нет возможности собирать эту информацию из ЦИС
                        int vagonDirection;
                        if (int.TryParse(StringTrim(line, "VagonDirection"), out vagonDirection))
                        {
                            uit.WagonsNumbering = (WagonsNumbering)vagonDirection;
                        }

                        //DefaultsPaths------------- нет возможности собирать эту информацию из ЦИС
                        uit.ViewBag["DefaultsPaths"] = StringTrim(line, "DefaultsPaths");

                        //TrainType---------
                        uit.TypeTrain = StringTrimToTitleCase(line, "TrainType");

                        //Stops------
                        uit.Note = StringTrim(line, "Stops");

                        //ScheduleStartDateTime---------------
                        DateTime dtStartDateTime = DateTime.MinValue;
                        DateTime.TryParse(StringTrim(line, "ScheduleStartDateTime"), out dtStartDateTime);
                        uit.ViewBag["ScheduleStartDateTime"] = dtStartDateTime;

                        //ScheduleEndDateTime---------------
                        DateTime dtEndDateTime = DateTime.MaxValue;
                        DateTime.TryParse(StringTrim(line, "ScheduleEndDateTime"), out dtEndDateTime);
                        uit.ViewBag["ScheduleEndDateTime"] = dtEndDateTime;

                        //Addition---------------
                        uit.Addition = StringTrimToTitleCase(line, "Addition");

                        //AdditionSend--------------- не принимаем из ЦИС, по умолчанию сделать 1
                        uit.ViewBag["AdditionSend"] = StringTrim(line, "AdditionSend");

                        //AdditionSendSound------------- не принимаем из ЦИС, по умолчанию сделать 1
                        uit.ViewBag["AdditionSendSound"] = StringTrim(line, "AdditionSendSound");

                        //SoundsType------------------- не принимаем из ЦИС, по умолчанию Автомат

                        //ItenaryTime--------------
                        var itenaryTime = StringTrim(line, "ItenaryTime");
                        int intTime;
                        int.TryParse(itenaryTime, out intTime);
                        intTime %= 24 * 60;
                        itenaryTime = intTime / 60 + ":" + (intTime % 60 < 10 ? "0" : string.Empty) + (intTime % 60);
                        //DateTime itenTime;
                        //DateTime.TryParse(itenaryTime, out itenTime);
                        uit.ViewBag["ItenaryTime"] = itenaryTime;

                        //DaysFollowingAlias-------------- для дальних вычисляем динамически циклом
                        // для пригорода - получаем из ЦИС
                        uit.DaysFollowingAlias = StringTrim(line, "DaysOfGoingAlias");


                        //DirectionStation----------------------- из ЦИС принимаем станции следования. 
                        // Либо проверить соответствие конечных станций, либо игнорировать поле
                        uit.DirectionStation = StringTrim(line, "Direction");

                        //VagonDirectionChanging----------- не принимаем из ЦИС, по умолчанию сделать 0
                        bool changeVagonDirection;
                        if (bool.TryParse(StringTrim(line, "VagonDirectionChanging"), out changeVagonDirection))
                        {
                            uit.ChangeVagonDirection = changeVagonDirection;
                        }

                        //login------------
                        uit.ViewBag["login"] = StringTrim(line, "login");

                        //password------------
                        uit.ViewBag["hash_salt_pass"] = StringTrim(line, "hash_salt_pass");

                        //role id------------
                        int role_id;
                        uit.ViewBag["role"] = int.TryParse(StringTrim(line, "role"), out role_id) ? role_id : 0;

                        //ФИО ------------
                        uit.ViewBag["FullName"] = $"{StringTrim(line, "surname")} {StringTrim(line, "name")?.FirstOrDefault()}.{StringTrim(line, "patronymic")?.FirstOrDefault()}.";

                        //Status------------
                        int status_id;
                        uit.ViewBag["status"] = int.TryParse(StringTrim(line, "status"), out status_id) ? status_id == 2 : false;

                        //Start_date------------
                        DateTime date;
                        uit.ViewBag["start_date"] = DateTime.TryParse(StringTrim(line, "start_date"), out date) ? date : new DateTime(1900, 01, 01);
                        //ent_date------------
                        uit.ViewBag["end_date"] = DateTime.TryParse(StringTrim(line, "end_date"), out date) ? date : new DateTime(2100, 12, 31);

                        //DateTime dtLate;
                        //if (DateTime.TryParse(StringTrim(line, "LateTime"), out dtLate))
                        //{
                        //    uit.DelayTime = dtLate;
                        //}

                        //byte emergencySituation;
                        //byte.TryParse(StringTrim(line, "EmergencySituation"), out emergencySituation);
                        //uit.EmergencySituation = emergencySituation;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show($"Ошибка: {ex.Message}");
                    }

                    shedules.Add(uit);
                }
            }


            return shedules;
        }

        private string ParseIntTimeToTextTime(string time)
        {
            var lengthIn = time.Length;
            // Вставляем двоеточие в середину, чтобы можно было распарсить в число
            return $"{time.Substring(0, lengthIn / 2)}:{time.Substring(lengthIn / 2, lengthIn - lengthIn / 2)}";
        }

        private string StringTrim(XElement line, string s)
        {
            var elem = line?.Element(s)?.Value.Replace("\\", "/") ?? string.Empty;
            return Regex.Replace(elem.TrimEnd(), "[\r\n\t]+", "");
        }
        private string StringTrimToTitleCase(XElement line, string s)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(StringTrim(line, s).ToLower());
        }
    }
}
