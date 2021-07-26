using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Service.ExternalApi.Services;

namespace Service.ExternalApi.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ExternalApiMetrics>()
                .AsSelf()
                .SingleInstance();
        }
    }
}