using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Figure
{
    internal class Gerade2
    {
        //************ Hauptvariablen
        internal Point P1, P2;
        internal bool Bogen;
        //************ Nebenvariablen
        internal bool ParallelY = false;
        internal double dd;
        internal double kk = 0;
        internal double Winkel = Math.PI / 2;  
        internal Point NormalenS1;
        internal double NormalenAbstand;
        internal int MaxPunktAbstandIndex = -1;
        internal FigContr2 FigC;
        internal double AlphaBeginn=0;
        internal double AlphaEnd=0;
        internal double AlphaBogen=0;

        //************ Methoden
        public Gerade2(Point Point1, Point Point2, FigContr2 FC, bool bogen)
        {
            this.P1 = Point1;
            this.P2 = Point2;
            this.Bogen = bogen;
            FigC = FC;
            Init();
            if (Bogen)
            {
                AlphaBeginn = tools.BerechneWinkel(Point1);
                AlphaEnd = tools.BerechneWinkel(Point2);
                AlphaBogen = AlphaEnd - AlphaBeginn;
                if (AlphaBogen <= 0)
                    AlphaBogen += 2 * Math.PI;
            }
        }
        private void Init()
        {
            if (P1.X == P2.X)
            {
                ParallelY = true;
            }
            else
            {
                kk = (P2.Y - P1.Y) / (P2.X - P1.X);
                dd = P1.Y - kk * P1.X;
                if (P2.X - P1.X >= 0)
                {
                    if (P2.Y - P1.Y >= 0)
                        Winkel = Math.Atan(kk);  //1.Qu.
                    else
                        Winkel = 2 * Math.PI + Math.Atan(kk); //4.Qu.
                }
                else
                {
                    if (P2.Y - P1.Y >= 0)
                        Winkel = Math.Atan(kk) + Math.PI;   //2.Qu.
                    else
                        Winkel = Math.PI + Math.Atan(kk); //3.Qu.
                }
            }
        }
        internal SPsKl SchneideEcht(Gerade2 zufallsgerade, SPsKl sPs, int GerIndex)
        {
            Point S1;
            double LageS1;
            if (kk == zufallsgerade.kk)
            {
                if (dd == zufallsgerade.dd)  //liegt aufeinander
                {
                    sPs.AnzS = 100;
                    return sPs;
                }
                else    // Parallel
                    return sPs;
            }
            else if(ParallelY)
            {
                S1.X = P1.X;
                S1.Y = zufallsgerade.kk * S1.X + zufallsgerade.dd;
            }
            else
            {
                S1.X = (dd - zufallsgerade.dd) / (zufallsgerade.kk - kk);
                S1.Y = kk * S1.X + dd;
            }

            if (Math.Abs(P2.X - P1.X) > Math.Abs(P2.Y - P1.Y))
                LageS1 = (S1.X - P1.X) / (P2.X - P1.X);
            else 
                LageS1 = (S1.Y - P1.Y) / (P2.Y - P1.Y);

            if (LageS1 <= 1 && LageS1 >= 0)
            {
                sPs.CheckS(S1, false,GerIndex);
            }
            return sPs;
        }
        internal SPsKl schneideKreisBogen(Gerade2 zufger, SPsKl sPs, int ii, bool FuerNormalenS1)
        {
            // k2*x2 +d2+2*k*d*x +x2 = r2
            //a=k2+1  b=2kd c=d2-r2
            Point[] S12 = new Point[2];
            Point hp;
            double a = zufger.kk * zufger.kk + 1;
            double b = 2 * zufger.dd * zufger.kk;
            double c = zufger.dd * zufger.dd - FigC.Radi * FigC.Radi;
            if (b * b - 4 * a * c > 0)
            {
                S12[0].X = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                S12[1].X = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                S12[0].Y = zufger.kk * S12[0].X + zufger.dd;
                S12[1].Y = zufger.kk * S12[1].X + zufger.dd;
                
                if(FuerNormalenS1) //durch M daher immer 2 Schnittp.
                {
                    SPsKl NormSps= new SPsKl();
                    NormSps.S1 = S12[0];
                    NormSps.S2 = S12[1];
                    return NormSps; 
                }

                double alphx1 = tools.BerechneWinkel(S12[0]);
                double alphx2 = tools.BerechneWinkel(S12[1]);
                if (WinkelAufBogen(alphx1) && WinkelAufBogen(alphx2) &&
                    alphx1 > alphx2)
                {
                    hp = S12[0];
                    S12[0] = S12[1];
                    S12[1] = hp;
                }

                if (WinkelAufBogen(alphx1))
                    sPs.CheckS(S12[0], true, ii);
                if (WinkelAufBogen(alphx2))
                    sPs.CheckS(S12[1], true, ii);

            }
            else if (b * b - 4 * a * c == 0)
            {
                sPs.AnzS = 200;
            }
            return sPs;
        }
        internal NormalKl SchneideNormale(Point point, int glIndex)
        {

            if (ParallelY)
            {
                NormalenS1.X = P1.X;
                NormalenS1.Y = point.Y;
            }
            else if (kk == 0)
            {
                NormalenS1.X = point.X;
                NormalenS1.Y = dd;
            }
            else
            {
                double Normalkk = -1 / kk;
                double Normaldd = point.Y - Normalkk * point.X;
                NormalenS1.X = (dd - Normaldd) / (Normalkk - kk);
                NormalenS1.Y = Normalkk * NormalenS1.X + Normaldd;
            }

            NormalenAbstand = tools.Abstand(point, NormalenS1);
            NormalKl normal = new NormalKl(NormalenAbstand, -1, NormalenS1.X, NormalenS1.Y );
            normal.N2 = point;
            normal.N2Ind = glIndex;

            return normal;
        }
        internal bool WinkelAufBogen(double v)
        {
            if (v < 0)
                v = v + 2 * Math.PI;
            double hwend = AlphaEnd;            
            if (hwend <= 0)
                hwend += 2 * Math.PI;

            if (hwend > AlphaBeginn)
            {
                if (v >= AlphaBeginn && v <= hwend)
                    return true;
                else
                    return false;
            }
            else
            {
                if (v >= AlphaBeginn || v <= hwend)
                    return true;
                else
                    return false;
            }            
        }
    }

}
