using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using CommunicationDevices.DataProviders;


namespace CommunicationDevices.Settings
{
    public class Conditions
    {
        #region prop

        public List<string> TypeTrainNames { get; set; } //типы поездов, ОДИНОЧНЫЕ БЕЗ +
        public List<string> Event { get; set; }        //События (отправление/прибытие/транзит), ОДИНОЧНЫЕ БЕЗ +

        public Command Command { get; set; }                //команда Очистка, Перезагрузка

        public bool LowCurrentTime { get; set; } // Больше Тек. времени
        public bool HightCurrentTime { get; set; } // Меньше Тек. времени

        public Dictionary<string, TimeSpan> DeltaCurrentTime { get; set; }       //+- относительно тек. времени  (ПРИБ- ПРИБ+)  (ОТПР- ОТПР+)  (ТРАНЗИТ- ТРАНЗИТ+)

        public bool PassengerArrival { get; set; } //Пассажирский+ПРИБ.
        public bool PassengerDepart { get; set; } //Пассажирский+ОТПР.
        public IEnumerable<string> PassengerPaths { get; set; } //Пассажирский+ПУТЬ:1,2,3

        public bool SuburbanArrival { get; set; } //Пригородный+ПРИБ.
        public bool SuburbanDepart { get; set; } //Пригородный+ОТПР.
        public IEnumerable<string> SuburbanPaths { get; set; } //Пригородный+ПУТЬ:1,2,3

        public bool CorporateArrival { get; set; } //Фирменный+ПРИБ.
        public bool CorporateDepart { get; set; } //Фирменный+ОТПР.
        public IEnumerable<string> CorporatePaths { get; set; } //Фирменный+ПУТЬ:1,2,3

        public bool ExpressArrival { get; set; } //Скорый+ПРИБ.
        public bool ExpressDepart { get; set; } //Скорый+ОТПР.
        public IEnumerable<string> ExpressPaths { get; set; } //Скорый+ПУТЬ:1,2,3

        public bool HighSpeedArrival { get; set; } //Скоростной+ПРИБ.
        public bool HighSpeedDepart { get; set; } //Скоростной+ОТПР.
        public IEnumerable<string> HighSpeedPaths { get; set; } //Скоростной+ПУТЬ:1,2,3

        public bool SwallowArrival { get; set; } //Ласточка+ПРИБ.
        public bool SwallowDepart { get; set; } //Ласточка+ОТПР.
        public IEnumerable<string> SwallowPaths { get; set; } //Ласточка+ПУТЬ:1,2,3

        public bool RexArrival { get; set; } //РЭКС+ПРИБ.
        public bool RexDepart { get; set; } //РЭКС+ОТПР.
        public IEnumerable<string> RexPaths { get; set; } //РЭКС+ПУТЬ:1,2,3



        public IEnumerable<string> ArrivalPaths { get; set; } //Пути для прибывающего поезда
        public IEnumerable<string> DeparturePaths { get; set; } //Пути для отправляющегося поезда


        public bool EmergencySituationCanceled { get; set; }           //Нешатная ситуация отменен
        public bool EmergencySituationDelayArrival { get; set; }       //Нешатная ситуация, задержка прибытия
        public bool EmergencySituationDelayDepart { get; set; }        //Нешатная ситуация, задержка отправления
        public bool EmergencySituationDispatchOnReadiness { get; set; }//Нешатная ситуация, отправление по говтовности 

        public int? LimitNumberRows { get; set; }                     //Ограничение кол-ва строк

        public List<string> DirectionStations { get; set; }           //Направленния

        public bool SendingDataLimit { get; set; }                    //Ограничние на отправку данных

        #endregion




        #region Methode

        /// <summary>
        /// Проверка ограничения привязки.
        /// </summary>
        public bool CheckContrains(UniversalInputType inData)
        {
            //var typeTrainFilter = true;
            //if (TypeTrainNames != null && TypeTrainNames.Any())
            //{
            //    typeTrainFilter = !TypeTrainNames.Contains(inData.TypeTrain.NameRu);
            //}

            //var eventFilter = true;
            //if (Event != null && Event.Any())
            //{
            //    eventFilter = !Event.Contains(inData.Event);
            //}


            //var emergencySituationCanceledFilter = true;
            //if (EmergencySituationCanceled)
            //{
            //    emergencySituationCanceledFilter = (inData.EmergencySituation & 0x01) == 0x00;
            //}
            //var emergencySituationDelayArrivalFilter = true;
            //if (EmergencySituationDelayArrival)
            //{
            //    emergencySituationDelayArrivalFilter = (inData.EmergencySituation & 0x02) == 0x00;
            //}
            //var emergencySituationDelayDepartFilter = true;
            //if (EmergencySituationDelayDepart)
            //{
            //    emergencySituationDelayDepartFilter = (inData.EmergencySituation & 0x04) == 0x00;
            //}
            //var emergencySituationDispatchOnReadinessFilter = true;
            //if (EmergencySituationDispatchOnReadiness)
            //{
            //    emergencySituationDispatchOnReadinessFilter = (inData.EmergencySituation & 0x08) == 0x00;
            //}




            //var timeFilter = true;
            //if (emergencySituationCanceledFilter && emergencySituationDelayArrivalFilter && emergencySituationDelayDepartFilter && emergencySituationDispatchOnReadinessFilter)
            //{
            //    var time = inData.TransitTime.ContainsKey("отпр") ? inData.TransitTime["отпр"] : inData.Time;
            //    DateTime min;
            //    DateTime max;

            //    if (LowCurrentTime) //"МеньшеТекВремени"
            //    {               
            //        timeFilter = time < DateTime.Now;                    
            //    }

            //    if (HightCurrentTime) //"БольшеТекВремени"
            //    {
            //        timeFilter = time > DateTime.Now;
            //    }

            //    if (DeltaCurrentTime != null && DeltaCurrentTime.ContainsKey("+") && DeltaCurrentTime.ContainsKey("-"))
            //    {
            //         min = DateTime.Now - DeltaCurrentTime["-"];
            //         max = DateTime.Now + DeltaCurrentTime["+"];
            //         timeFilter = (time > min && time < max);
            //    }

            //    if (DeltaCurrentTime != null && DeltaCurrentTime.ContainsKey("ПРИБ+") && DeltaCurrentTime.ContainsKey("ПРИБ-")
            //                                 && DeltaCurrentTime.ContainsKey("ОТПР+") && DeltaCurrentTime.ContainsKey("ОТПР-")
            //                                 && DeltaCurrentTime.ContainsKey("ТРАНЗИТ+") && DeltaCurrentTime.ContainsKey("ТРАНЗИТ-"))
            //    {
            //        switch (inData.Event)
            //        {
            //            case "ПРИБ.":
            //                min = DateTime.Now - DeltaCurrentTime["ПРИБ-"];
            //                max = DateTime.Now + DeltaCurrentTime["ПРИБ+"];
            //                timeFilter = (time > min && time < max);
            //                break;

            //            case "ОТПР.":
            //               min = DateTime.Now - DeltaCurrentTime["ОТПР-"];
            //               max = DateTime.Now + DeltaCurrentTime["ОТПР+"];
            //               timeFilter = (time > min && time < max);
            //               break;

            //            case "СТОЯНКА":
            //                min = DateTime.Now - DeltaCurrentTime["ТРАНЗИТ-"];
            //                max = DateTime.Now + DeltaCurrentTime["ТРАНЗИТ+"];
            //                timeFilter = (time > min && time < max);
            //                break;
            //        }
            //    }
            //}


            //var passengerArrivalfilter = true;
            //if (PassengerArrival)
            //{
            //    passengerArrivalfilter =
            //        !((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.Passenger));
            //}
            //var passengerDepartFilter = true;
            //if (PassengerDepart)
            //{
            //    passengerDepartFilter =
            //        !((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.Passenger));
            //}
            //var passengerPathsFilter = true;
            //if (PassengerPaths != null && PassengerPaths.Any())
            //{
            //    passengerPathsFilter =
            //        !(PassengerPaths.Contains(inData.PathNumber) &&
            //          (inData.TypeTrain == DataProviders.TypeTrain.Passenger));
            //}


            //var suburbanArrivalfilter = true;
            //if (SuburbanArrival)
            //{
            //    suburbanArrivalfilter =
            //        !((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.Suburban));
            //}
            //var suburbanDepartFilter = true;
            //if (SuburbanDepart)
            //{
            //    suburbanDepartFilter =
            //        !((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.Suburban));
            //}
            //var suburbanPathsFilter = true;
            //if (SuburbanPaths != null && SuburbanPaths.Any())
            //{
            //    suburbanPathsFilter =
            //        !(SuburbanPaths.Contains(inData.PathNumber) &&
            //          (inData.TypeTrain == DataProviders.TypeTrain.Suburban));
            //}


            //var corporateArrivalfilter = true;
            //if (CorporateArrival)
            //{
            //    corporateArrivalfilter =
            //        !((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.Corporate));
            //}
            //var corporateDepartFilter = true;
            //if (CorporateDepart)
            //{
            //    corporateDepartFilter =
            //        !((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.Corporate));
            //}
            //var corporatePathsFilter = true;
            //if (CorporatePaths != null && CorporatePaths.Any())
            //{
            //    corporatePathsFilter =
            //        !(CorporatePaths.Contains(inData.PathNumber) &&
            //          (inData.TypeTrain == DataProviders.TypeTrain.Corporate));
            //}


            //var expressArrivalfilter = true;
            //if (ExpressArrival)
            //{
            //    expressArrivalfilter =
            //        !((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.Express));
            //}
            //var expressDepartFilter = true;
            //if (ExpressDepart)
            //{
            //    expressDepartFilter =
            //        !((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.Express));
            //}
            //var expressPathsFilter = true;
            //if (ExpressPaths != null && ExpressPaths.Any())
            //{
            //    expressPathsFilter =
            //        !(ExpressPaths.Contains(inData.PathNumber) && (inData.TypeTrain == DataProviders.TypeTrain.Express));
            //}


            //var highSpeedArrivalfilter = true;
            //if (HighSpeedArrival)
            //{
            //    highSpeedArrivalfilter = !((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.HighSpeed));
            //}
            //var highSpeedDepartFilter = true;
            //if (HighSpeedDepart)
            //{
            //    highSpeedDepartFilter = !((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.HighSpeed));
            //}
            //var highSpeedPathsFilter = true;
            //if (HighSpeedPaths != null && HighSpeedPaths.Any())
            //{
            //    highSpeedPathsFilter =
            //        !(HighSpeedPaths.Contains(inData.PathNumber) &&
            //          (inData.TypeTrain == DataProviders.TypeTrain.HighSpeed));
            //}


            //var swallowArrivalfilter = true;
            //if (SwallowArrival)
            //{
            //    swallowArrivalfilter =!((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.Swallow));
            //}
            //var swallowDepartFilter = true;
            //if (SwallowDepart)
            //{
            //    swallowDepartFilter =!((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.Swallow));
            //}
            //var swallowPathsFilter = true;
            //if (SwallowPaths != null && SwallowPaths.Any())
            //{
            //    swallowPathsFilter =!(SwallowPaths.Contains(inData.PathNumber) && (inData.TypeTrain == DataProviders.TypeTrain.Swallow));
            //}


            //var rexArrivalfilter = true;
            //if (RexArrival)
            //{
            //    rexArrivalfilter = !((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.Rex));
            //}
            //var rexDepartFilter = true;
            //if (RexDepart)
            //{
            //    rexDepartFilter = !((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.Rex));
            //}
            //var rexPathsFilter = true;
            //if (RexPaths != null && RexPaths.Any())
            //{
            //    rexPathsFilter = !(RexPaths.Contains(inData.PathNumber) && (inData.TypeTrain == DataProviders.TypeTrain.Rex));
            //}


            //var arrivalPathsFilter = true;
            //if (ArrivalPaths != null && ArrivalPaths.Any())
            //{
            //    arrivalPathsFilter = !((inData.Event == "ПРИБ.") && ArrivalPaths.Contains(inData.PathNumber));
            //}

            //var departurePathsFilter = true;
            //if (DeparturePaths != null && DeparturePaths.Any())
            //{
            //    departurePathsFilter = !((inData.Event == "ОТПР.") && DeparturePaths.Contains(inData.PathNumber));
            //}

            //var directionStationsFilter = true;
            //if (DirectionStations != null && DirectionStations.Any())
            //{
            //    directionStationsFilter = DirectionStations.Contains(inData.DirectionStation.ToLower());
            //}

            //var sendingDataLimitFilter = true;
            //if (SendingDataLimit)
            //{
            //    sendingDataLimitFilter = inData.SendingDataLimit;
            //}

            //return typeTrainFilter &&
            //       eventFilter &&
            //       timeFilter &&
            //       passengerArrivalfilter &&
            //       passengerDepartFilter &&
            //       passengerPathsFilter &&
            //       suburbanArrivalfilter &&
            //       suburbanDepartFilter &&
            //       suburbanPathsFilter &&
            //       corporateArrivalfilter &&
            //       corporateDepartFilter &&
            //       corporatePathsFilter &&
            //       expressArrivalfilter &&
            //       expressDepartFilter &&
            //       expressPathsFilter &&
            //       highSpeedArrivalfilter &&
            //       highSpeedDepartFilter &&
            //       highSpeedPathsFilter &&
            //       swallowArrivalfilter &&
            //       swallowDepartFilter &&
            //       swallowPathsFilter &&
            //       rexArrivalfilter &&
            //       rexDepartFilter &&
            //       rexPathsFilter &&
            //       arrivalPathsFilter &&
            //       departurePathsFilter &&
            //       sendingDataLimitFilter &&
            //       directionStationsFilter;

            return true;
        }



        /// <summary>
        /// Проверка разрешения.
        /// </summary>
        public bool CheckResolutions(UniversalInputType inData)
        {
            //var typeTrainFilter = true;
            //if (TypeTrain != null && TypeTrain.Any())
            //{
            //    typeTrainFilter = TypeTrain.Contains(inData.TypeTrain);
            //}

            //var eventFilter = true;
            //if (Event != null && Event.Any())
            //{
            //    eventFilter = Event.Contains(inData.Event);
            //}

            //var commandFilter = true;
            //if (Command != Command.None)
            //{
            //    commandFilter = (Command == inData.Command);
            //}


            //var emergencySituationCanceledFilter = true;
            //if (EmergencySituationCanceled)
            //{
            //    emergencySituationCanceledFilter = (inData.EmergencySituation & 0x01) != 0x00;
            //}
            //var emergencySituationDelayArrivalFilter = true;
            //if (EmergencySituationDelayArrival)
            //{
            //    emergencySituationDelayArrivalFilter = (inData.EmergencySituation & 0x02) != 0x00;
            //}
            //var emergencySituationDelayDepartFilter = true;
            //if (EmergencySituationDelayDepart)
            //{
            //    emergencySituationDelayDepartFilter = (inData.EmergencySituation & 0x04) != 0x00;
            //}
            //var emergencySituationDispatchOnReadinessFilter = true;
            //if (EmergencySituationDispatchOnReadiness)
            //{
            //    emergencySituationDispatchOnReadinessFilter = (inData.EmergencySituation & 0x08) != 0x00;
            //}


            //var timeFilter = true;
            //if (LowCurrentTime) //"МеньшеТекВремени"
            //{
            //    timeFilter = inData.Time < DateTime.Now;
            //}
            //if (HightCurrentTime) //"БольшеТекВремени"
            //{
            //    timeFilter = inData.Time > DateTime.Now;
            //}


            //var passengerArrivalfilter = true;
            //if (PassengerArrival)
            //{
            //    passengerArrivalfilter = ((inData.Event == "ПРИБ.") &&
            //                              (inData.TypeTrain == DataProviders.TypeTrain.Passenger));
            //}
            //var passengerDepartFilter = true;
            //if (PassengerDepart)
            //{
            //    passengerDepartFilter = ((inData.Event == "ОТПР.") &&
            //                             (inData.TypeTrain == DataProviders.TypeTrain.Passenger));
            //}
            //var passengerPathsFilter = true;
            //if (PassengerPaths != null && PassengerPaths.Any())
            //{
            //    passengerPathsFilter = (PassengerPaths.Contains(inData.PathNumber) &&
            //                            (inData.TypeTrain == DataProviders.TypeTrain.Passenger));
            //}


            //var suburbanArrivalfilter = true;
            //if (SuburbanArrival)
            //{
            //    suburbanArrivalfilter = ((inData.Event == "ПРИБ.") &&
            //                             (inData.TypeTrain == DataProviders.TypeTrain.Suburban));
            //}
            //var suburbanDepartFilter = true;
            //if (SuburbanDepart)
            //{
            //    suburbanDepartFilter = ((inData.Event == "ОТПР.") &&
            //                            (inData.TypeTrain == DataProviders.TypeTrain.Suburban));
            //}
            //var suburbanPathsFilter = true;
            //if (SuburbanPaths != null && SuburbanPaths.Any())
            //{
            //    suburbanPathsFilter = (SuburbanPaths.Contains(inData.PathNumber) &&
            //                           (inData.TypeTrain == DataProviders.TypeTrain.Suburban));
            //}


            //var corporateArrivalfilter = true;
            //if (CorporateArrival)
            //{
            //    corporateArrivalfilter = ((inData.Event == "ПРИБ.") &&
            //                              (inData.TypeTrain == DataProviders.TypeTrain.Corporate));
            //}
            //var corporateDepartFilter = true;
            //if (CorporateDepart)
            //{
            //    corporateDepartFilter = ((inData.Event == "ОТПР.") &&
            //                             (inData.TypeTrain == DataProviders.TypeTrain.Corporate));
            //}
            //var corporatePathsFilter = true;
            //if (CorporatePaths != null && CorporatePaths.Any())
            //{
            //    corporatePathsFilter = (CorporatePaths.Contains(inData.PathNumber) &&
            //                            (inData.TypeTrain == DataProviders.TypeTrain.Corporate));
            //}


            //var expressArrivalfilter = true;
            //if (ExpressArrival)
            //{
            //    expressArrivalfilter = ((inData.Event == "ПРИБ.") &&
            //                            (inData.TypeTrain == DataProviders.TypeTrain.Express));
            //}
            //var expressDepartFilter = true;
            //if (ExpressDepart)
            //{
            //    expressDepartFilter = ((inData.Event == "ОТПР.") &&
            //                           (inData.TypeTrain == DataProviders.TypeTrain.Express));
            //}
            //var expressPathsFilter = true;
            //if (ExpressPaths != null && ExpressPaths.Any())
            //{
            //    expressPathsFilter = (ExpressPaths.Contains(inData.PathNumber) &&
            //                          (inData.TypeTrain == DataProviders.TypeTrain.Express));
            //}


            //var highSpeedArrivalfilter = true;
            //if (HighSpeedArrival)
            //{
            //    highSpeedArrivalfilter = ((inData.Event == "ПРИБ.") &&
            //                              (inData.TypeTrain == DataProviders.TypeTrain.HighSpeed));
            //}
            //var highSpeedDepartFilter = true;
            //if (HighSpeedDepart)
            //{
            //    highSpeedDepartFilter = ((inData.Event == "ОТПР.") &&
            //                             (inData.TypeTrain == DataProviders.TypeTrain.HighSpeed));
            //}
            //var highSpeedPathsFilter = true;
            //if (HighSpeedPaths != null && HighSpeedPaths.Any())
            //{
            //    highSpeedPathsFilter = (HighSpeedPaths.Contains(inData.PathNumber) &&
            //                            (inData.TypeTrain == DataProviders.TypeTrain.HighSpeed));
            //}


            //var swallowArrivalfilter = true;
            //if (SwallowArrival)
            //{
            //    swallowArrivalfilter = ((inData.Event == "ПРИБ.") &&
            //                            (inData.TypeTrain == DataProviders.TypeTrain.Swallow));
            //}
            //var swallowDepartFilter = true;
            //if (SwallowDepart)
            //{
            //    swallowDepartFilter = ((inData.Event == "ОТПР.") &&
            //                           (inData.TypeTrain == DataProviders.TypeTrain.Swallow));
            //}
            //var swallowPathsFilter = true;
            //if (SwallowPaths != null && SwallowPaths.Any())
            //{
            //    swallowPathsFilter = (SwallowPaths.Contains(inData.PathNumber) &&
            //                          (inData.TypeTrain == DataProviders.TypeTrain.Swallow));
            //}


            //var rexArrivalfilter = true;
            //if (RexArrival)
            //{
            //    rexArrivalfilter = ((inData.Event == "ПРИБ.") && (inData.TypeTrain == DataProviders.TypeTrain.Rex));
            //}
            //var rexDepartFilter = true;
            //if (RexDepart)
            //{
            //    rexDepartFilter = ((inData.Event == "ОТПР.") && (inData.TypeTrain == DataProviders.TypeTrain.Rex));
            //}
            //var rexPathsFilter = true;
            //if (RexPaths != null && RexPaths.Any())
            //{
            //    rexPathsFilter = (RexPaths.Contains(inData.PathNumber) &&
            //                      (inData.TypeTrain == DataProviders.TypeTrain.Rex));
            //}


            //var arrivalPathsFilter = true;
            //if (ArrivalPaths != null && ArrivalPaths.Any())
            //{
            //    arrivalPathsFilter = ((inData.Event == "ПРИБ.") && ArrivalPaths.Contains(inData.PathNumber));
            //}

            //var departurePathsFilter = true;
            //if (DeparturePaths != null && DeparturePaths.Any())
            //{
            //    departurePathsFilter = ((inData.Event == "ОТПР.") && DeparturePaths.Contains(inData.PathNumber));
            //}


            //return typeTrainFilter &&
            //       eventFilter &&
            //       commandFilter &&
            //       timeFilter &&
            //       emergencySituationCanceledFilter &&
            //       emergencySituationDelayArrivalFilter &&
            //       emergencySituationDelayDepartFilter &&
            //       emergencySituationDispatchOnReadinessFilter &&
            //       passengerArrivalfilter &&
            //       passengerDepartFilter &&
            //       passengerPathsFilter &&
            //       suburbanArrivalfilter &&
            //       suburbanDepartFilter &&
            //       suburbanPathsFilter &&
            //       corporateArrivalfilter &&
            //       corporateDepartFilter &&
            //       corporatePathsFilter &&
            //       expressArrivalfilter &&
            //       expressDepartFilter &&
            //       expressPathsFilter &&
            //       highSpeedArrivalfilter &&
            //       highSpeedDepartFilter &&
            //       highSpeedPathsFilter &&
            //       swallowArrivalfilter &&
            //       swallowDepartFilter &&
            //       swallowPathsFilter &&
            //       rexArrivalfilter &&
            //       rexDepartFilter &&
            //       rexPathsFilter &&
            //       arrivalPathsFilter &&
            //       departurePathsFilter;

            return true;
        }

        #endregion
    }

}