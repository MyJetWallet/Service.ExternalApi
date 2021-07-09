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
    }
}
