using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ConsoleApp1;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hub = new HubConnectionBuilder().WithUrl("http://localhost:49791/hubs/status")
                .Build();

            hub.On<Status>("SendAction", status => Console.WriteLine($"Altitude: {status.Altitude:F} m"));
            var closedObservable = CreateClosedObservable(hub);

            await hub.StartAsync();
            await closedObservable.FirstOrDefaultAsync();
        }

        private static IObservable<Unit> CreateClosedObservable(HubConnection hub)
        {
            return Observable.Create<Unit>(
                observer =>
                {
                    Task Handler(Exception ex)
                    {
                        observer.OnNext(Unit.Default);
                        return Task.CompletedTask;
                    }

                    hub.Closed += Handler;

                    return () => hub.Closed -= Handler;
                });
        }
    }
}
