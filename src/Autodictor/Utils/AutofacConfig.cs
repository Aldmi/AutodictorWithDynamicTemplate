using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
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
                                new NamedParameter("fileName", "DynamicSound.xml") });

            builder.RegisterType<XmlRawDirectionRepository>().As<IDirectionRepository>()
                .WithParameters(new List<Parameter> { new NamedParameter("folderName", "Config"),
                                new NamedParameter("fileName", "DynamicSound.xml") });
        }
    }
}