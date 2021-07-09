using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.ExternalApi.Grpc;

namespace Service.ExternalApi.Client
{
    [UsedImplicitly]
    public class ExternalApiClientFactory: MyGrpcClientFactory
    {
        public ExternalApiClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IHelloService GetHelloService() => CreateGrpcService<IHelloService>();
    }
}
