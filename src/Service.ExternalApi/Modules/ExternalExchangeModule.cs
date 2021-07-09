using System;
using Autofac;
using MyJetWallet.Domain.ExternalMarketApi;
using Service.ExternalApi.Domain.Services;

namespace Service.ExternalApi.Modules
{
    public class ExternalExchangeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var externalExchange in Program.Settings.ExternalExchanges)
            {
                if (externalExchange.Value?.IsEnabled == true)
                {
                    Console.WriteLine($"External exchange: {externalExchange.Key}");
                    builder.RegisterExternalMarketClient(externalExchange.Value.GrpcUrl);
                }
            }
            
            builder
                .RegisterType<ExternalMarketManager>()
                .As<IExternalMarketManager>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();
        }
    }
}