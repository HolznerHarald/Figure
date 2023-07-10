using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;
using System.IO;
using Rectangle = System.Drawing.Rectangle;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Figure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindow MW;
        internal FigContr2 FC2;
        internal PrintDocument pd;
        internal Options Opt;
        internal LetzteAufgabe LA;
        internal int FigAnz;

       /* internal bool letzteForm;
        internal Point[] letzteeGlP;
        internal int letzeZuf1;
        internal int letzteFigNr;*/

        public MainWindow()
        {
            InitializeComponent();
            MW = System.Windows.Application.Current.MainWindow as MainWindow;
            Opt = new Options(MW);
            LA = new LetzteAufgabe();
            LoeBut.IsEnabled = false;
            WhBut.IsEnabled = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Opt.close();
        }
        private void new_f2(bool bSchnittAufgabe)
        {
            int aa = GG1.Children.Count;
            GG1.Children.RemoveRange(0, aa);

            Random r1 = new Random();
            bool bForm;
            if (Rund.IsChecked == true)
                bForm = true;
            else if (Eckig.IsChecked == true)
                bForm = false;
            else
            {
                int iForm = r1.Next(2);
                if (iForm == 0)
                    bForm = false;
                else
                    bForm = true;
            }
            int FigNr;
            if (bForm)
            {
                FigAnz = 4;
                FigNr = r1.Next(FigAnz);
            }
            else
            {
                FigAnz = 6;
                FigNr = r1.Next(FigAnz);
            }                      
            Testliste.Items.Add(FigNr);
            int zuf1;
            do
            {
                zuf1 = r1.Next(6);

            } while (!Opt.WelcheTeile[zuf1]);

            int AnzTeile = zuf1 +3;
            double[] Alph = new double[AnzTeile];            
            for (int ii = 0; ii < AnzTeile; ii++)
                Alph[ii] = r1.NextDouble() * 2 * Math.PI;

                        
            LA.letzteForm = bForm;
            FC2 = new FigContr2(MW, bForm, FigNr, zuf1 + 3, Alph);
            
            while (!FC2.Starte(false))
                ;
            LA.LetzteSchnittOderAufgabe= bSchnittAufgabe;
            LA.letzteeGlP = FC2.glPoints;
            LA.letzeZuf1 = zuf1;
            LA.letzteFigNr = FigNr;
            LA.letzteAlph= Alph;

            LoeBut.IsEnabled = true;
            WhBut.IsEnabled = true;
        }        
        internal void ZeichneFigX(int xx)
        {
            //    int aa = GG1.Children.Count;
            //    GG1.Children.RemoveRange(0, aa);
             FC2.ZeichneFigMitte(xx);
            // FC.Figuren[xx].ZeichneNormale(System.Windows.Media.Brushes.Red);
        }
        private void Schnitt_Click(object sender, RoutedEventArgs e)
        {  
            new_f2(true);
            FC2.ZeichneAlleTeileMitte(1, MW.GG1.ActualWidth / 2, MW.GG1.ActualHeight / 2,true);                
        }
        private void Aufgabe_Click(object sender, RoutedEventArgs e)
        {
            new_f2(false);

            FC2.ZeichneAufgabe(false, false);
            for (int ii = 0; ii < FigAnz; ii++)
            {
                FigContr2 FCX = new FigContr2(MW, FC2.FigFormRund, ii, 1,new double[0]);
                FCX.ZeichneAlleTeileMitte(0.2, (ii + 1) * MW.GG1.ActualWidth / (FigAnz + 1), MW.GG1.ActualHeight * 3 / 4, false);
            }           
        }
        private void Loesung_Click(object sender, RoutedEventArgs e)
        {
            LoeBut.IsEnabled = false;
            int aa = GG1.Children.Count;
            GG1.Children.RemoveRange(0, aa);    
            double Faktor =FC2.ZeichneAufgabe(true,true);
            if(FC2.StartfigNr == 1  && FC2.FigFormRund)
                FC2.ZeichneAlleTeileMitte(Faktor, MW.GG1.ActualWidth / 2, MW.GG1.ActualHeight *1/2,true);                    
            else
                FC2.ZeichneAlleTeileMitte(Faktor, MW.GG1.ActualWidth / 2, MW.GG1.ActualHeight *3 / 4,true);                    
        }
        private void WiederHolen_Click(object sender, RoutedEventArgs e)
        {
            LoeBut.IsEnabled = true;
            int aa = GG1.Children.Count;
            GG1.Children.RemoveRange(0, aa);

            //Testliste.Items.Add(LA.letzteFigNr);

            FC2 = new FigContr2(MW, LA.letzteForm, LA.letzteFigNr, LA.letzeZuf1 +3,LA.letzteAlph);
            FC2.Starte(true);
            
            if (LA.LetzteSchnittOderAufgabe)
                FC2.ZeichneAlleTeileMitte(1, MW.GG1.ActualWidth / 2, MW.GG1.ActualHeight / 2, true);
            else
            {
                FC2.ZeichneAufgabe(false, false);
                for (int ii = 0; ii < FigAnz; ii++)
                {
                    FigContr2 FCX = new FigContr2(MW, FC2.FigFormRund, ii, 1, new double[0]);
                    FCX.ZeichneAlleTeileMitte(0.2, (ii + 1) * MW.GG1.ActualWidth / (FigAnz + 1), MW.GG1.ActualHeight * 3 / 4, false);
                }
            }
        }
        private void Einstellungen(object sender, RoutedEventArgs e)
        {
            Opt.AnsichtClick();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Test!!!
            // Create a StreamGeometry to use to specify myPath.
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;


            using (StreamGeometryContext context = geometry.Open())
            {

                // Create a path to draw a geometry with.
                System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
                myPath.StrokeThickness = 2;
                myPath.Stroke = System.Windows.Media.Brushes.Black;
                myPath.Fill = System.Windows.Media.Brushes.Black;

                int left_edge = 100;
                int right_edge = 200;
                int top_edge = 200;
                int bottom_edge = 100;
                    
                context.BeginFigure(new Point(left_edge, bottom_edge), true /*isFilled*/, true /*isClosed*/);
                context.LineTo(new Point(left_edge, top_edge), true /*isStroked*/, true /*isSmoothJoin*/);
                context.LineTo(new Point(right_edge, top_edge), true /*isStroked*/, true /*isSmoothJoin*/);
                context.LineTo(new Point(right_edge, bottom_edge), true /*isStroked*/, true /*isSmoothJoin*/);
                geometry.Freeze();
                myPath.Data = geometry;
                GG1.Children.Add(myPath);                
            }

        }
        private void BMSave(object sender, RoutedEventArgs e)
        {
            var bmp1 = new Bitmap(10000, 10000);
            Graphics g1 = Graphics.FromImage(bmp1);
            System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Red, 10);
            System.Drawing.PointF[] PF = new PointF[] { new PointF(0, 0), new PointF(bmp1.Width / 10, bmp1.Height / 20), new PointF(bmp1.Width - 1, bmp1.Height - 1), new PointF(0, 0) };
            g1.DrawPolygon(blackPen, PF);
            File.Delete("C:\\C#\\T1.jpg");
            bmp1.Save("C:\\C#\\T1.jpg");
        }
        private void PrintJPG(object sender, RoutedEventArgs e)
        {
            pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            for (int ii = 0; ii < pd.PrinterSettings.PrinterResolutions.Count; ii++)
            {
                pd.DefaultPageSettings.PrinterResolution = pd.PrinterSettings.PrinterResolutions[ii];
                Testliste.Items.Add(pd.DefaultPageSettings.PrinterResolution.ToString());
            }

            pd.Print();
        }
        private void PrintPage(object o, PrintPageEventArgs e)
        {
            //e.PageSettings
            //e.PageBounds();
            pd.DefaultPageSettings.PrinterResolution = pd.PrinterSettings.PrinterResolutions[0];
            Testliste.Items.Add(pd.DefaultPageSettings.PrinterResolution.ToString());
            Rectangle rr = e.PageBounds;
            Testliste.Items.Add(rr.Height);
            Testliste.Items.Add(rr.Width);

            System.Drawing.Image img = System.Drawing.Image.FromFile("C:\\C#\\T1.jpg");
            Point loc = new Point(100, 100);
            img = new Bitmap(img, new System.Drawing.Size(rr.Width, rr.Height));
            e.Graphics.DrawImage(img, new System.Drawing.Point(0, 0));
        }
        private void Delete_Lines(object sender, RoutedEventArgs e)
        {
            int aa = GG1.Children.Count;
            GG1.Children.RemoveRange(0, aa);
        }
        private void Zeichnen0(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(0);
        }
        private void Zeichnen1(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(1);
        }
        private void Zeichnen2(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(2);
        }
        private void Zeichnen3(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(3);
        }
        private void Zeichnen4(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(4);
        }
        private void Zeichnen5(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(5);

        }
        private void Zeichnen6(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(6);

        }
        private void Zeichnen7(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(7);

        }
        private void Zeichnen8(object sender, RoutedEventArgs e)
        {
            ZeichneFigX(8);

        }

    }
}
