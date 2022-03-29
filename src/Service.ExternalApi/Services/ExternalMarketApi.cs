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
            _logger.LogInformation("GetNameAsync receive request {@request}", request);
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);
                
                if (exchange == null)
                {
                    throw new Exception($"Cannot find exchange: {request.ExchangeName}");
                }

                _logger.LogInformation($"Exchange: {request.ExchangeName}");

                var exchangeResponse = await exchange.GetNameAsync(request);

                _logger.LogInformation("GetNameAsync Exchange Response: {@exchangeResponse}", exchangeResponse);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed GetNameAsync: {@request}", request);
                return null;
            }
        }
        
        public async Task<GetBalancesResponse> GetBalancesAsync(GetBalancesRequest request)
        {
            _logger.LogInformation("GetBalancesAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);

                if (exchange == null)
                {
                    throw new Exception($"Cannot find exchange: {request.ExchangeName}");
                }

                _logger.LogInformation($"GetBalancesAsync Exchange:{request.ExchangeName}");

                var exchangeResponse = await exchange.GetBalancesAsync(request);
                exchangeResponse.Balances ??= new List<ExchangeBalance>();

                _logger.LogInformation("GetBalancesAsync Exchange Response: {@response}", exchangeResponse);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed GetBalancesAsync receive exception: {@request}", request);
                
                return new GetBalancesResponse()
                {
                    Balances = new List<ExchangeBalance>()
                };
            }
        }

        public async Task<GetMarketInfoResponse> GetMarketInfoAsync(MarketRequest request)
        {
            _logger.LogInformation("GetMarketInfoAsync receive request {@request}", request);
            
            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);
                
                if (exchange == null)
                {
                    throw new Exception($"Cannot find exchange: {request.ExchangeName}");
                }

                _logger.LogInformation($"GetMarketInfoAsync Exchange: {request.ExchangeName}");

                var exchangeResponse = await exchange.GetMarketInfoAsync(request);

                _logger.LogInformation("GetMarketInfoAsync Exchange Response: {@exchangeResponse}", exchangeResponse);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed GetMarketInfoAsync {@request}", request);
                return null;
            }
        }

        public async Task<GetMarketInfoListResponse> GetMarketInfoListAsync(GetMarketInfoListRequest request)
        {
            _logger.LogInformation("GetMarketInfoListAsync receive request {@request}", request);
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);
                
                if (exchange == null)
                {
                    throw new Exception($"Cannot find exchange: {request.ExchangeName}");
                }

                _logger.LogInformation($"GetMarketInfoListAsync Exchange: {request.ExchangeName}");

                var exchangeResponse = await exchange.GetMarketInfoListAsync(request);

                _logger.LogInformation("GetMarketInfoListAsync ExchangeResponse: {@exchangeResponse}", exchangeResponse);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed GetMarketInfoListAsync {@request}", request);
                return null;
            }
        }

        public async Task<ExchangeTrade> MarketTrade(MarketTradeRequest request)
        {
            _logger.LogInformation("MarketTrade receive request {@request}", request);
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);
                
                if (exchange == null)
                {
                    throw new Exception($"Cannot find exchange: {request.ExchangeName}");
                }

                _logger.LogInformation($"MarketTrade Exchange: {request.ExchangeName}");
                
                _externalApiMetrics.SetMetrics(request);
                
                var exchangeResponse = await exchange.MarketTrade(request);

                _logger.LogInformation("MarketTrade Exchange Response: {@exchangeResponse}", exchangeResponse);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed MarketTrade {@request}", request);
                return null;
            }
        }

        public async Task<GetTradesResponse> GetTradesAsync(GetTradesRequest request)
        {
            _logger.LogInformation("GetTrades receive request {@request}", request);
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _externalMarketManager.GetExternalMarketByName(request.ExchangeName);
                
                if (exchange == null)
                {
                    _logger.LogWarning("Cannot GetTrades. No exchange with name: {exchangeName}", request.ExchangeName);

                    return new GetTradesResponse
                    {
                        ErrorMessage = $"Cannot find exchange: {request.ExchangeName}",
                        IsError = true
                    };
                }

                var exchangeResponse = await exchange.GetTradesAsync(request);

                _logger.LogInformation("GetTrades Exchange Response: {@exchangeResponse}", exchangeResponse);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed MarketTrade {@request}", request);
                return null;
            }
        }
    }
}
