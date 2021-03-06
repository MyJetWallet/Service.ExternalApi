using System;
using Grpc.Core.Interceptors;
using MyJetWallet.Domain.ExternalMarketApi.Dto;
using Prometheus;

namespace Service.ExternalApi.Services
{
    public class ExternalApiMetrics
    {
        private static readonly Gauge TradeVolume = Metrics
            .CreateGauge("jet_external_api_trade_volume",
                "Total trade volume.",
                new GaugeConfiguration { LabelNames = new[] { "market", "exchange"} });
        
        
        private static readonly Counter TradeCounter = Metrics
            .CreateCounter("jet_external_api_trade_count",
                "Total trade count.",
                new CounterConfiguration{ LabelNames = new []{ "market", "exchange"}});

        public void SetMetrics(MarketTradeRequest marketTrade)
        {
            TradeCounter
                .WithLabels(marketTrade.Market, marketTrade.ExchangeName)
                .Inc();

            TradeVolume
                .WithLabels(marketTrade.Market, marketTrade.ExchangeName)
                .Inc(Math.Abs(marketTrade.Volume));
        }
    }
}
