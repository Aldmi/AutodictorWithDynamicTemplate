using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Communication.Interfaces;


namespace CommunicationDevices.DI
{
    public class WindsorConfig : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
              .Register(Component.For<IWindsorContainer>().Instance(container).LifeStyle.Singleton);
        }
    }
}