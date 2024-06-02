using System;

namespace LENIA4
{
    internal class DonnutFilter: Fx
    {
        private double Amplitude;
        private double Average;
        private double Shift;
        private double VerticalOffset;

        public DonnutFilter(double kernelSize = 5.0f,double amplitude = 1f, double average = 0.5f, double shift = 0.1f, double verticalOffset = 0)
        {
            Amplitude = amplitude;
            Average = average;
            Shift = shift * kernelSize;
            VerticalOffset = verticalOffset;
        }

        public override double Evaluate(double x)
        {
            return (Amplitude * Math.Exp(-(Math.Pow(x - Average, 2) / (2 * Math.Pow(Shift, 2))))) + VerticalOffset;
        }
    } 
}

