using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;

namespace Service.ExternalApi.Client
{
    [UsedImplicitly]
    public class ExternalApiClientFactory: MyGrpcClientFactory
    {
        public ExternalApiClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }
    }
}
