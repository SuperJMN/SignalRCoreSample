using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hub = new HubConnectionBuilder().WithUrl("http://localhost:49791/hubs/status")
                .Build();

            hub.On<Status>("SendAction", status => Console.WriteLine($"Altitude: {status.Altitude:F} m"));
            await hub.StartAsync();


            await Observable
                .FromEventPattern<Func<Exception, Task>, Func<Exception, Task>>(
                    h => hub.Closed += h,
                    h => hub.Closed -= h)
                .FirstAsync();        
        }

    }
}
