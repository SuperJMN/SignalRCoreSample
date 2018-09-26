using System.Reactive.Linq;
using System.Reactive.Subjects;
using Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;

namespace UWP
{
    public class MainViewModel : ReactiveObject
    {
        private readonly ISubject<Status> statusSubject = new BehaviorSubject<Status>(Status.Empty) ;
        private readonly ObservableAsPropertyHelper<double> altitudeObsProperty;

        public MainViewModel(HubConnection hub)
        {
            hub.On<Status>(Methods.StatusUpdate, status => statusSubject.OnNext(status));

            altitudeObsProperty = statusSubject
                .Select(x => x.Altitude)
                .ObserveOnDispatcher()
                .ToProperty(this, model => model.Altitude);
        }

        public double Altitude => altitudeObsProperty.Value;
    }
}