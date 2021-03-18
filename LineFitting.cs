using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;


using System.Windows;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using PointCollection = Emgu.CV.PointCollection;
using System.Drawing;
using System.Diagnostics;

namespace Inclination_angle_App
{
    class LineFitting
    {

        public LineFitting()
        {

        }

        public Tuple<double, double> fit_Baumer_Image(System.Drawing.Bitmap bmp)
        {
            int threshold = 100;

            //System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(@"Z:\User Shares\Miro\Angle_of_Inclination_Calc\Images 16Feb\MV_48_16_114828_P1.bmp");

            Image<Bgr, Byte> output = new Image<Bgr, Byte>(bmp);
            Image<Gray, Byte> canim = new Image<Gray, Byte>(bmp);

            int x = 780;
            int y = 150;
            int width = 390;
            int height = 1240;
            Rectangle roi_rect = new Rectangle(x, y, width, height);


            Mat mask = new Mat();
            Mat fin = new Mat();


            //ImageViewer.Show(output);

            Image<Gray, byte> binaryimage = canim.ThresholdBinary((new Gray(threshold)), (new Gray(255)));

            //Masking

            //Image<Gray, byte> black = new Image<Gray, byte>(binaryimage.Width, binaryimage.Height);
            //CvInvoke.Rectangle(black, roi_rect, new MCvScalar(255, 255, 255), -1);
            //CvInvoke.BitwiseAnd(binaryimage, binaryimage, fin, black);


            //Image<Bgr, Byte> img = mat.ToImage<Bgr, Byte>();
            //Matrix<Byte> matrix = new Matrix<Byte>(mat.Rows, mat.Cols, mat.NumberOfChannels);
            //mat.CopyTo(matrix);

            Mat gaussianBlur = new Mat();
            CvInvoke.GaussianBlur(binaryimage, gaussianBlur, new System.Drawing.Size(21, 21), 3, 3, Emgu.CV.CvEnum.BorderType.Default);
            Mat canny = new Mat();

            CvInvoke.Canny(gaussianBlur, canny, 150, 200);

            System.Drawing.Point offset = new System.Drawing.Point();
            offset.X = x;
            offset.Y = y;

            Mat roi_data = new Mat(canny, roi_rect);
            Mat fit_line = new Mat();


            VectorOfVectorOfPoint contoursDetected = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(roi_data, contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
            //CvInvoke.DrawContours(output, contoursDetected, 0, new MCvScalar(0, 255, 255), 1, LineType.FourConnected, null, 1, offset);

            CvInvoke.FitLine(contoursDetected[0], fit_line, DistType.L2, 0, 0.01, 0.01);

            List<double> x_array = new List<double>();
            List<double> y_array = new List<double>();

            VectorOfPoint biggest_countour = new VectorOfPoint();
            biggest_countour = contoursDetected[0];


            for (int i = 0; i < biggest_countour.Size; i++)
            {
                x_array.Add(biggest_countour[i].X + offset.X);
                y_array.Add(biggest_countour[i].Y + offset.Y);
            }

            double[] xdata = x_array.ToArray();
            double[] ydata = y_array.ToArray();


            Tuple<double, double> p = Fit.Line(xdata, ydata);
            double a = p.Item1;
            double b = p.Item2;


            double X_point_upper = (400 - a) / b;
            double X_point_lower = (1300 - a) / b;


            Console.WriteLine(X_point_upper);
            Console.WriteLine(X_point_lower);

            CvInvoke.Line(output, new System.Drawing.Point((int)X_point_upper, 400), new System.Drawing.Point((int)X_point_lower, 1300), new MCvScalar(0, 0, 255), 2);
            ImageViewer viewer = new ImageViewer(); //create an image viewer
            viewer.Image = output;
            viewer.ShowDialog();

            return Tuple.Create(X_point_upper, X_point_lower);

        }


        public Tuple<double, double, double, double, double, double> fit_metrology_booth(System.Drawing.Bitmap bmp)
        {
            int threshold = 200;



            Image<Bgr, Byte> output = new Image<Bgr, Byte>(bmp);
            Image<Gray, Byte> canim = new Image<Gray, Byte>(bmp);

            int right_x = 380;
            int right_y = 0;
            int right_width = 3390;
            int right_height = 380;
            Rectangle right_roi_rect = new Rectangle(right_x, right_y, right_width, right_height);

            System.Drawing.Point right_offset = new System.Drawing.Point();
            right_offset.X = right_x;
            right_offset.Y = right_y;

            int left_x = 380;
            int left_y = 1700;
            int left_width = 3390;
            int left_height = 380;
            Rectangle left_roi_rect = new Rectangle(left_x, left_y, left_width, left_height);

            System.Drawing.Point left_offset = new System.Drawing.Point();
            left_offset.X = left_x;
            left_offset.Y = left_y;


            int height_x = 0;
            int height_y = 500;
            int height_width = 500;
            int height_height = 1000;
            Rectangle height_roi_rect = new Rectangle(height_x, height_y, height_width, height_height);

            System.Drawing.Point height_offset = new System.Drawing.Point();
            height_offset.X = height_x;
            height_offset.Y = height_y;


            Mat mask = new Mat();
            Mat fin = new Mat();

            ImageViewer viewer = new ImageViewer(); //create an image viewer
            //viewer.Image = output;
            //viewer.ShowDialog();

            Image<Gray, byte> binaryimage = canim.ThresholdBinary((new Gray(threshold)), (new Gray(255)));

            //Masking

            //Image<Gray, byte> black = new Image<Gray, byte>(binaryimage.Width, binaryimage.Height);
            //CvInvoke.Rectangle(black, roi_rect, new MCvScalar(255, 255, 255), -1);
            //CvInvoke.BitwiseAnd(binaryimage, binaryimage, fin, black);


            //Image<Bgr, Byte> img = mat.ToImage<Bgr, Byte>();
            //Matrix<Byte> matrix = new Matrix<Byte>(mat.Rows, mat.Cols, mat.NumberOfChannels);
            //mat.CopyTo(matrix);

            Mat gaussianBlur = new Mat();
            CvInvoke.GaussianBlur(binaryimage, gaussianBlur, new System.Drawing.Size(21, 21), 3, 3, Emgu.CV.CvEnum.BorderType.Default);
            Mat canny = new Mat();

            CvInvoke.Canny(gaussianBlur, canny, 150, 200);


            Mat right_side_ROI = new Mat(canny, right_roi_rect);
            Mat left_side_ROI = new Mat(canny, left_roi_rect);
            Mat height_ROI = new Mat(canny, height_roi_rect);


            Mat fit_line = new Mat();


            VectorOfVectorOfPoint right_contoursDetected = new VectorOfVectorOfPoint();
            VectorOfVectorOfPoint left_contoursDetected = new VectorOfVectorOfPoint();
            VectorOfVectorOfPoint height_contoursDetected = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(right_side_ROI, right_contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
            //CvInvoke.DrawContours(output, right_contoursDetected, 0, new MCvScalar(0, 0, 255), 1, LineType.FourConnected, null, 1, right_offset);

            CvInvoke.FindContours(left_side_ROI, left_contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
            //CvInvoke.DrawContours(output, left_contoursDetected, 0, new MCvScalar(0, 0, 255), 1, LineType.FourConnected, null, 1, left_offset);

            CvInvoke.FindContours(height_ROI, height_contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
            CvInvoke.DrawContours(output, height_contoursDetected, 0, new MCvScalar(0, 0, 255), 1, LineType.FourConnected, null, 3, height_offset);


            //CvInvoke.FitLine(right_contoursDetected[0], fit_line, DistType.L2, 0, 0.01, 0.01);

            VectorOfPoint biggest_countour = new VectorOfPoint();
            double a;
            double b;
            Tuple<double, double> p;
            double[] xdata;
            double[] ydata;

            List<double> right_x_array = new List<double>();
            List<double> right_y_array = new List<double>();

            List<double> left_x_array = new List<double>();
            List<double> left_y_array = new List<double>();

            List<double> height_x_array = new List<double>();
            List<double> height_y_array = new List<double>();

            biggest_countour = right_contoursDetected[0];

            for (int i = 0; i < biggest_countour.Size; i++)
            {
                right_x_array.Add(biggest_countour[i].X + right_offset.X);
                right_y_array.Add(biggest_countour[i].Y + right_offset.Y);
            }


            xdata = right_x_array.ToArray();
            ydata = right_y_array.ToArray();


            p = Fit.Line(xdata, ydata);
            a = p.Item1;
            b = p.Item2;

            double right_point_upper = b * 380 + a;
            double right_point_lower = b * 3770 + a;


            //LEFT SIDE
            biggest_countour = left_contoursDetected[0];

            for (int i = 0; i < biggest_countour.Size; i++)
            {
                left_x_array.Add(biggest_countour[i].X + left_offset.X);
                left_y_array.Add(biggest_countour[i].Y + left_offset.Y);
            }


            xdata = left_x_array.ToArray();
            ydata = left_y_array.ToArray();


            p = Fit.Line(xdata, ydata);
            a = p.Item1;
            b = p.Item2;

            double left_point_upper = b * 380 + a;
            double left_point_lower = b * 3770 + a;

            //HEIGHT
            biggest_countour = height_contoursDetected[0];

            for (int i = 0; i < biggest_countour.Size; i++)
            {
                height_x_array.Add(biggest_countour[i].X + height_offset.X);
                height_y_array.Add(biggest_countour[i].Y + height_offset.Y);
            }


            xdata = height_x_array.ToArray();
            ydata = height_y_array.ToArray();


            p = Fit.Line(xdata, ydata);
            a = p.Item1;
            b = p.Item2;

            double height_point_upper = (500 - a) / b;
            //double height__point_middle = (1536 - a) / b;
            double height_point_lower = (1500 - a) / b;

            Console.WriteLine(height_point_upper);
            Console.WriteLine(height_point_lower);

            //double height_point_upper = 0;
            //////double height__point_middle = (1536 - a) / b;
            //double height_point_lower = 0;


            CvInvoke.Line(output, new System.Drawing.Point(380, (int)right_point_upper), new System.Drawing.Point(3770, (int)right_point_lower), new MCvScalar(0, 0, 255), 2);
            CvInvoke.Line(output, new System.Drawing.Point(380, (int)left_point_upper), new System.Drawing.Point(3770, (int)left_point_lower), new MCvScalar(0, 0, 255), 2);
            CvInvoke.Line(output, new System.Drawing.Point((int)height_point_upper, 500), new System.Drawing.Point((int)height_point_lower, 1500), new MCvScalar(0, 0, 255), 2);

            //viewer.Image = output;
            //viewer.ShowDialog();


            return Tuple.Create(right_point_upper, right_point_lower, left_point_upper, left_point_lower, height_point_upper, height_point_lower);
        }



    }
}
