using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AtomSimulator.Classes
{
    public class Atom : IComparable<Atom>,IEquatable<Atom>
    {
        private string name;
        private List<Shell> shells;
        private Point center;
        private int myRadius;
        private int picsPerSec;
        private Painter parent;
        private double direction;
        private int molName;

        public String Name => name;
        public List<Shell> Shells => shells;
        public Point Center => center;
        public int OuterWeight => shells.Last().Electrons.Count();
        public int MyRadius => myRadius;
        public double Direction { get{ return direction; } }
        public int MolName { get { return molName; } }

        public Atom(DataAtom a, Point c, Painter p,double direction,int number)
        {
            molName = number;
            this.direction = direction;
            parent = p;
            picsPerSec = 24;
            center = c;
            this.name = a.Name;
            int weight = a.Weight - a.OuterWeight;
            shells = new List<Shell>();
            int counter = 1;
            int data;
            while (weight > 0)
            {
                data = (int)(2 * Math.Pow(counter, 2));
                shells.Add(new Shell(counter, (weight > data ? data : weight), Center));
                weight -= data;
                counter++;
            }
            shells.Add(new Shell(a.Shells,a.OuterWeight, center));
            myRadius = 17 * shells.Count() + 10;
        }

        public void Delta(double x, double y)
        {
            center.X += x;
            center.Y += y;
            foreach (Shell s in shells)
                s.Delta(center.X, center.Y);
        }

        public int CompareTo(Atom other)
        {
            return shells.Count() - other.Shells.Count();
        }

        public double DistanceFrom(Atom other)
        {
            return Math.Sqrt(Math.Pow(center.X - other.Center.X, 2) + Math.Pow(center.Y - other.Center.Y,2));
        }

        public void Reflection(List<Atom> other)
        {
            foreach(Atom a in other)
            {
                double dir = 1;
                int fact = 1;
                if (CompareTo(a) >= 0)
                {
                    double dist = DistanceFrom(a);
                    if (dist > 0 && dist < (myRadius + a.MyRadius))
                    {
                        if (this.Equals(a))
                        {
                            if (this.molName != a.MolName)
                            {
                                fact = 0;
                                direction = a.direction - (a.direction<1? -1: 1);
                            }
                            dir = direction;
                        }
                        dist = dist * fact / shells.Count;
                        double angle = Navigate.Bearing(this, a) + ((int)(dir) % 2) * 180;
                        double x = dist * Math.Cos(angle * Math.PI / 180);
                        double y = dist * Math.Sin(angle * Math.PI / 180);
                        foreach (Shell s in shells)
                            s.Delta(center.X + shells.IndexOf(s) * x, center.Y + shells.IndexOf(s) * y);
                    }
                }
            }
            direction = (direction + 0.01);
            if (direction > 2)
            {
                direction -= 2;
            }
        }

        public void Increase()
        {
            picsPerSec *= 2;
        }

        public void Decrease()
        {
            picsPerSec /= 2;
        }

        public bool Equals(Atom other)
        {
            if (other == null)
                return false;
            return name.Equals(other.Name);
        }
    }
}
