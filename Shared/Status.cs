namespace Hubs
{
    public class Status
    {
        public double Altitude { get; set; }
        public static Status Empty => new Status() { Altitude = double.NaN };
    }
}