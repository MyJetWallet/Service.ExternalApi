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
                    Console.WriteLine($"ENABLED External exchange: {externalExchange.Key}");
                    builder.RegisterExternalMarketClient(externalExchange.Value.GrpcUrl);
                }
                else
                {
                    Console.WriteLine($"DISABLED External exchange: {externalExchange.Key}");
                }
            }
            
            builder
                .RegisterType<ExternalMarketManager>()
                .As<IExternalMarketManager>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();
            
            builder
                .RegisterType<OrderBookSourceManager>()
                .As<IOrderBookSourceManager>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();
        }
    }
}