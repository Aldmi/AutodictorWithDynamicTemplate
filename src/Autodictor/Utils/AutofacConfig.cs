using System.Collections.Generic;
using AutodictorBL.DataAccess;
using AutodictorBL.Services;
using AutodictorBL.Services.SoundRecordServices;
using Autofac;
using Autofac.Core;
using Autofac.Features.OwnedInstances;
using Castle.Core.Logging;
using DAL.Abstract.Abstract;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DAL.Abstract.Entitys.Authentication;
using DAL.Composite.Repository;
using DAL.InMemory.Repository;
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
            var builder = new ContainerBuilder();
            RegisterType(builder);
            Container = builder.Build();
        }


        /// <summary>
        /// регистрируем сопоставление типов
        /// </summary>
        private static void RegisterType(ContainerBuilder builder)
        {
            //РЕПОЗИТОРИИ--------------------------------------------------------------------------------------------
            builder.RegisterType<XmlRawTrainTypeByRuleRepository>().As<ITrainTypeByRyleRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("folderName", "Config"),
                                                      new NamedParameter("fileName", "DynamicSound.xml") }).InstancePerLifetimeScope();

            builder.RegisterType<XmlRawPathWaysRepository>().As<IPathwaysRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("folderName", "Config"),
                                                      new NamedParameter("fileName", "PathNames.xml") }).InstancePerLifetimeScope();

            builder.RegisterType<XmlRawDirectionRepository>().As<IDirectionRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("folderName", "Config"),
                                                      new NamedParameter("fileName", "Stations.xml") }).InstancePerLifetimeScope();

            builder.RegisterType<NoSqlUsersRepository>().As<IUsersRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("connection", @"NoSqlDb\Users.db") }).InstancePerLifetimeScope();

            builder.RegisterType<ParticirovanieNoSqlRepositoryService<SoundRecordChangesDb>>().As<IParticirovanieService<SoundRecordChangesDb>>()
                .WithParameters(new List<Parameter> { new NamedParameter("baseFileName", @"NoSqlDb\Main_") }).InstancePerLifetimeScope();


            //builder.RegisterType<XmlSerializeTableRecRepository>().Keyed<ITrainTableRecRepository>(TrainRecType.LocalMain)
            //    .WithParameters(new List<Parameter> { new NamedParameter("connection", @"TrainTableMain.xml") }).InstancePerLifetimeScope();

            //builder.RegisterType<XmlSerializeTableRecRepository>().Keyed<ITrainTableRecRepository>(TrainRecType.RemoteCis)
            //    .WithParameters(new List<Parameter> { new NamedParameter("connection", @"TrainTableRemoteCis.xml") }).InstancePerLifetimeScope();


            //TEST TrainRecRepository
            //builder.RegisterType<InMemoryTrainRecRepository>().Keyed<ITrainTableRecRepository>(TrainRecType.LocalMain)
            //    .WithParameters(new List<Parameter> { new NamedParameter("key", @"LocalMain") }).InstancePerLifetimeScope();

            //builder.RegisterType<InMemoryTrainRecRepository>().Keyed<ITrainTableRecRepository>(TrainRecType.RemoteCis)
            //    .WithParameters(new List<Parameter> { new NamedParameter("key", @"RemoteCis") }).InstancePerLifetimeScope();

            //ITrainTableRecRepository -> InMemoryTrainRecRepository with name= "LocalMain"
            builder.RegisterType<InMemoryTrainRecRepository>().Named<ITrainTableRecRepository>("LocalMain")
                .WithParameters(new List<Parameter> { new NamedParameter("key", @"LocalMain") }).InstancePerLifetimeScope();

            //ITrainTableRecRepository -> InMemoryTrainRecRepository with name= "RemoteCis"
            builder.RegisterType<InMemoryTrainRecRepository>().Named<ITrainTableRecRepository>("RemoteCis")
                .WithParameters(new List<Parameter> { new NamedParameter("key", @"RemoteCis") }).InstancePerLifetimeScope();


            //ITrainTableRecRepository -> CompositerTrainRecRepositoryDecorator with key= TrainRecType.LocalMain
            builder.RegisterType<CompositerTrainRecRepositoryDecorator>().Keyed<ITrainTableRecRepository>(TrainRecType.LocalMain)
                .WithParameters(new List<ResolvedParameter> {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "trainTableRecRep")),
                        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("LocalMain")),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTypeByRyleRepository) && (pi.Name == "trainTypeByRyleRep")),
                        (pi, ctx) => ctx.Resolve<ITrainTypeByRyleRepository>())
                }).InstancePerLifetimeScope();

            //ITrainTableRecRepository -> CompositerTrainRecRepositoryDecorator with key= TrainRecType.RemoteCis
            builder.RegisterType<CompositerTrainRecRepositoryDecorator>().Keyed<ITrainTableRecRepository>(TrainRecType.RemoteCis)
                .WithParameters(new List<ResolvedParameter> {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "trainTableRecRep")),
                        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("RemoteCis")),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTypeByRyleRepository) && (pi.Name == "trainTypeByRyleRep")),
                        (pi, ctx) => ctx.Resolve<ITrainTypeByRyleRepository>())
                }).InstancePerLifetimeScope();




            //СЕРВИСЫ---------------------------------------------------------------------------------
            builder.RegisterType<DirectionService>().SingleInstance();
            builder.RegisterType<PathwaysService>().SingleInstance();
            builder.RegisterType<TrainTypeByRyleService>().SingleInstance(); //TODO: удалить
            builder.RegisterType<UserService>().InstancePerDependency();
            builder.RegisterType<AuthenticationService>().As<IAuthentificationService>().SingleInstance();
            builder.RegisterType<SoundReсordWorkerService>().As<ISoundReсordWorkerService>().InstancePerLifetimeScope();
            builder.RegisterType<TrainRecService>().WithParameters(new List<ResolvedParameter> {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "repLocalMain")),
                        (pi, ctx) => ctx.ResolveKeyed<ITrainTableRecRepository>(TrainRecType.LocalMain)),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "repRemoteCis")),
                        (pi, ctx) => ctx.ResolveKeyed<ITrainTableRecRepository>(TrainRecType.RemoteCis))
                }).SingleInstance();




            //ФОРМЫ----------------------------------------------------------------------------------
            builder.RegisterType<MainForm>().InstancePerDependency();
            builder.RegisterType<AdminForm>().InstancePerDependency();
            builder.RegisterType<AuthenticationForm>().InstancePerDependency();
            builder.RegisterType<MainWindowForm>().InstancePerDependency();
            builder.RegisterType<StaticDisplayForm>().InstancePerDependency();
            builder.RegisterType<StaticSoundForm>().InstancePerDependency();
            builder.RegisterType<КарточкаСтатическогоЗвуковогоСообщенияForm>().InstancePerDependency();
            builder.RegisterType<TrainTableGridForm>().InstancePerDependency();
            builder.RegisterType<EditTrainTableRecForm>().InstancePerDependency();
            builder.RegisterType<КарточкаДвиженияПоездаForm>().InstancePerDependency();


            //builder.RegisterType<XmlSerializeTableRecRepository>()
            //    .WithParameter(new ResolvedParameter(
            //        (pi, ctx) => pi.ParameterType == typeof(ITrainTableRecRepository),
            //        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("Remote")
            //    ));
        }
    }
}