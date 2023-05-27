using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using Grpc.Core;

namespace WebApp6.Grpc
{
    public class HelloService : AppCallback.AppCallbackBase
    {
        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            switch (request.Method)
            {
                case "sayhi":
                    var response = new InvokeResponse();
                    //var input = request.Data.Unpack<HelloRequest>();
                    //response.Data = Any.Pack(new HelloReply { Message = "ok" });
                    return response;

                default:

                    return await base.OnInvoke(request, context);
            }
        }
    }
}