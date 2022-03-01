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
                    
                    var orderBookClientFactory = new ExternalMarketClientFactory(externalExchange.Value.OrderBookGrpcUrl);
                    var apiClientFactory = new ExternalMarketClientFactory(externalExchange.Value.ApiGrpcUrl);
                    
                    builder.RegisterInstance(orderBookClientFactory.GetIExternalExchangeManagerGrpc())
                        .As<IExternalExchangeManager>()
                        .SingleInstance();
                    
                    builder.RegisterInstance(apiClientFactory.GetExternalMarketGrpc())
                        .As<IExternalMarket>()
                        .SingleInstance();
                    
                    builder.RegisterInstance(orderBookClientFactory.GetOrderBookSourceGrpc())
                        .As<IOrderBookSource>()
                        .SingleInstance();
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