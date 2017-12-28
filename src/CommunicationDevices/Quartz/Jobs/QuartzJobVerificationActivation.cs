using System;
using CommunicationDevices.Verification;
using Quartz;

namespace CommunicationDevices.Quartz.Jobs
{
    public class QuartzJobVerificationActivation : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //Получение данных установленных в dataMap для данного ключа job
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var action = dataMap["verificationActAction"] as Action;

            action?.Invoke();
        }
    }
}