using System;

namespace Hubs
{
    public interface IDrone
    {
        double BaseAltitude { get; set; }
        IObservable<double> Altitude { get;  }
        void Land();
    }
}