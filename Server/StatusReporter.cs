using System;
using System.Threading;
using System.Threading.Tasks;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Server
{
    public class StatusReporter : BackgroundService
    {
        private readonly IHubContext<StatusHub> hubContext;
        private readonly IDrone drone;

        public StatusReporter(IHubContext<StatusHub> hubContext, IDrone drone)
        {
            this.hubContext = hubContext;
            this.drone = drone;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            drone.Altitude.Subscribe(SendMessage);

            return Task.CompletedTask;
        }

        private void SendMessage(double alt)
        {
            hubContext.Clients.All.SendAsync(Methods.StatusUpdate, new Status { Altitude = alt });
        }
    }
}