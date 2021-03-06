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
    public class OrderBookSource : IOrderBookSource
    {
        private readonly ILogger<OrderBookSource> _logger;
        private readonly IOrderBookSourceManager _orderBookSourceManager;

        public OrderBookSource(ILogger<OrderBookSource> logger,
            IOrderBookSourceManager orderBookSourceManager)
        {
            _logger = logger;
            _orderBookSourceManager = orderBookSourceManager;
        }

        public async Task<GetNameResult> GetNameAsync(GetOrderBookNameRequest request)
        {
            _logger.LogInformation("GetNameAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _orderBookSourceManager.GetOrderBookSourceByName(request.ExchangeName);

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

        public async Task<GetSymbolResponse> GetSymbolsAsync(GetSymbolsRequest request)
        {
            _logger.LogInformation("GetSymbolsAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _orderBookSourceManager.GetOrderBookSourceByName(request.ExchangeName);

                _logger.LogInformation("Exchange: {exchangeJson}", JsonConvert.SerializeObject(exchange));

                var exchangeResponse = await exchange.GetSymbolsAsync(request);

                _logger.LogInformation("Exchange Response: {exchangeResponseJson}",
                    JsonConvert.SerializeObject(exchangeResponse));

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetSymbolsAsync receive exception: {exJson}",
                    JsonConvert.SerializeObject(ex));
                return null;
            }
        }

        public async Task<HasSymbolResponse> HasSymbolAsync(MarketRequest request)
        {
            _logger.LogInformation("HasSymbolAsync receive request {requestJson}", JsonConvert.SerializeObject(request));
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _orderBookSourceManager.GetOrderBookSourceByName(request.ExchangeName);

                _logger.LogInformation("Exchange: {exchangeJson}", JsonConvert.SerializeObject(exchange));

                var exchangeResponse = await exchange.HasSymbolAsync(request);

                _logger.LogInformation("Exchange Response: {exchangeResponseJson}",
                    JsonConvert.SerializeObject(exchangeResponse));

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("HasSymbolAsync receive exception: {exJson}",
                    JsonConvert.SerializeObject(ex));
                return null;
            }
        }

        public async Task<GetOrderBookResponse> GetOrderBookAsync(MarketRequest request)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            _logger.LogInformation("GetOrderBookAsync receive request {requestJson}", requestJson);
            
            ProxyHelper.ValidateExchangeName(_logger, request.ExchangeName);

            try
            {
                var exchange = _orderBookSourceManager.GetOrderBookSourceByName(request.ExchangeName);

                var exchangeResponse = await exchange.GetOrderBookAsync(request);
                exchangeResponse.OrderBook.Asks ??= new List<LeOrderBookLevel>();
                exchangeResponse.OrderBook.Bids ??= new List<LeOrderBookLevel>();

                _logger.LogInformation("GetOrderBookAsync exchange {requestJson} response Asks: {count}, Bids: {count2}",
                    requestJson, exchangeResponse.OrderBook?.Asks?.Count, exchangeResponse.OrderBook?.Bids?.Count);

                return exchangeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetOrderBookAsync {requestJson} receive exception: {JsonText}",
                    requestJson, JsonConvert.SerializeObject(ex));
                return null;
            }
        }
    }
}