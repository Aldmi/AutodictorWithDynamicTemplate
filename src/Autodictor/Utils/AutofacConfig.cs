using Autofac;
using Domain.Abstract;
using Domain.Concrete.XmlRepository;
using Domain.Entitys;

namespace MainExample.Utils
{
    public class AutofacConfig
    {
        public static IContainer Container { get; private set; }


        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем споставление типов
            builder.RegisterType<RepositoryXmlTrainTypeByRyle>().As<IRepository<TrainTypeByRyle>>();

            // создаем новый контейнер с теми зависимостями, которые определены выше
             Container = builder.Build();

            // установка сопоставителя зависимостей для WinForms
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}