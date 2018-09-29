using System;
using System.Reactive.Linq;

namespace Hubs
{
    public class Drone : IDrone
    {
        public Drone()
        {
            var interval = TimeSpan.FromMilliseconds(100);

            var takeOff = Observable.Interval(interval).TakeWhile(h => h < BaseAltitude).Select(t => (double)t);

            var cruise = Observable.Interval(interval)
                .Select(t => 15 * Math.Sin(t * 2 * Math.PI / 180) + BaseAltitude)
                .TakeUntil(_ => IsLanding)
                .Replay(1)
                .RefCount();

            var landing = cruise
                .LastAsync()
                .SelectMany(currentAltitude => Observable.Interval(interval).Select(i => currentAltitude - i))
                .TakeWhile(alt => alt >= 0);

            Altitude = takeOff
                .Concat(cruise)
                .Concat(landing);
        }

        public bool IsLanding { get; set; }
        public double BaseAltitude { get; set; } = 100;
        public IObservable<double> Altitude { get; }
        public void Land()
        {
            IsLanding = true;
        }
    }
}