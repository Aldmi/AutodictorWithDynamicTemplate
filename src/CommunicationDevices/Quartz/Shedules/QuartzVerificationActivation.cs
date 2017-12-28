using System;
using CommunicationDevices.Quartz.Jobs;
using CommunicationDevices.Verification;
using Quartz;
using Quartz.Impl;

namespace CommunicationDevices.Quartz.Shedules
{
    public class QuartzVerificationActivation
    {
        public static void Start(VerificationActivation verificationAct)
        {
            //Планировщик
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();


            //---60....80 дней 1 раз в день--------------------------------------------------------------------------------------------------------
            //Заполнение словаря пользовательских данных
            JobDataMap dataMap = new JobDataMap
            {
                ["verificationActAction"] = new Action(verificationAct.CheckActivation_60To80)
            };

            //Создание объекта работы и установка данных для метода Execute
            IJobDetail job = JobBuilder.Create<QuartzJobVerificationActivation>()
                .WithIdentity("Job60To80", "group1")                 //идентификатор работы (по нему можно найти работу)
                .SetJobData(dataMap)
                .Build();

            //Создание первого условия сработки
            ITrigger trigger = TriggerBuilder.Create()                // создаем триггер
                .WithIdentity("trigger60To80", "group1")              // идентифицируем триггер с именем и группой
                .StartAt(DateTimeOffset.Now.AddSeconds(5))            // старт тригера и первый вызов через 5 сек
                .WithSimpleSchedule(x => x                         
                    .WithIntervalInHours(24)
                    .RepeatForever())                             
                .ForJob(job)
                .Build(); // создаем триггер 

            //Связывание объекта работы с тригером внутри планировщика
            scheduler.ScheduleJob(job, trigger);


            //---80....89 дней раз в 8 часов (3 раза в день)--------------------------------------------------------------------------------------------------------
            dataMap = new JobDataMap
            {
                ["verificationActAction"] = new Action(verificationAct.CheckActivation_80To89)
            };

            //Создание объекта работы и установка данных для метода Execute
            job= JobBuilder.Create<QuartzJobVerificationActivation>()
                .WithIdentity("Job80To89", "group1")                
                .SetJobData(dataMap)
                .Build();

            trigger= TriggerBuilder.Create()           
                .WithIdentity("trigger80To89", "group1")      
                .StartAt(DateTimeOffset.Now.AddSeconds(5))      
                .WithSimpleSchedule(x => x                          
                    .WithIntervalInHours(8)
                    .RepeatForever())
                .ForJob(job)
                .Build(); // создаем триггер 

            scheduler.ScheduleJob(job, trigger);


            //---89....90 дней - раз в час--------------------------------------------------------------------------------------------------------
            dataMap = new JobDataMap
            {
                ["verificationActAction"] = new Action(verificationAct.CheckActivation_89To90)
            };

            //Создание объекта работы и установка данных для метода Execute
            job = JobBuilder.Create<QuartzJobVerificationActivation>()
                .WithIdentity("Job89To90", "group1")
                .SetJobData(dataMap)
                .Build();

            trigger = TriggerBuilder.Create()
                .WithIdentity("trigger89To90", "group1")
                .StartAt(DateTimeOffset.Now.AddSeconds(5))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .ForJob(job)
                .Build(); // создаем триггер 

            scheduler.ScheduleJob(job, trigger);


            //запуск планировщика
            scheduler.Start();
        }


        public static void Stop()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            JobKey job = new JobKey("Job60To80", "group1");
            scheduler.DeleteJob(job);

            job = new JobKey("Job80To89", "group1");
            scheduler.DeleteJob(job);

            job = new JobKey("Job89To90", "group1");
            scheduler.DeleteJob(job);
        }


        public static void Shutdown()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown(true);
        }
    }
}