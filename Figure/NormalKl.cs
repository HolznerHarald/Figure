using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Figure
{
    internal class NormalKl
    {
        internal double Normalenabstand;
        internal int Gindex = -1;
        internal Point N1 = new Point();
        internal Point N2 = new Point();
        internal int N1Ind=-1;
        internal int N2Ind=-1;

      public NormalKl(double normalenAbstand, int GInd, double x, double y)
        {
            Normalenabstand = normalenAbstand;
            Gindex= GInd;
            N1.X = x;
            N1.Y = y;
        }
    }
}
