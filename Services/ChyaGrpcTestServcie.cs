using ChyaGrpc.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using System.Threading;

namespace ChyaGrpc
{
   public class ChyaGrpcTestServcie : TestService.TestServiceBase
   {
      private readonly ILogger<ChyaGrpcTestServcie> _logger;
      public ChyaGrpcTestServcie(ILogger<ChyaGrpcTestServcie> logger)
      {
         _logger = logger;
      }

      public override Task<EchoOutput> GetEcho(EchoInput request, ServerCallContext context)
      {
         var output = new StringBuilder();
         output.AppendLine("Your input is: " + request.Input);
         output.AppendLine("The server is: " + Environment.MachineName);

         var returnVal = new EchoOutput()
         {
            Output = output.ToString(),
            TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
         };

         return Task.FromResult(returnVal);
      }

      public override Task<TimeOutput> GetDateTime(Empty request, ServerCallContext context)
      {

         var returnVal = new TimeOutput()
         {
            TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
         };

         return Task.FromResult(returnVal);
      }

      public override async Task GetEchoStream(EchoInput request, IServerStreamWriter<EchoOutput> responseStream, ServerCallContext context)
      {
         for (int i = 0; i < 3; i++)
         {
            var returnVal = new EchoOutput()
            {
               Output = request.Input.ToString(),
               TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            _logger.LogInformation("Looping echo stream count " + i);

            await responseStream.WriteAsync(returnVal);

            Thread.Sleep(5000);
         }
      }
   }
}
