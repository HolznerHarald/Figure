using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Figure
{
    class FigContr2
    {
        //************ Hauptvariablen
        public Point[] glPoints;
        public Figur2[] Figuren;
        internal bool[] FigTeile;
        internal bool FigFormRund;
        //Regelungswerte
        internal double Radi = 400; 
        internal double RadiFaktor = 2; 
        internal double MinAusd = 0.166;
        internal int MinBreiteFaktor = 6;
        internal double Spalt = 80;
        internal double fixerFaktorAltuelleBildschirmGroesse;
        internal int StartfigNr;
        //************ Nebenvariablen
        private MainWindow MW;  
        internal int AnzTeile;
        public Gerade2 zufger;
        internal double[] Alph;
        public string testteile;
        private LetzteAufgabe lA;

        //************ Methoden
        public FigContr2(MainWindow mW,bool figForm, int figNr, int Teilanzahl, double[] alph)
        {
            FigFormRund=figForm;
            Figuren = new Figur2[20];
            double StartWinkel;
            this.MW = mW;
            this.StartfigNr = figNr;
            glPoints = new Point[30];
            for (int ii = 0; ii < 30; ii++) { glPoints[ii].X = 0; glPoints[ii].Y = 0; }
            
            if(FigFormRund)
            {            
                StartWinkel = figNr * Math.PI / 2;

                if (figNr == 0)
                {
                    Figuren[0] = new Figur2(new bool[] { true }, new int[] { 0 }, this, 0, MW);
                }
                else if (figNr == 2)
                {
                    glPoints[0].X = Radi;
                    glPoints[0].Y = 0;
                    glPoints[1].X = -Radi;
                    glPoints[1].Y = 0;
                    Figuren[0] = new Figur2(new bool[] { true, false }, new int[] { 0, 1 }, this, 0, MW);
                }
                else
                {
                    glPoints[2].X = Radi * Math.Cos(StartWinkel);
                    glPoints[2].Y = Radi * Math.Sin(StartWinkel);
                    glPoints[1].X = Radi;
                    glPoints[1].Y = 0;
                    Figuren[0] = new Figur2(new bool[] { false, true, false }, new int[] { 0, 1, 2 }, this, 0, MW);
                }
            }
            else//Eckig, Fignr = Anzahl der Ecken
            {
                int AnzEcken = figNr + 5;
                StartWinkel = Math.PI / AnzEcken + Math.PI/2;
                double FigWinkel = 2 * Math.PI / AnzEcken;
                bool[] brund = new bool[AnzEcken];
                int[] heck = new int[AnzEcken];
                for (int ii=0;ii < AnzEcken; ii++)
                {
                    brund[ii] = false;
                    heck[ii] = ii;
                    glPoints[ii].X = Math.Cos(StartWinkel+ii*FigWinkel)*Radi;
                    glPoints[ii].Y = Math.Sin(StartWinkel + ii * FigWinkel) * Radi; 
                }
                Figuren[0] = new Figur2(brund, heck, this, 0, MW);

            }
            FigTeile = new bool[20];
            for (int ii = 0; ii < FigTeile.Count(); ii++)
                FigTeile[ii] = false;
            FigTeile[0] = true;

            AnzTeile = Teilanzahl;
            Alph = alph;
        }

        public FigContr2(MainWindow mW, LetzteAufgabe lA)
        {
            MW = mW;
            this.lA = lA;
        }

        internal bool Starte(bool Wiederholung)
        {         
            testteile = "";

            MW.Testliste.Items.Add("Anzahlteile:" + AnzTeile.ToString());
            
            if (AnzTeile > 1 && !TeileIn2(0, 1, 11, Wiederholung))
                return false;
            if (AnzTeile > 2 && !TeileIn2(1, 3, 13, Wiederholung))
                return false;

            if (AnzTeile > 3 && !TeileIn2(2, 5, 15, Wiederholung))
                return false;

            int LaufIndex = 0;
            for (int ii = 0; ii < AnzTeile - 4; ii++)
            {
                int Mindex = bestimmeMaxTeil2();
                if (!TeileIn2(Mindex, 7 + LaufIndex, 17 + LaufIndex, Wiederholung))
                    return false;
                LaufIndex += 2;
            }
            string hs = "";
            //Test!!!
            for (int ii = 0; ii < LaufIndex + 7; ii++)
                if (FigTeile[ii] == true)
                    hs = hs + " " + ii.ToString();
            //MW.Testliste.Items.Add(hs);
            return true;
        }
        internal void ZeichneAlleTeileMitte(double Faktor,double verx,double very,bool filled)
        {
            int FarbenInd = 0;
            Point[] hpp = tools.VerGroessern(Faktor, glPoints);
            hpp = tools.VerSchiebe(new Point(verx,very), hpp);

            for (int ii = 0; ii < FigTeile.Count(); ii++)
                if (FigTeile[ii] == true)
                {
                    Figuren[ii].ZeichneFig(hpp, tools.Farben[FarbenInd], Faktor, filled);
                    FarbenInd++;
                }

        }

        internal double ZeichneAufgabe(bool verschiedeneFarben,bool filled)
        {
            double[] Figurenbreite = new double[Figuren.Length];
            double[] FigurenMitteRL = new double[Figuren.Length];
            double[] FigurenMitteOU = new double[Figuren.Length];

            Point[] hpp;
            int GezeichneteFigNr = 0;
            double Gesamtbreite = 0;
                      
            for (int ii = 0; ii < FigTeile.Count(); ii++)
            {
                if (FigTeile[ii] == false)
                {
                    continue;
                }
                
                hpp = dreheWinkel(glPoints, Alph[GezeichneteFigNr], 1);
                double[] rolu = Figuren[ii].ROLU_ber(hpp,Alph[GezeichneteFigNr]);
                Figurenbreite[ii] = rolu[0] - rolu[2];
                FigurenMitteRL[ii] = (rolu[0] + rolu[2])/2;
                FigurenMitteOU[ii] = (rolu[1] + rolu[3])/2;
                Gesamtbreite = Gesamtbreite + Figurenbreite[ii];
                GezeichneteFigNr++;
            }
            Gesamtbreite= Gesamtbreite + (AnzTeile + 1) * Spalt;
            double Faktor = MW.GG1.ActualWidth / Gesamtbreite;
            if (StartfigNr  == 1 && FigFormRund)
                RadiFaktor = 1;
            double Gesamthoehe = RadiFaktor * Radi;
            var Faktorh= MW.GG1.ActualHeight / (2*Gesamthoehe+2*Spalt);
            Faktor = Math.Min(Faktor, Faktorh);

            //Für Alle Fig.Teile , Koordinaten drehen, mal Faktor, 
            double XLinks = Spalt;
            GezeichneteFigNr = 0;
            for (int ii = 0; ii < FigTeile.Count(); ii++)
            {
                if (FigTeile[ii] == false)
                {
                    continue;
                }
                double VerschiebeX = XLinks - FigurenMitteRL[ii] + Figurenbreite[ii]/2;
                VerschiebeX = VerschiebeX * Faktor;
                double VerschiebeY = MW.GG1.ActualHeight/4 - FigurenMitteOU[ii] * Faktor;
                XLinks = XLinks + Figurenbreite[ii] + Spalt;
                
                hpp = dreheWinkel(glPoints, Alph[GezeichneteFigNr],Faktor);

                hpp = tools.VerSchiebe(new Point(VerschiebeX, VerschiebeY), hpp);
                if(verschiedeneFarben) 
                    Figuren[ii].ZeichneFig(hpp, tools.Farben[GezeichneteFigNr],Faktor, filled);
                else
                    Figuren[ii].ZeichneFig(hpp, tools.Farben[0], Faktor, filled);

                GezeichneteFigNr++;
            }
            return Faktor;
        }

        public Point[] dreheWinkel(Point[] pp, double Winkel,double Faktor)
        {
            double[] polarKoo = new double[2] { 0, 0 };
            Point KartKoo;
            Point[] hpp = new Point[pp.Length];

            for (int jj = 0; jj < pp.Length; jj++)
            {
                polarKoo = tools.KartInPolar(pp[jj]);
                polarKoo[1] += Winkel;
                if (polarKoo[1] >= 2 * Math.PI)
                    polarKoo[1] -= 2 * Math.PI;
                KartKoo = tools.PolarInKart(polarKoo);
                KartKoo.X = KartKoo.X * Faktor;
                KartKoo.Y = KartKoo.Y * Faktor;
                hpp[jj] = KartKoo;
            }
            return hpp;
        }
        private bool TeileIn2(int FigInd, int NeueFigSpeicherInd, int S1Ind, bool wiederhol)
        {
            testteile = testteile + "T:" + FigInd.ToString() + NeueFigSpeicherInd.ToString();
            //MW.Testliste.Items.Add(testteile);
            SPsKl SPS;
            Figur2[] TestFiguren;
            int Zae = 0;
            bool passt = false;
            do
            {
                if (wiederhol)
                    zufger = new Gerade2(MW.LA.letzteeGlP[S1Ind], MW.LA.letzteeGlP[S1Ind + 1],this,false);
                else
                    Zufallsgerade2(FigInd);
                Zae++;
                if (Zae > 1000)
                {
                    MW.Testliste.Items.Add("!!!!!Grösser 1000");
                    return false;
                }

                SPS = Figuren[FigInd].SchneideZufGer2(zufger);
                if (SPS.AnzS != 2)
                    continue;

                glPoints[S1Ind] = SPS.S1;
                glPoints[S1Ind + 1] = SPS.S2;
                TestFiguren = Erstelle2Teile(FigInd, S1Ind, SPS, NeueFigSpeicherInd);

                if (TestFiguren[0].MinAusdehnungNormal.Normalenabstand < Figuren[0].MinAusdehnungNormal.Normalenabstand / MinBreiteFaktor ||
                    TestFiguren[1].MinAusdehnungNormal.Normalenabstand < Figuren[0].MinAusdehnungNormal.Normalenabstand / MinBreiteFaktor)
                    continue;
                //MW.Testliste.Items.Add("Min Ausd 0:" + Figuren[0].MinAusdehnungNormal.Normalenabstand.ToString());

                passt = true;
                Figuren[NeueFigSpeicherInd] = TestFiguren[0];
                Figuren[NeueFigSpeicherInd + 1] = TestFiguren[1];
                FigTeile[FigInd] = false;
                FigTeile[NeueFigSpeicherInd] = true;
                FigTeile[NeueFigSpeicherInd + 1] = true;
                glPoints[S1Ind] = SPS.S1;
                glPoints[S1Ind + 1] = SPS.S2;

            } while (!passt);

            return true;
        }
        private Figur2[] Erstelle2Teile(int figInd, int s1Ind, SPsKl sPS, int neueFigSpeicherInd)
        {
            Figur2[] testFiguren = new Figur2[2];
            int MAnzeck = Figuren[figInd].AnzEck;

            int StartIndex = sPS.indS1;
            int EndIndex = sPS.indS2;
            if (EndIndex < StartIndex)
                EndIndex += MAnzeck;
            
            // Der Fehler
            bool MEimBereich = false;
            if(sPS.indS1 == sPS.indS2)
            {
                //Festellen ob Mutter Ende zwischen S1 und S2 ist, dann mehr als 2 Ecken
                double MEWinkel= tools.BerechneWinkel(glPoints[Figuren[figInd].EckP[Figuren[figInd].AnzEck - 1]]);
                double S1Winkel= tools.BerechneWinkel(sPS.S1);
                double S2Winkel= tools.BerechneWinkel(sPS.S2);

                
                if (S1Winkel< S2Winkel)
                {
                    if (S1Winkel < MEWinkel && MEWinkel < S2Winkel)
                        MEimBereich = true;
                }
                else
                {
                    if (S1Winkel < MEWinkel || MEWinkel < S2Winkel)
                        MEimBereich = true;
                }   
                if (MEimBereich)
                    EndIndex += MAnzeck;        
            }
            int neuAnzEcken = EndIndex - StartIndex + 2;
            int[] hPind = new int[neuAnzEcken];
            bool[] hRind = new bool[neuAnzEcken];

            hPind[0] = s1Ind;
            hPind[hPind.Length - 1] = s1Ind + 1;
            hRind[0] = sPS.S1aufBogen;
            hRind[hRind.Length - 1] = false;

            int hInd;
            for (int ii = 1; ii < neuAnzEcken - 1; ii++)
            {
                hInd = StartIndex + ii;
                if (hInd >= MAnzeck)
                    hInd -= MAnzeck;
                hPind[ii] = Figuren[figInd].EckP[hInd];
                hRind[ii] = Figuren[figInd].FigurGeraden[hInd].Bogen;
            }
            //MW.Testliste.Items.Add("St End Anz:" + StartIndex.ToString() + " " + EndIndex.ToString() + " " + neuAnzEcken.ToString());
            testFiguren[0] = new Figur2(hRind, hPind, this, neueFigSpeicherInd, MW);

            //  2.Teil
            StartIndex = sPS.indS2;
            EndIndex = sPS.indS1;
            if (EndIndex <= StartIndex)
                EndIndex += MAnzeck;
            // Der Fehler , wenn vorher im Bereich dann hier Eckenanzahl wieder zurück
            if (sPS.indS1 == sPS.indS2 && MEimBereich)
                EndIndex -= MAnzeck;
            neuAnzEcken = EndIndex - StartIndex + 2;
            if (MAnzeck == 1)//Spezialfall Startfigur=Kreis
                neuAnzEcken = 2;
            hPind = new int[neuAnzEcken];
            hRind = new bool[neuAnzEcken];

            hPind[0] = s1Ind + 1;
            hPind[hPind.Length - 1] = s1Ind;

            hRind[0] = sPS.S2aufBogen;
            hRind[hRind.Length - 1] = false;

            for (int ii = 1; ii < neuAnzEcken - 1; ii++)
            {
                hInd = StartIndex + ii;
                if (hInd >= MAnzeck)
                    hInd = hInd - MAnzeck;
                hPind[ii] = Figuren[figInd].EckP[hInd];
                hRind[ii] = Figuren[figInd].FigurGeraden[hInd].Bogen;
            }
            //MW.Testliste.Items.Add("St End Anz:" + StartIndex.ToString() + " " + EndIndex.ToString() + " " + neuAnzEcken.ToString());
            testFiguren[1] = new Figur2(hRind, hPind, this, neueFigSpeicherInd + 1, MW);

            return testFiguren;
        }
        private void Zufallsgerade2(int figInd)
        {
            double[] ROLU = Figuren[figInd].ROLU_ber(glPoints,0);
            int Rand = 5;

            Random r1 = new Random();

            //Test!!!
            //zufXoben = -100;
            //zufXunten = 200;
            // Schnitt durch (0,0) bei TeileIn(0... Figur3


            if (MW.Opt.scnhitt0 && figInd == 0 && StartfigNr == 3)
            {
                double zuf2 = r1.NextDouble() * Math.PI;
                zufger = new Gerade2(new Point(Math.Cos(zuf2), Math.Sin(zuf2)), new Point(0, 0), this, false);
            }
            else
            {
                int zuf1 = r1.Next(2);
                if (zuf1 == 0)
                {
                    double zufXoben = r1.Next((int)ROLU[2] - Rand, (int)ROLU[0] + Rand);
                    double zufXunten = r1.Next((int)ROLU[2] - Rand, (int)ROLU[0] + Rand);
                    zufger = new Gerade2(new Point(zufXoben, ROLU[1] + Rand), new Point(zufXunten, ROLU[3] - Rand), this, false);
                }
                else
                {
                    double zufYlinks = r1.Next((int)ROLU[3], (int)ROLU[1]);
                    double zufYrechts = r1.Next((int)ROLU[3], (int)ROLU[1]);
                    zufger = new Gerade2(new Point(ROLU[2] - Rand, zufYlinks), new Point(ROLU[0] + Rand, zufYrechts), this, false);
                }
            }
        }
        private int bestimmeMaxTeil2()
        {
            double MaxBreite = 0;
            int MaxIndex = -1;
            for (int ii = 0; ii < FigTeile.Count(); ii++)
                if (FigTeile[ii] == true)
                {
                    if (Figuren[ii].MinAusdehnungNormal.Normalenabstand > MaxBreite)
                    {
                        MaxBreite = Figuren[ii].MinAusdehnungNormal.Normalenabstand;
                        MaxIndex = ii;
                    }
                }
            return MaxIndex;
        }
        internal void ZeichneFigMitte(int xx)
        {
            Point[] hpp;
            double ww = MW.GG1.ActualWidth;
            double hh = MW.GG1.ActualHeight;
            hpp = tools.VerSchiebe(new Point(MW.GG1.ActualWidth / 2, MW.GG1.ActualHeight / 2), glPoints);
            Figuren[xx].ZeichneFig(hpp, tools.Farben[xx],1,true);

            // Test!!!  Min. Normale Zeichnen
            Point[] hp = new Point[2];
            hp[0] = Figuren[xx].MinAusdehnungNormal.N1;
            hp[1] = Figuren[xx].MinAusdehnungNormal.N2;
            hpp = tools.VerSchiebe(new Point(MW.GG1.ActualWidth / 2, MW.GG1.ActualHeight / 2), hp);
            ZeichneStrecke(hpp[0], hpp[1], tools.Farben[1]);
        }
        internal void ZeichneStrecke(Point p1, Point p2, System.Windows.Media.SolidColorBrush col)
        {
            Line glline1 = new Line() { Stroke = col, X1 = p1.X, Y1 = p1.Y, X2 = p2.X, Y2 = p2.Y };
            MW.GG1.Children.Add(glline1);
        }
        internal void ZeichneKreuz(Point s1, System.Windows.Media.SolidColorBrush col)
        {
            int KreuzGr = 10;
            Point hp1 = new Point() { X = s1.X - KreuzGr, Y = s1.Y - KreuzGr };
            Point hp2 = new Point() { X = s1.X + KreuzGr, Y = s1.Y + KreuzGr };
            ZeichneStrecke(hp1, hp2, col);
            hp1 = new Point() { X = s1.X - KreuzGr, Y = s1.Y + KreuzGr };
            hp2 = new Point() { X = s1.X + KreuzGr, Y = s1.Y - KreuzGr };
            ZeichneStrecke(hp1, hp2, col);
        }
    }
}