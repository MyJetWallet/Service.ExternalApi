using System.ServiceModel;
using System.Threading.Tasks;
using Service.ExternalApi.Grpc.Models;

namespace Service.ExternalApi.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}