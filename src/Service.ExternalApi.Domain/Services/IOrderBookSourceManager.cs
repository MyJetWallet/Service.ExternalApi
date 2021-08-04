using System.Collections.Generic;
using MyJetWallet.Domain.ExternalMarketApi;

namespace Service.ExternalApi.Domain.Services
{
    public interface IOrderBookSourceManager
    {
        IOrderBookSource GetOrderBookSourceByName(string name);

        List<string> GetOrderBookSourceNames();
    }
}