using System;
using System.Collections.Generic;
using System.Linq;

namespace AnomalyDetection.Application.Utilities
{
    public class SlidingWindow
    {
        private readonly int _size;
        private readonly Queue<double> _q = new();

        public SlidingWindow(int size) => _size = size;

        public void Push(double x)
        {
            _q.Enqueue(x);
            if (_q.Count > _size)
                _q.Dequeue();
        }

        public (double Q1, double Q3) GetQuartiles()
        {
            var sorted = _q.OrderBy(v => v).ToArray();
            int n = sorted.Length;
            if (n < 4) return (double.NaN, double.NaN);

            double q1 = Percentile(sorted, 25);
            double q3 = Percentile(sorted, 75);
            return (q1, q3);
        }

        private double Percentile(double[] data, double p)
        {
            double pos = (data.Length + 1) * p / 100.0;
            int idx = (int)pos;
            double frac = pos - idx;
            if (idx <= 0) return data[0];
            if (idx >= data.Length) return data[^1];
            return data[idx - 1] + frac * (data[idx] - data[idx - 1]);
        }
    }
}
