using System;
using Microsoft.Extensions.Logging;

namespace Service.ExternalApi.Services
{
    public class ProxyHelper
    {
        public static void ValidateExchangeName(ILogger logger, string requestExchangeName)
         {
             if (!string.IsNullOrWhiteSpace(requestExchangeName)) 
                 return;
             
             const string message = "Request without ExchangeName - bad request!";
             logger.LogError(message);
             throw new Exception(message);
         }
    }
}