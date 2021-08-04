using System.Threading.Tasks;
using MyJetWallet.Domain.ExternalMarketApi;
using MyJetWallet.Domain.ExternalMarketApi.Dto;
using Service.ExternalApi.Domain.Services;

namespace Service.ExternalApi.Services
{
    public class ExternalExchangeManager : IExternalExchangeManager
    {
        private readonly IExternalMarketManager _externalMarketManager;

        public ExternalExchangeManager(IExternalMarketManager externalMarketManager)
        {
            _externalMarketManager = externalMarketManager;
        }

        public async Task<GetExternalExchangeCollectionResponse> GetExternalExchangeCollectionAsync()
        {
            var response = new GetExternalExchangeCollectionResponse()
            {
                ExchangeNames = _externalMarketManager.GetMarketNames()
            };
            return response;
        }
    }
}