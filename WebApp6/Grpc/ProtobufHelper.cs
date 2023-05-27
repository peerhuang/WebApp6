using Google.Protobuf;
using ProtoBuf;

namespace WebApp6.Grpc
{
    public static class ProtobufHelper
    {
        public static ByteString Serialize<T>(T value)
        {
            using var mem = new MemoryStream();
            Serializer.Serialize(mem, value);
            return ByteString.FromStream(mem);
        }

        public static T Deserialize<T>(ByteString byteString)
        {
            using var mem = new MemoryStream(byteString.ToArray());
            return Serializer.Deserialize<T>(mem);
        }
    }
}