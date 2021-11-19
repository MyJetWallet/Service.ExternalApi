using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.Domain.ExternalMarketApi;
using MyJetWallet.Domain.ExternalMarketApi.Dto;
using Newtonsoft.Json;

namespace Service.ExternalApi.Domain.Services
{
    public class ExternalMarketManager : IExternalMarketManager, IStartable
    {
        private readonly Dictionary<string, IExternalMarket> _markets = new();

        private readonly IExternalMarket[] _sources;
        private readonly ILogger<ExternalMarketManager> _logger;
        private bool _isAllSourcesLoaded = false;

        public ExternalMarketManager(IExternalMarket[] markets, ILogger<ExternalMarketManager> logger)
        {
            _sources = markets;
            _logger = logger;
        }

        public IExternalMarket GetExternalMarketByName(string name)
        {
            if (_markets.TryGetValue(name, out var market))
                return market;

            if (!_isAllSourcesLoaded)
            {
                Reload(true).GetAwaiter().GetResult();
                if (_markets.TryGetValue(name, out market))
                    return market;
            }

            return null;
        }

        public List<string> GetMarketNames()
        {
            return _markets.Keys.ToList();
        }

        public void Start()
        {
            Reload(false).GetAwaiter().GetResult();
        }

        private async Task Reload(bool reload)
        {
            _logger.LogInformation($"Start load markets [reload = {reload}] ...");
            _isAllSourcesLoaded = true;

            var emptyRequest = new GetNameRequest();
            
            foreach (var source in _sources)
            {
                try
                {
                    var name = await source.GetNameAsync(emptyRequest);
                    if (!string.IsNullOrEmpty(name?.Name))
                        _markets[name.Name] = source;
                    else
                        _isAllSourcesLoaded = false;
                }
                catch(Exception ex)
                {
                    _isAllSourcesLoaded = false;
                    _logger.LogError(ex ,"Cannot load one of ExternalMarket");
                }
            }

            _logger.LogInformation($"Load ExternalMarket is finished: {JsonConvert.SerializeObject(_markets.Keys.ToArray())}");
        }
    }
}