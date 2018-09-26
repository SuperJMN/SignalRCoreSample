using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Hubs
{
    public static class HubMixin
    {
        public static IObservable<Unit> ClosedObservable(this HubConnection hub)
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