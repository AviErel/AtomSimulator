using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AtomSimulator.Classes
{
    public class Electron
    {
        private Color[] coloName = { Colors.Red, Colors.Green, Colors.Blue, Colors.Violet, Colors.Red, Colors.Green, Colors.Blue, Colors.Violet };
        private Color colorName;
        private double angle;
        private Random rnd;

        public Electron(int level)
        {
            rnd = new Random();
            colorName = coloName[level - 1];
            angle = 0;
        }

        public Color ColorName => colorName;

        public double Angle
        {
            get{
                angle = (angle+rnd.Next(1,120)) % 360;
                return angle;
            } }
    }
}
