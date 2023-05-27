using Dapr.Grpc;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
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

        public static Hello ConvertoHello<T>(T value)
        {
            var hello = new Hello();
            hello.Value = Serialize(value);
            return hello;
        }

        public static T UnpackHello<T>(this Any any)
        {
            var hello = any.Unpack<Hello>();
            return Deserialize<T>(hello.Value);
        }

        public static Any PackHello<T>(this Any any, T value)
        {
            using var mem = new MemoryStream();
            Serializer.Serialize(mem, value);
            var hello = new Hello { Value = ByteString.FromStream(mem) };
            return Any.Pack(hello);
        }
    }
}