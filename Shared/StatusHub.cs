using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Hubs
{
    public class StatusHub : Hub
    {
        private readonly IDrone service;

        public StatusHub(IDrone service)
        {
            this.service = service;
        }

        public Task Bounce()
        {
            service.BaseAltitude += 20D;
            return Task.CompletedTask;
        }

        public Task Land()
        {
            service.Land();
            return Task.CompletedTask;
        }
    }
}