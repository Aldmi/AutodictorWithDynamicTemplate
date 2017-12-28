using System;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Services
{
    public class UitPreprocessingChnagePathDirection4Transit : IUitPreprocessing
    {



        public void StartPreprocessing(UniversalInputType uit)
        {
            if (uit.Event == "СТОЯНКА") 
            {
                if (uit.ChangeVagonDirection)
                {
                    if (DateTime.Now > uit.TransitTime["приб"] && DateTime.Now < uit.TransitTime["отпр"])// поезд прибыл и еще не отправился, поменяем нумерацию вагонов для отправления
                    {
                        switch (uit.VagonDirection)
                        {
                            case VagonDirection.FromTheHead:
                                uit.VagonDirection = VagonDirection.FromTheTail;
                                break;
                            case VagonDirection.FromTheTail:
                                uit.VagonDirection = VagonDirection.FromTheHead;
                                break;
                        }
                    }
                }
            }
        }
    }
}