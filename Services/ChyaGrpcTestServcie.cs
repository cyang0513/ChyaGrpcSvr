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

      /// <summary>
      /// Get echo of input, unary call
      /// </summary>
      /// <param name="request"></param>
      /// <param name="context"></param>
      /// <returns></returns>
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

      /// <summary>
      /// Get datetime with google protobuf timestamp
      /// </summary>
      /// <param name="request"></param>
      /// <param name="context"></param>
      /// <returns></returns>
      public override Task<TimeOutput> GetDateTime(Empty request, ServerCallContext context)
      {

         var returnVal = new TimeOutput()
         {
            TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
         };

         return Task.FromResult(returnVal);
      }

      /// <summary>
      /// Get echo returned as stream, server streaming call
      /// </summary>
      /// <param name="request"></param>
      /// <param name="responseStream"></param>
      /// <param name="context"></param>
      /// <returns></returns>
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

            //Sleep 3s
            Thread.Sleep(3000);
         }
      }

      /// <summary>
      /// Get input as stream, client streaming call
      /// </summary>
      /// <param name="requestStream"></param>
      /// <param name="context"></param>
      /// <returns></returns>
      public override async Task<EchoOutput> GetInputStream(IAsyncStreamReader<EchoInput> requestStream, ServerCallContext context)
      {
         var result = new EchoOutput();
         var input = new StringBuilder();

         while (await requestStream.MoveNext())
         {
            input.AppendLine(requestStream.Current.Input);
         }

         result.Output = input.ToString();
         result.TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow);

         return await Task.FromResult(result);
      }

      /// <summary>
      /// Get input as stream and return as stream
      /// </summary>
      /// <param name="requestStream"></param>
      /// <param name="responseStream"></param>
      /// <param name="context"></param>
      /// <returns></returns>
      public override async Task GetInputStreamAsServerStream(IAsyncStreamReader<EchoInput> requestStream, IServerStreamWriter<EchoOutput> responseStream, ServerCallContext context)
      {      
         while (await requestStream.MoveNext())
         {
            var returnVal = new EchoOutput()
            {
               Output = requestStream.Current.Input.ToString(),
               TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            await responseStream.WriteAsync(returnVal);

            //Sleep 3s
            Thread.Sleep(3000);
         }
      }
   }
}
