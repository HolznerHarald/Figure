using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Figure
{
    internal class tools
    {
        public static System.Windows.Media.SolidColorBrush[] Farben = new System.Windows.Media.SolidColorBrush[9] { System.Windows.Media.Brushes.Black, System.Windows.Media.Brushes.Red, System.Windows.Media.Brushes.Orange, System.Windows.Media.Brushes.Green, System.Windows.Media.Brushes.Turquoise, System.Windows.Media.Brushes.Blue, System.Windows.Media.Brushes.Violet, System.Windows.Media.Brushes.Gray, System.Windows.Media.Brushes.Brown };
        public static double Abstand(Point p1, Point p2)
        {
            double abstand = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            return abstand;
        }
        public static Point PolarInKart(double[] polarKoo)
        {
            Point point;
            point.X = polarKoo[0] * Math.Cos(polarKoo[1]);
            point.Y = polarKoo[0] * Math.Sin(polarKoo[1]);
            return point;
        }
        public static double BerechneWinkel(Point point)
        {
            double alph;
            
            if (point.X == 0 && point.Y == 0)
                return 0;

            if (point.X >= 0)
            {
                if (point.Y >= 0)
                    alph = Math.Atan(point.Y / point.X);  //1.Qu.
                else
                    alph = 2 * Math.PI + Math.Atan(point.Y / point.X); //4.Qu.
            }
            else
            {
                alph = Math.Atan(point.Y / point.X) + Math.PI;   //2.  3. Qu.

            }
            return alph;
        }
        public static double[] KartInPolar(Point point)
        {
            double[] polK = new double[2] { 0, 0 };

            polK[0] = Math.Sqrt(point.X * point.X + point.Y * point.Y);

            if (point.X == 0)
            {
                if (point.Y >= 0)
                {
                    polK[1] = Math.PI / 2;
                }
                else
                {
                    polK[1] = Math.PI * 3 / 2;
                }
            }
            else
            {
                polK[1] = Math.Atan(point.Y / point.X);
                if (point.X <= 0)
                    polK[1] += Math.PI;
                else if (point.Y <= 0)
                    polK[1] += 2 * Math.PI;
            }
            return polK;
        }
        public static double bestimmeMinY(PointCollection glColPoints)
        {
            double minY = glColPoints[0].Y;
            for (int ii = 0; ii < glColPoints.Count; ii++)
            {
                if (glColPoints[ii].Y < minY)
                    minY = glColPoints[ii].Y;
            }
            return minY;
        }

        public static Point[] Drehe(double alpha, Point[] lpoints)
        {
            double[] polarKoo = new double[2] { 0, 0 };
            Point KartKoo;
            for (int ii = 0; ii < lpoints.Length; ii++)
            {
                polarKoo = tools.KartInPolar(lpoints[ii]);
                polarKoo[1] += alpha;
                if (polarKoo[1] >= 2 * Math.PI)
                    polarKoo[1] -= 2 * Math.PI;
                KartKoo = tools.PolarInKart(polarKoo);
                lpoints[ii] = KartKoo;               
            }
            return lpoints;
        }
        public static Point[] VerGroessern(double faktor, Point[] lpoints)
        {            
            for (int ii = 0; ii < lpoints.Length; ii++)
            {
                lpoints[ii].X = faktor* lpoints[ii].X ;
                lpoints[ii].Y = faktor* lpoints[ii].Y ;               
            }
            return lpoints;
        }
        public static Point[] VerSchiebe(Point vektor, Point[] lpoints)
        {
            Point[] hpoints = new Point[lpoints.Length];
            for (int ii = 0; ii < lpoints.Length; ii++)
            {
                hpoints[ii].X = lpoints[ii].X+vektor.X;
                hpoints[ii].Y = lpoints[ii].Y+vektor.Y;                
            }
            return hpoints;
        }
    }
}
