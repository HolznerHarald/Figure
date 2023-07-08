using System;
using System.Windows;

namespace Figure
{
    internal class SPsKl
    {
        internal int AnzS;
        internal Point S1;
        internal Point S2;
        internal int indS1;
        internal int indS2;
        internal bool S1aufBogen = false;
        internal bool S2aufBogen = false;

        public SPsKl() 
        { 
            AnzS = 0;           
        }

        internal void CheckS(Point s1,bool bRund,int KurvenIndex)
        {
            AnzS++;
            if (AnzS == 1)
            {
                S1 = new Point(s1.X,s1.Y);                
                S1aufBogen = bRund;
                indS1 = KurvenIndex;
            }
            else if (AnzS == 2)
            {
                S2 = new Point(s1.X, s1.Y);                
                S2aufBogen = bRund;
                indS2 = KurvenIndex;    
            }
        }
    }
}