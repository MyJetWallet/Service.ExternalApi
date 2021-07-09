using Autofac;
using Service.ExternalApi.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.ExternalApi.Client
{
    public static class AutofacHelper
    {
        public static void RegisterExternalApiClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new ExternalApiClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetHelloService()).As<IHelloService>().SingleInstance();
        }
    }
}
