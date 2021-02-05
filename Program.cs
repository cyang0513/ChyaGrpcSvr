using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ChyaGrpc
{
   public class Program
   {
      static IConfiguration m_Config;

      public static void Main(string[] args)
      {
         //m_Config = new ConfigurationBuilder()
         //            .SetBasePath(Directory.GetCurrentDirectory())
         //            .AddJsonFile("appsettings.json", optional: true)
         //            .AddCommandLine(args)
         //            .Build();

         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
                                       {
                                          webBuilder.UseStartup<Startup>();
                                       });

      //public static IHostBuilder CreateHostBuilder(string[] args) =>
      //    Host.CreateDefaultBuilder(args)
      //        .ConfigureWebHostDefaults(webBuilder =>
      //        {
      //           webBuilder.UseStartup<Startup>().ConfigureKestrel(ConfigKestrel);
      //        });

      private static void ConfigKestrel(KestrelServerOptions kestrelOpt)
      {
         //kestrelOpt.ListenAnyIP(5001, ConfigureListen);
      }

      static void ConfigureListen(ListenOptions listenOpt)
      {
         //var sslPath = m_Config.GetValue<string>("Kestrel:Certificates:Default:Path");
         //var sslPwd = m_Config.GetValue<string>("Kestrel:Certificates:Default:Password");
         //listenOpt.UseHttps(sslPath, sslPwd);
      }
   }
}
