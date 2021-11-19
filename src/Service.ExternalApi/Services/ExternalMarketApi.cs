using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Domain.ExternalMarketApi;
using MyJetWallet.Domain.ExternalMarketApi.Dto;
using MyJetWallet.Domain.ExternalMarketApi.Models;
using Newtonsoft.Json;
using Service.ExternalApi.Domain.Services;

namespace Service.ExternalApi.Services
{
    public class ExternalMarketApi: IExternalMarket
    {
        private readonly ILogger<ExternalMarketApi> _logger;
        private readonly IExternalMarketManager _externalMarketManager;
        private readonly ExternalApiMetrics _externalApiMetrics;
        
        public ExternalMarketApi(IExternalMarketManager externalMarketManager,
            ILogger<ExternalMarketApi> logger,
            ExternalApiMetrics externalApiMetrics)
        {
            _externalMarketManager = externalMarketManager;
            _logger = logger;
            _externalApiMetrics = externalApiMetrics;
        }

        public async Task<GetNameResult> GetNameAsync(GetNameRequest request)
        {
            _logger.LogInformation("GetNameAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

                _logger.LogInformation("Exchange: {exchangeJson}", JsonConvert.SerializeObject(exchange));

                var exchangeResponse = await exchange.GetNameAsync(request);

                _logger.LogInformation("Exchange Response: {exchangeResponseJson}",
                    JsonConvert.SerializeObject(exchangeResponse));

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetNameAsync receive exception: {exJson}",
                    JsonConvert.SerializeObject(ex));
                return null;
            }
        }
        
        public async Task<GetBalancesResponse> GetBalancesAsync(GetBalancesRequest request)
        {
            _logger.LogInformation("GetBalancesAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

                _logger.LogInformation("Exchange: {exchangeJson}", JsonConvert.SerializeObject(exchange));

                var exchangeResponse = await exchange.GetBalancesAsync(request);
                exchangeResponse.Balances ??= new List<ExchangeBalance>();

                _logger.LogInformation("Exchange Response Balances count: {count}",
                    exchangeResponse.Balances?.Count);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetBalancesAsync receive exception: {exJson}",
                    JsonConvert.SerializeObject(ex));
                return null;
            }
        }

        public async Task<GetMarketInfoResponse> GetMarketInfoAsync(MarketRequest request)
        {
            _logger.LogInformation("GetMarketInfoAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

                _logger.LogInformation("Exchange: {exchangeJson}", JsonConvert.SerializeObject(exchange));

                var exchangeResponse = await exchange.GetMarketInfoAsync(request);

                _logger.LogInformation("Exchange Response: {exchangeResponseJson}",
                    JsonConvert.SerializeObject(exchangeResponse));

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetMarketInfoAsync receive exception: {exJson}",
                    JsonConvert.SerializeObject(ex));
                return null;
            }
        }

        public async Task<GetMarketInfoListResponse> GetMarketInfoListAsync(GetMarketInfoListRequest request)
        {
            _logger.LogInformation("GetMarketInfoListAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

                _logger.LogInformation("Exchange: {exchangeJson}", JsonConvert.SerializeObject(exchange));

                var exchangeResponse = await exchange.GetMarketInfoListAsync(request);

                _logger.LogInformation("Exchange Response: {exchangeResponseJson}",
                    JsonConvert.SerializeObject(exchangeResponse));

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetMarketInfoListAsync receive exception: {exJson}",
                    JsonConvert.SerializeObject(ex));
                return null;
            }
        }

        public async Task<ExchangeTrade> MarketTrade(MarketTradeRequest request)
        {
            _logger.LogInformation("MarketTrade receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

                _logger.LogInformation("Exchange: {exchangeJson}", JsonConvert.SerializeObject(exchange));
                
                _externalApiMetrics.SetMetrics(request);
                
                var exchangeResponse = await exchange.MarketTrade(request);

                _logger.LogInformation("Exchange Response: {exchangeResponseJson}",
                    JsonConvert.SerializeObject(exchangeResponse));

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("MarketTrade receive exception: {exJson}",
                    JsonConvert.SerializeObject(ex));
                return null;
            }
        }
    }
}
