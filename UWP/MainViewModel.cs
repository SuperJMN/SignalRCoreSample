using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using Serilog;

namespace UWP
{
    public class MainViewModel : ReactiveObject
    {
        private readonly ISubject<Status> statusSubject = new BehaviorSubject<Status>(Status.Empty) ;
        private readonly ObservableAsPropertyHelper<double> altitudeObsProperty;
        private bool isConnected;

        public MainViewModel(HubConnection hub)
        {
            hub.On<Status>(Methods.StatusUpdate, status => statusSubject.OnNext(status));

            altitudeObsProperty = statusSubject
                .Select(x => x.Altitude)
                .ObserveOnDispatcher()
                .ToProperty(this, model => model.Altitude);

            BounceCommand = ReactiveCommand.CreateFromTask(() => hub.InvokeAsync(Methods.Bounce));
            LandCommand = ReactiveCommand.CreateFromTask(() => hub.InvokeAsync(Methods.Land));
            ConnectCommand = ReactiveCommand.CreateFromTask(() => hub.StartAsync());
            DisconnectCommand = ReactiveCommand.CreateFromTask(() => hub.StopAsync());
            ConnectCommand
                .Subscribe(_ => IsConnected = true);

            hub.ClosedObservable()
                .ObserveOnDispatcher()
                .Subscribe(_ => IsConnected = false);

            ConnectCommand.ThrownExceptions.Subscribe(exception => Log.Error(exception, "Couldn't connect, yo!"));
        }

        public ReactiveCommand<Unit, Unit> DisconnectCommand { get; }

        public bool IsConnected
        {
            get => isConnected;
            set => this.RaiseAndSetIfChanged(ref isConnected, value);
        }

        public ReactiveCommand<Unit, Unit> ConnectCommand { get; }

        public ReactiveCommand<Unit, Unit> BounceCommand { get; }

        public double Altitude => altitudeObsProperty.Value;

        public ReactiveCommand<Unit, Unit> LandCommand { get; }
    }
}