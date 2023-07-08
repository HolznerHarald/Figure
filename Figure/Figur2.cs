using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Shapes;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Ink;
using System.Windows.Documents;

namespace Figure
{
    internal class Figur2
    {
        //************ Hauptvariablen
        internal int AnzEck;
        public int[] EckP;
        public bool[] Rund;
        public Gerade2[] FigurGeraden;
        internal int FigInd;
        //************ Nebenvariablen        
        internal NormalKl MinAusdehnungNormal;
        FigContr2 FigC;
        public MainWindow MW;        
        //************ Methoden
        public Figur2(bool[] rundIndex, int[] pP, FigContr2 figurenController,int Find, MainWindow mW)
        {
            FigInd= Find;
            Rund=new bool[rundIndex.Length];
            rundIndex.CopyTo(Rund,0);
            EckP = new int[rundIndex.Length];
            pP.CopyTo(EckP,0);
            FigC = figurenController;
            MW = mW;  
            AnzEck=EckP.Length;
            FigurGeraden = new Gerade2[AnzEck];
            BerechneGeraden();
            MinAusdehnungNormal = BerechneMinAusdehnung();
            //MW.Testliste.Items.Add(FigInd.ToString() + ":" + MinAusdehnungNormal.Normalenabstand.ToString());       
        }
        internal SPsKl SchneideZufGer2(Gerade2 zufger)
        {
            SPsKl SPs = new SPsKl();

            for (int ii = 0; ii < FigurGeraden.Length; ii++)
            {
                if (!Rund[ii])
                {
                    SPs = FigurGeraden[ii].SchneideEcht(zufger, SPs, ii);
                }
                else
                {
                    SPs= FigurGeraden[ii].schneideKreisBogen(zufger, SPs,ii,false);
                }
            }
            return SPs;
        } 
        internal void BerechneGeraden()
        {
            int jj;
            for (int ii = 0; ii < AnzEck; ii++)
            {
                if (ii == AnzEck - 1)
                    jj = 0;
                else
                    jj = ii + 1;
                FigurGeraden[ii] = new Gerade2(FigC.glPoints[EckP[ii]], FigC.glPoints[EckP[jj]], FigC, Rund[ii]);
            }
        }
        private NormalKl BerechneMinAusdehnung()
        {
            NormalKl retNorma;
            NormalKl MinNorma = MaxPunktGeradenAbstand(0);
            for (int ii = 1; ii < FigurGeraden.Length; ii++)
            {
                retNorma = MaxPunktGeradenAbstand(ii);
                if (retNorma.Normalenabstand < MinNorma.Normalenabstand)
                {
                    MinNorma = retNorma;
                }
            }
            return MinNorma;
        }
        internal NormalKl MaxPunktGeradenAbstand(int Geradenindex)
        {
            NormalKl norma;
            NormalKl maxNorma;

            maxNorma = new NormalKl(0, 0, FigC.Radi, FigC.Radi);
            maxNorma.N2 = new Point(0,0);

            if (Rund[Geradenindex])
            {
                maxNorma.Normalenabstand = 2 * FigC.Radi;
                return maxNorma;
            }

            for (int ii = 0; ii < AnzEck; ii++)
            {
                if (ii == Geradenindex || ii == Geradenindex + 1)
                    continue;
                if (Geradenindex + 1 == AnzEck && ii == 0)
                    continue;

                norma = FigurGeraden[Geradenindex].SchneideNormale(FigC.glPoints[EckP[ii]], EckP[ii]);

                //Testl     FigC.ZeichneStrecke(FigurGeraden[Geradenindex].NormalenS1, FigC.glPoints[EckP[ii]], FigC.Farben[ii]);
                if (norma.Normalenabstand > maxNorma.Normalenabstand)
                {
                    maxNorma = norma;
                    maxNorma.Gindex = Geradenindex;
                }
            }

            // Normalabstand zum Kreis größer???
            Point hp1;
            NormalKl MitteNorma = FigurGeraden[Geradenindex].SchneideNormale(new Point(0, 0), -1);
            MitteNorma.Normalenabstand = 0;
            double[] NormalWinkel = new double[2];
            
            if (MitteNorma.N1.X == 0 && MitteNorma.N1.Y == 0)  // Gerade durch (0,0)
            {
                if (FigurGeraden[Geradenindex].P1.X != 0 || FigurGeraden[Geradenindex].P1.Y != 0)
                    hp1 = new Point(FigurGeraden[Geradenindex].P1.Y, -FigurGeraden[Geradenindex].P1.X);
                else
                    hp1 = new Point(FigurGeraden[Geradenindex].P2.Y, -FigurGeraden[Geradenindex].P2.X);
                NormalWinkel[0] = tools.BerechneWinkel(hp1);
            }
            else
                NormalWinkel[0] = tools.BerechneWinkel(MitteNorma.N1);

            NormalWinkel[1] = NormalWinkel[0] + Math.PI;
            if (NormalWinkel[1] > 2*Math.PI)
                NormalWinkel[1] -= 2*Math.PI;

            for (int Wii = 0; Wii <= 1; Wii++)
            {
                for (int ii = 0; ii < AnzEck; ii++)
                {
                    if (FigurGeraden[ii].Bogen &&
                        FigurGeraden[ii].WinkelAufBogen(NormalWinkel[Wii]))
                    {
                            MitteNorma.N2 = new Point(Math.Cos(NormalWinkel[Wii]) * FigC.Radi, Math.Sin(NormalWinkel[Wii]) * FigC.Radi);                        
                            MitteNorma.Normalenabstand = tools.Abstand(MitteNorma.N2, MitteNorma.N1);
                    }
                }
            }
            if (MitteNorma.Normalenabstand > maxNorma.Normalenabstand)
            {
                maxNorma = MitteNorma;
                maxNorma.Gindex = Geradenindex;
            }
            //FigC.ZeichneStrecke(BasisMinAusd, FigC.glPoints[EckP[FigurGeraden[Geradenindex].MaxPunktAbstandIndex]], FigC.Farben[7]);
            //MW.Testliste.Items.Add(Maxretarr[1]);
            return maxNorma;
        }       
        private double BerechneMinWinkel()
        {
            double MinimWinkel = 2 * Math.PI;
            double aktWinkel;
            for (int ii = 0; ii < AnzEck; ii++)
            {
                int Index1 = ii + 1;
                if (Index1 == AnzEck)
                    Index1 = 0;
                aktWinkel = FigurGeraden[Index1].Winkel - FigurGeraden[ii].Winkel;
                if (aktWinkel < 0)
                    aktWinkel += 2 * Math.PI;
                aktWinkel = Math.PI - aktWinkel;
                if (aktWinkel < MinimWinkel)
                    MinimWinkel = aktWinkel;
            }
            return MinimWinkel;
        }
        private double BerechneAusdehnung()
        {
            double maxAbstand = 0;
            double aktAbstand;
            for (int ii = 0; ii < AnzEck; ii++)
                for (int jj = 0; jj < AnzEck; jj++)
                {
                    aktAbstand = tools.Abstand(FigC.glPoints[EckP[ii]], FigC.glPoints[EckP[jj]]);
                    if (aktAbstand > maxAbstand)
                        maxAbstand = aktAbstand;
                }
            return maxAbstand;
        }        
     internal double[] ROLU_ber(Point[] pp,double AlphaWinkel)
        {
            double[] rolu= new double[4];
            if (AnzEck == 1)
            {
                rolu = new double[4] { FigC.Radi, FigC.Radi, -FigC.Radi, -FigC.Radi };
            }
            rolu[0] = pp[EckP[0]].X;
            rolu[1] = pp[EckP[0]].Y;
            rolu[2] = pp[EckP[0]].X;
            rolu[3] = pp[EckP[0]].Y;
            
            for (int ii=0;ii<AnzEck;ii++)   // Wegen Bogen auch 0
            {
                if (pp[EckP[ii]].X > rolu[0])
                    rolu[0] = pp[EckP[ii]].X;
                if (pp[EckP[ii]].Y > rolu[1])
                    rolu[1] = pp[EckP[ii]].Y;
                if (pp[EckP[ii]].X < rolu[2])
                    rolu[2] = pp[EckP[ii]].X;
                if (pp[EckP[ii]].Y < rolu[3])
                    rolu[3] = pp[EckP[ii]].Y;

                if (FigurGeraden[ii].Bogen)
                {
                    if (FigurGeraden[ii].WinkelAufBogen(0 - AlphaWinkel))
                        rolu[0] = FigC.Radi;
                    if (FigurGeraden[ii].WinkelAufBogen(Math.PI / 2 - AlphaWinkel))
                        rolu[1] = FigC.Radi;
                    if (FigurGeraden[ii].WinkelAufBogen(Math.PI - AlphaWinkel))
                        rolu[2] = -FigC.Radi;
                    if (FigurGeraden[ii].WinkelAufBogen(Math.PI * 3 / 2 - AlphaWinkel))
                        rolu[3] = -FigC.Radi;
                }
            }
            return rolu;
        }
        internal void ZeichneFig(Point[] hpp, SolidColorBrush col,double korrFaktor,bool filled)
        {
            /*Test!!!
            int stInd = 11;
            while (hpp[stInd].X != 842 )
            {
                FigC.ZeichneKreuz(hpp[stInd], tools.Farben[0]);
                FigC.ZeichneKreuz(hpp[stInd+1], tools.Farben[1]);
                stInd += 2;
            }*/
            double korrRadi = FigC.Radi * korrFaktor;

            if (AnzEck == 1)
            {
                ZeichneArcSeg(new Point(hpp[0].X - korrRadi, hpp[0].Y), new Point(hpp[0].X + korrRadi, hpp[0].Y), Math.PI);
                ZeichneArcSeg(new Point(hpp[0].X + korrRadi, hpp[0].Y), new Point(hpp[0].X - korrRadi, hpp[0].Y), Math.PI);
            }
            
            var g = new StreamGeometry();
            g.FillRule = FillRule.EvenOdd;

            int hii;
            using (var gc = g.Open())
            {
                if (AnzEck == 1)
                {
                    gc.BeginFigure(new Point(hpp[0].X - korrRadi, hpp[0].Y), filled, true);
                    gc.ArcTo(new Point(hpp[0].X + korrRadi, hpp[0].Y), new Size(korrRadi, korrRadi), 0d,
                                true, SweepDirection.Clockwise, true, false);
                    gc.ArcTo(new Point(hpp[0].X -
                        korrRadi, hpp[0].Y), new Size(korrRadi, korrRadi), 0d,
                                true, SweepDirection.Clockwise, true, false);
                }
                else
                {
                    gc.BeginFigure(hpp[EckP[0]], filled, true);

                    for (int ii = 0; ii < AnzEck; ii++)
                    {
                        hii = ii + 1;
                        if (hii >= AnzEck)
                            hii = 0;

                        if (Rund[ii])
                        {
                            bool bWinkelLarge;
                            if (FigurGeraden[ii].AlphaBogen > Math.PI)
                                bWinkelLarge = true;
                            else
                                bWinkelLarge = false;

                            gc.ArcTo(hpp[EckP[hii]], new Size(korrRadi, korrRadi), 0d,
                                bWinkelLarge, SweepDirection.Clockwise, true, false);
                        }
                        else
                        {
                            gc.LineTo(hpp[EckP[hii]], true, true);
                        }
                    }
                }
                g.Freeze();

                var path = new Path
                {
                    Data = g,
                    Stroke = Brushes.Black,
                    Fill = col,
                    StrokeThickness = 1
                };
                MW.GG1.Children.Add(path);                
            };
           
            /* !!! Kreis auf Canvas zeichnen mit Windows.Shapes , passt nicht ganz überein mit StreamGeometry
            var ellipse1 = new Ellipse();
            ellipse1.Stroke = Brushes.Black;
            ellipse1.StrokeThickness = 1;
            ellipse1.Width = 200;
            ellipse1.Height = 200;          
            MW.CC1.Children.Add(ellipse1);            
            Canvas.SetLeft(ellipse1, 300);
            Canvas.SetTop(ellipse1, 200); */
        }
        private void ZeichneArcSeg(Point point1, Point point2, double alphaBogen)
        {


            ;
        }
    }
}

