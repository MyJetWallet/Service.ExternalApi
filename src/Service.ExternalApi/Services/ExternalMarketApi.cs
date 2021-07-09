using System;
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
        
        public ExternalMarketApi(IExternalMarketManager externalMarketManager, ILogger<ExternalMarketApi> logger)
        {
            _externalMarketManager = externalMarketManager;
            _logger = logger;
        }

        public async Task<GetNameResult> GetNameAsync(GetNameRequest request)
        {
            _logger.LogInformation("GetNameAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ValidateExchangeName(request.ExchangeName);

            var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

            return await exchange.GetNameAsync(request);
        }
        
        public async Task<GetBalancesResponse> GetBalancesAsync(GetBalancesRequest request)
        {
            _logger.LogInformation("GetBalancesAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ValidateExchangeName(request.ExchangeName);

            var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

            return await exchange.GetBalancesAsync(request);
        }

        public async Task<GetMarketInfoResponse> GetMarketInfoAsync(MarketRequest request)
        {
            _logger.LogInformation("GetMarketInfoAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ValidateExchangeName(request.ExchangeName);

            var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

            return await exchange.GetMarketInfoAsync(request);
        }

        public async Task<GetMarketInfoListResponse> GetMarketInfoListAsync(GetMarketInfoListRequest request)
        {
            _logger.LogInformation("GetMarketInfoListAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ValidateExchangeName(request.ExchangeName);

            var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

            return await exchange.GetMarketInfoListAsync(request);
        }

        public async Task<ExchangeTrade> MarketTrade(MarketTradeRequest request)
        {
            _logger.LogInformation("MarketTrade receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ValidateExchangeName(request.ExchangeName);

            var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

            return await exchange.MarketTrade(request);
        }
        
        private void ValidateExchangeName(string requestExchangeName)
        {
            if (!string.IsNullOrWhiteSpace(requestExchangeName)) 
                return;
            
            const string message = "Request without ExchangeName - bad request!";
            _logger.LogError(message);
            throw new Exception(message);
        }
    }
}
