using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure.Security.KeyVault.Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace ChyaGrpc
{
   public class Program
   {
      static IConfiguration m_Config;

      public static void Main(string[] args)
      {
         m_Config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddCommandLine(args)
                    .Build();

         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
                                       {
                                          webBuilder.UseStartup<Startup>().ConfigureKestrel(ConfigKestrel);
                                       });

      private static void ConfigKestrel(KestrelServerOptions kestrelOpt)
      {
         kestrelOpt.ListenAnyIP(5001, ConfigureListen);
      }

      static void ConfigureListen(ListenOptions listenOpt)
      {
         //Kestrel need a ssl cert with private key, try to download it as secret.
         //Once pfx is loaded as Certificate, it will generate a secret as well
         var kvClient = new SecretClient(new Uri(m_Config.GetValue<string>("AzKv:Uri")), new DefaultAzureCredential());
         var cert = kvClient.GetSecret(m_Config.GetValue<string>("AzKv:SslSecretName")).Value.Value;

         listenOpt.UseHttps(new X509Certificate2(Convert.FromBase64String(cert)));

      }
   }
}
