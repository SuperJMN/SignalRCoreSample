using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Hubs;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:49791/hubs/status")
                .Build();

            hub.On<Status>(Methods.StatusUpdate, status => Console.WriteLine($"Altitude: {status.Altitude:F} m"));

            await hub.StartAsync();
            await hub.ClosedObservable().FirstOrDefaultAsync();
        }   
    }    
}
