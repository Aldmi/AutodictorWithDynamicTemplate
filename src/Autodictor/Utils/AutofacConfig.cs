﻿using System.Collections.Generic;
using AutodictorBL.DataAccess;
using AutodictorBL.Services;
using Autofac;
using Autofac.Core;
using DAL.Abstract.Abstract;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;
using DAL.NoSqlLiteDb.Repository;
using DAL.NoSqlLiteDb.Service;
using DAL.Serialize.XML.Reposirory;
using DAL.XmlRaw.Repository;


namespace MainExample.Utils
{
    public class AutofacConfig
    {
        public static IContainer Container { get; private set; }


        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            RegisterType(builder);

            // создаем новый контейнер с теми зависимостями, которые определены выше
            Container = builder.Build();

            // установка сопоставителя зависимостей для WinForms
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }


        /// <summary>
        /// регистрируем сопоставление типов
        /// </summary>
        private static void RegisterType(ContainerBuilder builder)
        {
            builder.RegisterType<XmlRawTrainTypeByRuleRepository>().As<ITrainTypeByRyleRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("folderName", "Config"),
                                                      new NamedParameter("fileName", "DynamicSound.xml") });

            builder.RegisterType<XmlRawPathWaysRepository>().As<IPathwaysRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("folderName", "Config"),
                                                      new NamedParameter("fileName", "PathNames.xml") });

            builder.RegisterType<XmlRawDirectionRepository>().As<IDirectionRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("folderName", "Config"),
                                                      new NamedParameter("fileName", "Stations.xml") });


            builder.RegisterType<NoSqlUsersRepository>().As<IUsersRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("connection", @"NoSqlDb\Users.db") }).InstancePerLifetimeScope();


            builder.RegisterType<ParticirovanieNoSqlRepositoryService<SoundRecordChangesDb>>().As<IParticirovanieService<SoundRecordChangesDb>>()
                .WithParameters(new List<Parameter> { new NamedParameter("baseFileName", @"NoSqlDb\Main_") }).InstancePerLifetimeScope();


            builder.RegisterType<XmlSerializeTableRecRepository>().Keyed<ITrainTableRecRepository>(TrainRecType.LocalMain)
                .WithParameters(new List<Parameter> { new NamedParameter("connection", @"TrainTableMain.xml") });


            builder.RegisterType<XmlSerializeTableRecRepository>().Keyed<ITrainTableRecRepository>(TrainRecType.RemoteCis)
                .WithParameters(new List<Parameter> { new NamedParameter("connection", @"TrainTableRemoteCis.xml") });


            builder.RegisterType<MainForm>().InstancePerDependency();
            builder.RegisterType<AdminForm>().InstancePerDependency();
            builder.RegisterType<AuthenticationForm>().InstancePerDependency();

            builder.RegisterType<AuthenticationService>().As<IAuthentificationService>().SingleInstance();


            //builder.RegisterType<XmlSerializeTableRecRepository>()
            //    .WithParameter(new ResolvedParameter(
            //        (pi, ctx) => pi.ParameterType == typeof(ITrainTableRecRepository),
            //        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("Remote")
            //    ));
        }
    }
}