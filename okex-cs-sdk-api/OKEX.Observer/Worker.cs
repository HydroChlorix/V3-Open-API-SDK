using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OKExSDK;

namespace OKEX.Observer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}

            WebSocketor websocketor = new WebSocketor("wss://real.okex.com:8443/ws/v3?BrokerId=181");

            websocketor.WebSocketPush -= handleWebsocketMessage;
            websocketor.WebSocketPush += handleWebsocketMessage;

            try
            {

                await websocketor.ConnectAsync();

                await websocketor.Subscribe(new List<String>() { _configuration.GetSection("ProductA").Value, _configuration.GetSection("ProductB").Value });
                //await websocketor.Subscribe(new List<String>() { "swap/price_range:BTC-USD-SWAP", "swap/price_range:BTC-USDT-SWAP" });
                //await websocketor.Subscribe(new List<String>() { "swap/ticker:BTC-USD-SWAP", "swap/candle60s:BTC-USD-SWAP" });

                //await websocketor.LoginAsync(apiKey, secret, passPhrase);

                await Task.Delay(500);

                //await websocketor.Subscribe(new List<String>() { "index/ticker:BTC-USD" , "index/ticker:ETH-USD" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Console.WriteLine("Press q to exit");

            //while (!Console.Read().Equals("q")) { }
        }
        private static void handleWebsocketMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
