using System.Collections.Generic;
using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.ExternalApi.Settings
{
    public class SettingsModel
    {
        [YamlProperty("ExternalApi.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("ExternalApi.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("ExternalApi.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
        
        [YamlProperty("ExternalApi.ExternalExchange")]
        public Dictionary<string, ExternalExchange> ExternalExchanges { get; set; }
    }
    
    public class ExternalExchange
    {
        [YamlProperty("IsEnabled")]
        public bool IsEnabled { get; set; }

        [YamlProperty("GrpcUrl")]
        public string GrpcUrl { get; set; }
    }
}
