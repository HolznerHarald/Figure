using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Figure
{
    internal class Options
    {      
        internal bool[] WelcheTeile = new bool[6] {false,false,true,false,false,false};
        internal bool scnhitt0 = true;
        internal string fname = "Inits.txt";
        public Options(MainWindow mW)
        {
         string[] ts;
            
            if (!File.Exists(fname))
                return;
            
            ts = File.ReadAllLines(fname);
            File.Delete(fname);

            if (ts.Length !=2 )
            {
                //MW.Testliste.Items.Add("Inits.txt ungleich 2 Zeilen: Standardoptions werden verwendet");
                return;
            }
             
            string hs = ts[0];
            for(int ii=0;ii<hs.Count();ii++)
                if (hs[ii] !='3'&& hs[ii] != '4' && hs[ii] != '5' && 
                   hs[ii] != '6' && hs[ii] != '7' && hs[ii] != '8')
                {                
                   //MW.Testliste.Items.Add("Inits.txt 1.Zeile falsch: Standardoptions werden verwendet");
                    return;
                }
            for (int ii = 0; ii < WelcheTeile.Count(); ii++)
                WelcheTeile[ii] = false;

            for (int ii = 0; ii < hs.Count(); ii++)
            {
                int ind = hs[ii] - '0';
                
                WelcheTeile[ind-3] = true;
            }

            string hs1 = ts[1];
            if (hs1 == "NichtDurch00")
                scnhitt0 = false;
        }

        internal void AnsichtClick()
        {
            Settings settings = new Settings();
            settings.Init(this);

            bool? Res = settings.ShowDialog();
            if (Res == false)
                return;
            if (settings.CH1.IsChecked == true)
                WelcheTeile[0] = true;
            else
                WelcheTeile[0] = false;
            if (settings.CH2.IsChecked == true)
                WelcheTeile[1] = true;
            else
                WelcheTeile[1] = false;
            if (settings.CH3.IsChecked == true)
                WelcheTeile[2] = true;
            else
                WelcheTeile[2] = false;
            if (settings.CH4.IsChecked == true)
                WelcheTeile[3] = true;
            else
                WelcheTeile[3] = false;
            if (settings.CH5.IsChecked == true)
                WelcheTeile[4] = true;
            else
                WelcheTeile[4] = false;
            if (settings.CH6.IsChecked == true)
                WelcheTeile[5] = true;
            else
                WelcheTeile[5] = false;
            // Schnitt durch 0
            if(settings.Schnitt0.IsChecked == true)
                scnhitt0 = true;
            else
                scnhitt0 = false;
        }

        internal void close()
        {
            string hs="";
            String hs1 = "Durch00";
            for (int ii = 0; ii < WelcheTeile.Count(); ii++)
                if (WelcheTeile[ii])
                    hs=hs+(ii+3).ToString();

            if (!scnhitt0)
                hs1 = "NichtDurch00";
            string[] text = {
                hs,
                hs1
            };
            
            File.WriteAllLines(fname, text);
        }
    }
}
