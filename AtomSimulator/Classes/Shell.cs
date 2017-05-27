using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AtomSimulator.Classes
{
    public class Shell
    {
        private List<Electron> electrons;
        private int radius;
        private Point center;

        public Shell(int level,int num,Point c)
        {
            electrons = new List<Electron>();
            center = c;
            for (int count = 0; count < num;count++)
            {
                electrons.Add(new Electron(level));
                for(int i=0;i<count*360/num;i++)
                {
                    double temp=electrons.Last().Angle;
                }
            }
            radius = 10 + 17 * level;
        }

        public List<Electron> Electrons => electrons;
        public int Radius => radius;
        public Point Center => center;

        public void Delta(double x,double y)
        {
            center.X = x;
            center.Y = y;
        }
    }
}
