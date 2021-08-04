using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.Domain.ExternalMarketApi;
using MyJetWallet.Domain.ExternalMarketApi.Dto;
using Newtonsoft.Json;

namespace Service.ExternalApi.Domain.Services
{
    public class OrderBookSourceManager : IOrderBookSourceManager, IStartable
    {
        private readonly Dictionary<string, IOrderBookSource> _orderBookSources = new();

        private readonly IOrderBookSource[] _sources;
        private readonly ILogger<OrderBookSourceManager> _logger;
        private bool _isAllSourcesLoaded = false;

        public OrderBookSourceManager(IOrderBookSource[] sources,
            ILogger<OrderBookSourceManager> logger)
        {
            _sources = sources;
            _logger = logger;
        }

        public IOrderBookSource GetOrderBookSourceByName(string name)
        {
            if (_orderBookSources.TryGetValue(name, out var market))
                return market;

            if (!_isAllSourcesLoaded)
            {
                Start();
                if (_orderBookSources.TryGetValue(name, out market))
                    return market;
            }

            return null;
        }

        public List<string> GetOrderBookSourceNames()
        {
            return _orderBookSources.Keys.ToList();
        }
        
        public void Start()
        {
            _isAllSourcesLoaded = true;
            _orderBookSources.Clear();

            var emptyRequest = new GetOrderBookNameRequest();
            
            foreach (var source in _sources)
            {
                try
                {
                    var name = source.GetNameAsync(emptyRequest).GetAwaiter().GetResult();
                    if (!string.IsNullOrEmpty(name?.Name))
                        _orderBookSources[name.Name] = source;
                }
                catch(Exception ex)
                {
                    _isAllSourcesLoaded = false;
                    _logger.LogError(ex ,"Cannot load one of IOrderBookSource");
                }
            }

            _logger.LogInformation($"Load IOrderBookSource is finished: {JsonConvert.SerializeObject(_orderBookSources.Keys.ToArray())}");
        }
    }
}