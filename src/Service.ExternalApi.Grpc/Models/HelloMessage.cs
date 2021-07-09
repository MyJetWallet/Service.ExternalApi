using System.Runtime.Serialization;
using Service.ExternalApi.Domain.Models;

namespace Service.ExternalApi.Grpc.Models
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}