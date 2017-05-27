using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AtomSimulator.Classes
{
    public static class Navigate
    {

        public static double Bearing(Atom from, Atom other)
        {
            double tan, sin, angle;
            sin = (other.Center.Y - from.Center.Y);
            tan = (sin / (other.Center.X - from.Center.X));
            angle = Math.Atan(tan) * 180 / Math.PI;
            if (sin * tan < 0)
                angle += 180;
            else
                angle += 360;
            return angle % 360;
        }

        public static double Bearing(Point from, Point other)
        {
            double tan, sin, angle;
            sin = (other.Y - from.Y);
            tan = (sin / (other.X - from.X));
            angle = Math.Atan(tan) * 180 / Math.PI;
            if (sin * tan < 0)
                angle += 180;
            else
                angle += 360;
            return angle % 360;
        }

        public static bool Equals(Point from,Point to)
        {
            if (from == null || to == null)
                return false;
            double dist = Math.Sqrt(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2))-10;
            return (dist <= 0);
        }
    }
}
