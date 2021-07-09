using System.Collections.Generic;
using MyJetWallet.Domain.ExternalMarketApi;

namespace Service.ExternalApi.Domain.Services
{
    public interface IExternalMarketManager
    {
        IExternalMarket GetExternalMarketByName(string name);

        List<string> GetMarketNames();
    }
}