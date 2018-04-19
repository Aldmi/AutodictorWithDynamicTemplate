using System;
using CommunicationDevices.DataProviders;
using DAL.Abstract.Entitys;

namespace CommunicationDevices.Services
{
    public class UitPreprocessingChnagePathDirection4Transit : IUitPreprocessing
    {
        public void StartPreprocessing(UniversalInputType uit)
        {
            if (uit.Classification == Classification.Transit) 
            {
                if (uit.ChangeVagonDirection)
                {
                    if (DateTime.Now > uit.TransitTime["приб"] && DateTime.Now < uit.TransitTime["отпр"])// поезд прибыл и еще не отправился, поменяем нумерацию вагонов для отправления
                    {
                        switch (uit.WagonsNumbering)
                        {
                            case WagonsNumbering.Head:
                                uit.WagonsNumbering = WagonsNumbering.Rear;
                                break;
                            case WagonsNumbering.Rear:
                                uit.WagonsNumbering = WagonsNumbering.Head;
                                break;
                        }
                    }
                }
            }
        }
    }
}