using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using OxyPlot;
using OxyPlot.Series;


using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using PointCollection = Emgu.CV.PointCollection;

using System.Diagnostics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.Statistics;
using Matrix = MathNet.Numerics.LinearAlgebra.Complex.Matrix;
using OxyPlot.Axes;
using OxyPlot.Annotations;

namespace Inclination_angle_App
{
    public partial class Form1 : Form
    {

        double[] Ypoint1;
        double[] Ypoint2;
        double[] Angle_Rad = { 0, 0.3926991, 0.785398, 1.178097, 1.5708, 1.9634954, 2.35619, 2.7488936, 3.14159, 3.5342917, 3.92699, 4.3196899, 4.71239, 5.1050881, 5.49779, 5.8904862 };

        List<double> Ypoint1_list = new List<double>();
        List<double> Ypoint2_list = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Inclination_Calc sine_fit = new Inclination_Calc();
            LineFitting line_fit = new LineFitting();
            Tuple<double, double> points;

            //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(@"C:\Users\Test3\source\repos\PrintedImageBooth\python\precession measurement new baumer location\1.bmp");
            //points = line_fit.fit_Baumer_Image(bmp);

            for (int i = 0; i < 16; i++)
            {
                //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(@"C:/Users/Test3/source/repos/PrintedImageBooth/python/precession_image_" + (i + 1) + ".bmp");
                //points = line_fit.fit_metrology_booth(bmp);

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(@"C:\Users\Test3\source\repos\PrintedImageBooth\python\precession measurement new baumer location\" + (i + 1) + ".bmp");
                points = line_fit.fit_Baumer_Image(bmp);

                Console.WriteLine(points.Item1);
                Console.WriteLine(points.Item2);

                Ypoint1_list.Add(points.Item1);
                Ypoint2_list.Add(points.Item2);
            }

            //double[] Angle = { 0, 22.5, 45, 67.5, 90, 112.5, 135, 157.5, 180, 202.5, 225, 247.5, 270, 292.5, 315, 337.5 };
            //double[] Angle_Rad = { 0, 0.3926991, 0.785398, 1.178097, 1.5708, 1.9634954, 2.35619, 2.7488936, 3.14159, 3.5342917, 3.92699, 4.3196899, 4.71239, 5.1050881, 5.49779, 5.8904862 };
            //double[] Ypoint1 = { 1853.81529, 1847.660185, 1844.26392, 1844.565391, 1850.022335, 1853.779641, 1861.10121, 1869.905686, 1876.988192, 1882.889382, 1886.051181, 1886.038851, 1885.149105, 1876.565114, 1868.736134, 1860.421681 };
            //double[] Ypoint2 = { 1853.248, 1846.372287, 1843.176348, 1843.989568, 1850.517779, 1857.074984, 1867.463348, 1878.007069, 1887.979784, 1895.239886, 1898.598238, 1897.837502, 1894.264412, 1883.825911, 1873.735975, 1862.378465 };



            double inclination_angle;
            Func<double, double> sinewave;
            Func<double, double> sinewave_2;

            Ypoint1 = Ypoint1_list.ToArray();
            Ypoint2 = Ypoint2_list.ToArray();

            Tuple<double, Func<double, double>, Func<double, double>, Func<double, double>> tuple_return = sine_fit.fit_sinewave(Angle_Rad, Ypoint1, Ypoint2);

            //Inclination_angle = tuple_return.Item1;

            var line1 = new OxyPlot.Series.ScatterSeries();
            var line2 = new OxyPlot.Series.ScatterSeries();

            for (int i = 0; i < Angle_Rad.Length; i++)
            {
                line1.Points.Add(new ScatterPoint(Angle_Rad[i], Ypoint1[i]));
                line2.Points.Add(new ScatterPoint(Angle_Rad[i], Ypoint2[i]));
            }

            var myModel = new PlotModel { Title = "Inclination" };
            myModel.Series.Add(line1);
            myModel.Series.Add(line2);

            //myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            myModel.Series.Add(new FunctionSeries(tuple_return.Item2, 0, 2 * Math.PI, 0.1, "lower"));
            myModel.Series.Add(new FunctionSeries(tuple_return.Item3, 0, 2 * Math.PI, 0.1, "upper"));
            // myModel.Series.Add(new FunctionSeries(batFn2, 0, 360, 0.1, "linear"));

            var textAnnotation = new TextAnnotation
            {
                Text = "Angle: " + Math.Round(tuple_return.Item1, 6).ToString(),
                TextPosition = new DataPoint(1, 1890),
                FontSize = 20
            };
            myModel.Annotations.Add(textAnnotation);

            this.plotView1.Model = myModel;


        }

        private void plotView1_Click(object sender, EventArgs e)
        {

        }
    }
}
