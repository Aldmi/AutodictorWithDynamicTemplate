using System.Collections.Generic;
using AutodictorBL.Builder.SoundRecordCollectionBuilder;
using AutodictorBL.Builder.TrainRecordBuilder;
using AutodictorBL.Services;
using AutodictorBL.Services.AuthenticationServices;
using AutodictorBL.Services.DataAccessServices;
using AutodictorBL.Services.SoundRecordServices;
using AutodictorBL.Services.TrainRecServices;
using Autofac;
using Autofac.Core;
using DAL.Abstract.Abstract;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using DAL.Composite.Repository;
using DAL.InMemory.Repository;
using DAL.NoSqlLiteDb.Entityes;
using DAL.NoSqlLiteDb.Repository;
using DAL.NoSqlLiteDb.Service;
using DAL.Serialize.XML.Reposirory;
using DAL.XmlRaw.Repository;
using MainExample.Services;
using MainExample.Services.GetDataService;
using MainExample.ViewModel.AddingTrainFormVM;
using MainExample.ViewModel.EditRouteFormVM;
using AuthenticationService = AutodictorBL.Services.AuthenticationServices.AuthenticationService;


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

            #region REPOSITORY

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


            //ITrainTableRecRepository -> InMemoryTrainRecRepository with name= "LocalMain"
            builder.RegisterType<XmlSerializeTrainTableRecRepository>().Named<ITrainTableRecRepository>("LocalMain")
                .WithParameters(new List<Parameter> { new NamedParameter("key", @"LocalMain"),
                                                      new NamedParameter("folderName", @"XmlSerialize"),
                                                      new NamedParameter("fileName", @"TrainTableRec.xml")}).InstancePerLifetimeScope();

            //ITrainTableRecRepository -> InMemoryTrainRecRepository with name= "RemoteCis"
            builder.RegisterType<XmlSerializeTrainTableRecRepository>().Named<ITrainTableRecRepository>("RemoteCis")
                .WithParameters(new List<Parameter> { new NamedParameter("key", @"RemoteCis"),
                                                      new NamedParameter("folderName", @"XmlSerialize"),
                                                      new NamedParameter("fileName", @"TrainTableRecRemoteCis.xml")}).InstancePerLifetimeScope();

            builder.RegisterType<XmlSerializeTrainTableRecRepository>().Named<ITrainTableRecRepository>("LocalOperative")
                .WithParameters(new List<Parameter> { new NamedParameter("key", @"LocalOperative"),
                    new NamedParameter("folderName", @"XmlSerialize"),
                    new NamedParameter("fileName", @"TrainTableRecLocalOperative.xml")}).InstancePerLifetimeScope();


            //ITrainTableRecRepository -> CompositerTrainRecRepositoryDecorator with key= TrainRecType.LocalMain
            builder.RegisterType<CompositerTrainRecRepositoryDecorator>().Keyed<ITrainTableRecRepository>(TrainRecRepType.LocalMain)
                .WithParameters(new List<ResolvedParameter> {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "trainTableRecRep")),
                        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("LocalMain")),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTypeByRyleRepository) && (pi.Name == "trainTypeByRyleRep")),
                        (pi, ctx) => ctx.Resolve<ITrainTypeByRyleRepository>())
                }).InstancePerLifetimeScope();

            //ITrainTableRecRepository -> CompositerTrainRecRepositoryDecorator with key= TrainRecType.RemoteCis
            builder.RegisterType<CompositerTrainRecRepositoryDecorator>().Keyed<ITrainTableRecRepository>(TrainRecRepType.RemoteCis)
                .WithParameters(new List<ResolvedParameter> {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "trainTableRecRep")),
                        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("RemoteCis")),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTypeByRyleRepository) && (pi.Name == "trainTypeByRyleRep")),
                        (pi, ctx) => ctx.Resolve<ITrainTypeByRyleRepository>())
                }).InstancePerLifetimeScope();

            //ITrainTableRecRepository -> CompositerTrainRecRepositoryDecorator with key= TrainRecType.LocalOper
            builder.RegisterType<CompositerTrainRecRepositoryDecorator>().Keyed<ITrainTableRecRepository>(TrainRecRepType.LocalOper)
                .WithParameters(new List<ResolvedParameter> {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "trainTableRecRep")),
                        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("LocalOperative")),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTypeByRyleRepository) && (pi.Name == "trainTypeByRyleRep")),
                        (pi, ctx) => ctx.Resolve<ITrainTypeByRyleRepository>())
                }).InstancePerLifetimeScope();

            #endregion



            #region SERVICES

            builder.RegisterType<DirectionService>().SingleInstance();
            builder.RegisterType<PathwaysService>().SingleInstance();
            builder.RegisterType<TrainTypeByRyleService>().SingleInstance(); //TODO: удалить
            builder.RegisterType<UserService>().InstancePerDependency();
            builder.RegisterType<SoundRecChangesService>().InstancePerDependency();
            builder.RegisterType<AuthenticationService>().As<IAuthentificationService>().SingleInstance();
            builder.RegisterType<SoundReсordWorkerService>().As<ISoundReсordWorkerService>().InstancePerLifetimeScope();
            builder.RegisterType<TrainRecBuilderFluent>().As<ITrainRecBuilder>().InstancePerDependency();
            builder.RegisterType<SchedulingPipelineService>().InstancePerLifetimeScope();
            builder.RegisterType<TrainRecService>().WithParameters(new List<ResolvedParameter> {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "repLocalMain")),
                        (pi, ctx) => ctx.ResolveKeyed<ITrainTableRecRepository>(TrainRecRepType.LocalMain)),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "repLocalOperative")),
                        (pi, ctx) => ctx.ResolveKeyed<ITrainTableRecRepository>(TrainRecRepType.LocalOper)),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ITrainTableRecRepository) && (pi.Name == "repRemoteCis")),
                        (pi, ctx) => ctx.ResolveKeyed<ITrainTableRecRepository>(TrainRecRepType.RemoteCis))
                }).SingleInstance();

            builder.RegisterType<TrainRecWorkerService>().As<ITrainRecWorkerService>().SingleInstance();
            builder.RegisterType<SoundRecCollectionBuilderFluent>().As<ISoundRecCollectionBuilder>().InstancePerDependency();
            builder.RegisterType<GetCisRegSh>().SingleInstance();

            #endregion



            #region VIEWMODELS

            builder.RegisterType<AddingTrainFormViewModel>().InstancePerDependency();
            builder.RegisterType<EditListStationFormViewModel>().InstancePerDependency();
            
            #endregion



            #region FORMS

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
            builder.RegisterType<SoundRecordEditForm>().InstancePerDependency();
            builder.RegisterType<ArchiveChangesForm>().InstancePerDependency();
            builder.RegisterType<AddingTrainForm>().InstancePerDependency();
            builder.RegisterType<TechnicalMessageForm>().InstancePerDependency();
            builder.RegisterType<OperativeTableAddItemForm>().InstancePerDependency();
            builder.RegisterType<TrainTableOperativeForm>().InstancePerDependency();
            builder.RegisterType<EditListStationForm>().InstancePerDependency();
            
            #endregion



            //builder.RegisterType<XmlSerializeTableRecRepository>()
            //    .WithParameter(new ResolvedParameter(
            //        (pi, ctx) => pi.ParameterType == typeof(ITrainTableRecRepository),
            //        (pi, ctx) => ctx.ResolveNamed<ITrainTableRecRepository>("Remote")
            //    ));
        }
    }
}