using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using static System.Math;

namespace Server
{
    public class MyService : BackgroundService
    {
        private readonly IHubContext<MyHub> hubContext;

        public MyService(IHubContext<MyHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Observable.Interval(TimeSpan.FromSeconds(0.5)).Subscribe(l =>
            {
                var alt = CalcAltitude(l);
                SendMessage(alt);
            });

            return Task.CompletedTask;
        }

        private void SendMessage(double alt)
        {
            hubContext.Clients.All.SendAsync("SendAction", new Status() { Altitude = alt });
        }

        private double CalcAltitude(long l)
        {
            return 100 * Sin((double)l * PI / 180) + 100;
        }
    }
}