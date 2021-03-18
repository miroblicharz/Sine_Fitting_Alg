using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;
using System.Diagnostics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.Statistics;
using Matrix = MathNet.Numerics.LinearAlgebra.Complex.Matrix;


namespace Inclination_angle_App
{
    class Inclination_Calc
    {


        public Inclination_Calc()
        {

        }

        public Tuple<double, Func<double, double>, Func<double, double>, Func<double, double>> fit_sinewave(double[] Angle_Rad, double[] Ypoint1, double[] Ypoint2)
        {

            var M = Matrix<double>.Build;
            var V = Vector<double>.Build;

            var omega = 1.0d;

            double[] p1 = Fit.LinearCombination(Angle_Rad, Ypoint1,
                x => 1.0,
                x => Math.Sin(x * omega),
                x => Math.Cos(x * omega));

            var a = p1[0];
            var b = SpecialFunctions.Hypotenuse(p1[1], p1[2]);
            var c = Math.Atan2(p1[2], p1[1]);



            double[] p2 = Fit.LinearCombination(Angle_Rad, Ypoint2,
                x => 1.0,
                x => Math.Sin(x * omega),
                x => Math.Cos(x * omega));

            var d = p2[0];
            var e = SpecialFunctions.Hypotenuse(p2[1], p2[2]);
            var f = Math.Atan2(p2[2], p2[1]);

            //double[] p3 = Fit.LinearCombination(Angle_Rad, Ypoint2,
            //    x => 1.0,
            //    x => Math.Sin(x * omega),
            //    x => Math.Cos(x * omega));

            //var g = p2[0];
            //var h = SpecialFunctions.Hypotenuse(p3[1], p3[2]);
            //var i = Math.Atan2(p3[2], p3[1]);

            Console.WriteLine("Radius Lower Plane Circle " + b);

            double ArcSinFuncLowerPlane = Math.Asin(1);
            double XPointAtPeakLower = (ArcSinFuncLowerPlane - c);


            double ArcSinFuncUpperPlane = Math.Asin(1);
            double XPointAtPeakUpper = (ArcSinFuncUpperPlane - f);


            double ThetaR = (XPointAtPeakUpper - XPointAtPeakLower);
            Console.WriteLine("Theta in radians :-" + ThetaR);

            double Rt = b;
            double Rb = e;

            double BTopSec = Rt * Rb * Math.Sin(ThetaR);
            Console.WriteLine("BTopSec value: " + BTopSec);
            double BBottomSec = Math.Sqrt(Math.Pow(Rt, 2) + Math.Pow(Rb, 2) - (2 * Rt * Rb * Math.Cos(ThetaR)));
            Console.WriteLine("BBottomSec value: " + BBottomSec);
            double B = BTopSec / BBottomSec;

            Console.WriteLine("B value: " + B);

            double Alpha_Deg;
            double A;
            double alpha;
            if (ThetaR < Math.PI / 2) //if thetaR less than Pi/2
            {
                A = Math.Sqrt(Math.Pow(Rt, 2) - Math.Pow(B, 2)) - Math.Sqrt(Math.Pow(Rb, 2) - Math.Pow(B, 2));
                Console.WriteLine("A value: " + A);

                alpha = Math.Atan(A / 500);

                Alpha_Deg = (alpha * 180) / Math.PI;
            }
            else
            {
                A = Math.Sqrt(Math.Pow(Rt, 2) - Math.Pow(B, 2)) + Math.Sqrt(Math.Pow(Rb, 2) - Math.Pow(B, 2));
                Console.WriteLine("A value: " + A);

                alpha = Math.Atan(A / 500);

                Alpha_Deg = (alpha * 180) / Math.PI;


            }
            Console.WriteLine("Inclination Angle: " + Math.Abs(Alpha_Deg));

            Func<double, double> sinewave = (x) => a + b * Math.Sin(x + c);
            Func<double, double> sinewave_2 = (x) => d + e * Math.Sin(x + f);
            Func<double, double> sinewave_3 = (x) => d + e * Math.Sin(x + f);

            return Tuple.Create(Math.Abs(Alpha_Deg), sinewave, sinewave_2, sinewave_3);
        }


    }
}
