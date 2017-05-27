using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AtomSimulator.Classes
{
    public class Molecule
    {
        private List<Atom> atoms;
        private Point center;
        private int radius;
        private int angle;
        private List<Point> path = new List<Point>();
        private Painter parent;
        private int direction = 1;

        public bool Start = false;

        //public Molecule(Painter p, DataAtom a)
        //{
        //    center = new Point(0, 0);
        //    if (a.Mol > 1)
        //    {
        //        angle = 360 / a.Mol - 1;
        //    }
        //    radius = a.Shells * 17 + 10;
        //    atoms = new List<Atom>();
        //    for (int i = 0; i < a.Mol; i++)
        //    {
        //        Point c = new Point(2 * radius * Math.Cos(angle * Math.PI / 180) + Center.X, 2 * radius * Math.Sin(angle * Math.PI / 180) + Center.Y);
        //        atoms.Add(new Atom(a,c,p,(i==0 ? 0:1)));
        //    }
        //    parent = p;
        //}

        public Molecule(Painter p,DataAtom a,Point cent,int number)
        {
            radius = a.Shells * 14 + 5;
            atoms = new List<Atom>();
            int fact = 0; ;
            center = cent;
            if (a.Mol > 1)
            {
                angle = 360 / a.Mol - 1;
            }
            for (int i = 0; i < a.Mol; i++) { 

                fact = (a.Mol == 1 ?  0 : radius);
                Point c = new Point( fact* Math.Cos(i*angle * Math.PI / 180)+Center.X, fact * Math.Sin(i*angle * Math.PI / 180)+Center.Y);
                atoms.Add(new Atom(a, c, p,(i==0?0:1),number));
            }
            parent = p;
        }

        public List<Atom> Atoms => atoms;

        public Point Center => center;

        public int MyRadius => 2 * radius;

        public Point Path { set => path.Add(value); }

        public void Move()
        {
            direction *= -1;
            double x=0, y=0;
            if(path.Count()>0)
            {
                double angle = Navigate.Bearing(center, path[0])*Math.PI/180;
                x = Math.Cos(angle);
                y = Math.Sin(angle);
                center.X += x;
                center.Y += y;
                foreach (Atom a in atoms)
                {
                    a.Delta(x, y);
                }
                if (Navigate.Equals(center,path[0]))
                    path.RemoveAt(0);
            }
            else
            {
                Start = false;
            }
        }
    }
}
