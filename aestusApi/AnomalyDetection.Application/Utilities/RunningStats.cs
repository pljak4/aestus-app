namespace AnomalyDetection.Application.Utilities
{
    public class RunningStats
    {
        private long _n;
        private double _mean;
        private double _M2;

        public void Push(double x)
        {
            _n++;
            var delta = x - _mean;
            _mean += delta / _n;
            _M2 += delta * (x - _mean);
        }

        public double Mean => _mean;
        public double Variance => _n > 1 ? _M2 / (_n - 1) : 0;
        public double StdDev => Math.Sqrt(Variance);
    }
}
