using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings;



namespace CommunicationDevices.Settings
{
    public static class XmlSettingFactory
    {

        #region Methode

        /// <summary>
        /// Создание списка настроек для устройств с послед. портом
        /// </summary>
        public static List<XmlSpSetting> CreateXmlSpSetting(XElement xml)
        {
            var devSp = xml?.Element("DevicesWithSP")?.Elements("DeviceSp").ToList();
            var listSpSett = new List<XmlSpSetting>();


            if (devSp == null || !devSp.Any())
                return listSpSett;


            foreach (var el in devSp)
            {
                var spSett = new XmlSpSetting(
                                   (string)el.Attribute("Id"),
                                   (string)el.Attribute("Name"),
                                   (string)el.Attribute("Port"),
                                   (string)el.Attribute("Address"),
                                   (string)el.Attribute("TimeRespone"),
                                   (string)el.Attribute("Description"));

                var bind = (string)el.Attribute("Binding");
                //if (bind != null)
                {
                    spSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    spSett.SpecialDictionary.Add("Contrains", new XmlConditionsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    spSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }

                var countRow = (string)el.Attribute("CountRow");
                if (countRow != null)
                {
                    spSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
                }

                var providerType = (string)el.Attribute("ProviderType");
                if (providerType != null)
                {
                    spSett.SpecialDictionary.Add("ProviderType", new XmlProviderTypeSetting(providerType));
                }

                if (el.Element("Settings") != null)
                {
                    var pathPermissionElem = el.Element("Settings")?.Element("PathPermission");
                    if (pathPermissionElem != null)
                    {
                        var pathPermissionEnable = (string)pathPermissionElem.Attribute("Enable");
                        spSett.SpecialDictionary.Add("PathPermission", new XmlPathPermissionSetting(pathPermissionEnable));
                    }
                }


                if (el.Element("ExchangeRules") != null)
                {
                    var exchangeRules = new List<XmlExchangeRule>();

                    var tableElement = el.Element("ExchangeRules")?.Element("Table");

                    string viewSetting = String.Empty;
                    int viewSettingTableSize = 0;
                    int viewSettingFirstTableElement = 0;

                    IEnumerable<XElement> ruleElements;
                    if (tableElement != null)
                    {
                        ruleElements = tableElement.Elements("Rule");

                        viewSetting = "Table";
                        var tableSize = (string)tableElement.Attribute("Size");
                        if (tableSize != null)
                        {
                            int.TryParse(tableSize, out viewSettingTableSize);
                        }

                        var firstTableElement = (string)tableElement.Attribute("Position");
                        if (firstTableElement != null)
                        {
                            int.TryParse(firstTableElement, out viewSettingFirstTableElement);
                        }

                    }
                    else
                    {
                        ruleElements = el.Element("ExchangeRules")?.Elements("Rule");
                    }



                    if (ruleElements != null)
                    {
                        foreach (var ruleElem in ruleElements)
                        {
                            if (ruleElem != null)
                            {
                                var exchRule = new XmlExchangeRule { TableSize = viewSettingTableSize, ViewType = viewSetting, FirstTableElement = viewSettingFirstTableElement};

                                exchRule.Format = (string)ruleElem.Attribute("Format");
                                exchRule.Conditions = ruleElem.Attribute("Resolution") == null ? null : new XmlConditionsSetting((string)ruleElem.Attribute("Resolution"));

                                //REPEAT-------------------------
                                var repeat = ruleElem.Element("Repeat");
                                if (repeat != null)
                                {
                                    var count = (string)repeat.Attribute("Count");
                                    if (count != null)
                                    {
                                        int countint;
                                        if (int.TryParse(count, out countint))
                                        {
                                            exchRule.RepeatCount = countint;
                                        }
                                    }

                                    var deltaX = repeat.Element("DeltaX");
                                    if (deltaX != null)
                                    {
                                        int deltaXint;
                                        if (int.TryParse(deltaX.Value, out deltaXint))
                                        {
                                            exchRule.RepeatDeltaX = deltaXint;
                                        }
                                    }

                                    var deltaY = repeat.Element("DeltaY");
                                    if (deltaY != null)
                                    {
                                        int deltaYint;
                                        if (int.TryParse(deltaY.Value, out deltaYint))
                                        {
                                            exchRule.RepeatDeltaX = deltaYint;
                                        }
                                    }
                                }

                                //REQUEST-------------------------
                                var request = ruleElem.Element("Request");
                                if (request != null)
                                {
                                    int maxLenght;
                                    if (int.TryParse((string)request.Attribute("maxLenght"), out maxLenght))
                                    {
                                        exchRule.RequestMaxLenght = maxLenght;
                                    }

                                    exchRule.RequestBody = request.Value.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                                }


                                //RESPONSE-------------------------
                                var response = ruleElem.Element("Response");
                                if (response != null)
                                {
                                    int maxLenght;
                                    if (int.TryParse((string)response.Attribute("maxLenght"), out maxLenght))
                                    {
                                        exchRule.ResponseMaxLenght = maxLenght;
                                    }

                                    int timeResp;
                                    if (int.TryParse((string)response.Attribute("TimeRespone"), out timeResp))
                                    {
                                        exchRule.TimeResponse = timeResp;
                                    }

                                    exchRule.ResponseBody = response.Value.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                                }

                                exchangeRules.Add(exchRule);
                            }
                        }

                        spSett.SpecialDictionary.Add("ExchangeRules", exchangeRules);
                    }
                }

                listSpSett.Add(spSett);
            }

            return listSpSett;
        }



        /// <summary>
        /// Создание списка настроек для устройств подключенных по TCP/Ip
        /// </summary>
        public static List<XmlTcpIpSetting> CreateXmlTcpIpSetting(XElement xml)
        {
            var devTcpIp = xml?.Element("DevicesWithTcpIp")?.Elements("DeviceTcpIp").ToList();
            var listTcpIpSett = new List<XmlTcpIpSetting>();


            if (devTcpIp == null || !devTcpIp.Any())
                return listTcpIpSett;


            foreach (var el in devTcpIp)
            {
                var tcpIpSett = new XmlTcpIpSetting(
                                   (string)el.Attribute("Id"),
                                   (string)el.Attribute("Name"),
                                   (string)el.Attribute("Address"),
                                   (string)el.Attribute("DeviceAddress"),
                                   (string)el.Attribute("TimeRespone"),
                                   (string)el.Attribute("Description"));

                var bind = (string)el.Attribute("Binding");
                if (bind != null)
                {
                    tcpIpSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    tcpIpSett.SpecialDictionary.Add("Contrains", new XmlConditionsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    tcpIpSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }

                var countRow = (string)el.Attribute("CountRow");
                if (countRow != null)
                {
                    tcpIpSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
                }

                if (el.Element("Settings") != null)
                {
                    var pathPermissionElem = el.Element("Settings")?.Element("PathPermission");
                    if (pathPermissionElem != null)
                    {
                        var pathPermissionEnable = (string)pathPermissionElem.Attribute("Enable");
                        tcpIpSett.SpecialDictionary.Add("PathPermission", new XmlPathPermissionSetting(pathPermissionEnable));
                    }
                }

                if (el.Element("ExchangeRules") != null)
                {
                    var exchangeRules = new List<XmlExchangeRule>();

                    var tableElement = el.Element("ExchangeRules")?.Element("Table");

                    string viewSetting = String.Empty;
                    int viewSettingTableSize = 0;
                    int viewSettingFirstTableElement = 0;

                    IEnumerable<XElement> ruleElements;
                    if (tableElement != null)
                    {
                        ruleElements = tableElement.Elements("Rule");

                        viewSetting = "Table";
                        var tableSize = (string)tableElement.Attribute("Size");
                        if (tableSize != null)
                        {
                            int.TryParse(tableSize, out viewSettingTableSize);
                        }

                        var firstTableElement = (string)tableElement.Attribute("Position");
                        if (firstTableElement != null)
                        {
                            int.TryParse(firstTableElement, out viewSettingFirstTableElement);
                        }
                    }
                    else
                    {
                        ruleElements = el.Element("ExchangeRules")?.Elements("Rule");
                    }




                    if (ruleElements != null)
                    {
                        foreach (var ruleElem in ruleElements)
                        {
                            if (ruleElem != null)
                            {
                                var exchRule = new XmlExchangeRule { TableSize = viewSettingTableSize, ViewType = viewSetting, FirstTableElement = viewSettingFirstTableElement };

                                exchRule.Format = (string)ruleElem.Attribute("Format");
                                exchRule.Conditions = ruleElem.Attribute("Resolution") == null ? null : new XmlConditionsSetting((string)ruleElem.Attribute("Resolution"));

                                //REPEAT-------------------------
                                var repeat = ruleElem.Element("Repeat");
                                if (repeat != null)
                                {
                                    var count = (string)repeat.Attribute("Count");
                                    if (count != null)
                                    {
                                        int countint;
                                        if (int.TryParse(count, out countint))
                                        {
                                            exchRule.RepeatCount = countint;
                                        }
                                    }

                                    var deltaX = repeat.Element("DeltaX");
                                    if (deltaX != null)
                                    {
                                        int deltaXint;
                                        if (int.TryParse(deltaX.Value, out deltaXint))
                                        {
                                            exchRule.RepeatDeltaX = deltaXint;
                                        }
                                    }

                                    var deltaY = repeat.Element("DeltaY");
                                    if (deltaY != null)
                                    {
                                        int deltaYint;
                                        if (int.TryParse(deltaY.Value, out deltaYint))
                                        {
                                            exchRule.RepeatDeltaX = deltaYint;
                                        }
                                    }
                                }

                                //REQUEST-------------------------
                                var request = ruleElem.Element("Request");
                                if (request != null)
                                {
                                    int maxLenght;
                                    if (int.TryParse((string)request.Attribute("maxLenght"), out maxLenght))
                                    {
                                        exchRule.RequestMaxLenght = maxLenght;
                                    }

                                    exchRule.RequestBody = request.Value.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                                }


                                //RESPONSE-------------------------
                                var response = ruleElem.Element("Response");
                                if (response != null)
                                {
                                    int maxLenght;
                                    if (int.TryParse((string)response.Attribute("maxLenght"), out maxLenght))
                                    {
                                        exchRule.ResponseMaxLenght = maxLenght;
                                    }

                                    int timeResp;
                                    if (int.TryParse((string)response.Attribute("TimeRespone"), out timeResp))
                                    {
                                        exchRule.TimeResponse = timeResp;
                                    }

                                    exchRule.ResponseBody = response.Value.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                                }

                                exchangeRules.Add(exchRule);
                            }
                        }

                        tcpIpSett.SpecialDictionary.Add("ExchangeRules", exchangeRules);
                    }
                }



                listTcpIpSett.Add(tcpIpSett);
            }

            return listTcpIpSett;
        }



        /// <summary>
        /// Создание списка настроек для устройств работающих под ОС Windows.
        /// Использующих WCF для обмена данными
        /// </summary>
        public static List<XmlPcSetting> CreateXmlPcSetting(XElement xml)
        {
            var devPc = xml?.Element("DevicesWithPC")?.Elements("DevicePc").ToList();
            var listPcSett = new List<XmlPcSetting>();


            if (devPc == null || !devPc.Any())
                return listPcSett;


            foreach (var el in devPc)
            {
                var pcSett = new XmlPcSetting(
                                   (string)el.Attribute("Id"),
                                   (string)el.Attribute("Name"),
                                   (string)el.Attribute("Address"),
                                   (string)el.Attribute("TimeRespone"),
                                   (string)el.Attribute("Description"));

                var bind = (string)el.Attribute("Binding");
                if (bind != null)
                {
                    pcSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Conditions");
                if (contrains != null)
                {
                    pcSett.SpecialDictionary.Add("Conditions", new XmlConditionsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    pcSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }

                var countRow = (string)el.Attribute("CountRow");
                if (countRow != null)
                {
                    pcSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
                }

                listPcSett.Add(pcSett);
            }

            return listPcSett;
        }





        /// <summary>
        /// Создание списка настроек для устройств подключенных по Http
        /// </summary>
        public static List<XmlHttpSetting> CreateXmlHttpSetting(XElement xml)
        {
            var devHttp = xml?.Element("DevicesWithHttp")?.Elements("DeviceHttp").ToList();
            var listHttpSett = new List<XmlHttpSetting>();


            if (devHttp == null || !devHttp.Any())
                return listHttpSett;


            foreach (var el in devHttp)
            {
                var httpSett = new XmlHttpSetting(
                                   (string)el.Attribute("Id"),
                                   (string)el.Attribute("Name"),
                                   (string)el.Attribute("Address"),
                                   (string)el.Attribute("Period"),
                                   (string)el.Attribute("TimeRespone"),
                                   (string)el.Attribute("Description"));

                var bind = (string)el.Attribute("Binding");
                if (bind != null)
                {
                    httpSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    httpSett.SpecialDictionary.Add("Contrains", new XmlConditionsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    httpSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }

                var countRow = (string)el.Attribute("CountRow");
                if (countRow != null)
                {
                    httpSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
                }

                var providerType = (string)el.Attribute("ProviderType");
                if (providerType != null)
                {
                    httpSett.SpecialDictionary.Add("ProviderType", new XmlProviderTypeSetting(providerType));
                }





                var headers = (string)el.Attribute("Headers");
                if (headers != null)
                {
                    var pair = headers.Split('+');
                    foreach (var p in pair)
                    {
                        if (!p.Contains(":"))
                            continue;

                        var keyValue = p.Split(':');
                        if (keyValue.Length == 2)
                        {
                            httpSett.Headers.Add(keyValue[0].Trim(), keyValue[1].Trim());
                        }
                    }
                }

                listHttpSett.Add(httpSett);
            }

            return listHttpSett;
        }

        #endregion

    }
}