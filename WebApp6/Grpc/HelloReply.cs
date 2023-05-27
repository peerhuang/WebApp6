using ProtoBuf;

namespace WebApp6.Grpc
{
    [ProtoContract]
    public class HelloReply
    {
        [ProtoMember(1)]
        public string Message { get; set; }
    }
}