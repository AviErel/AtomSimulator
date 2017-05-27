using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AtomSimulator.Classes
{
    /// <summary>
    /// Interaction logic for Painter.xaml
    /// </summary>
    /// 

    public partial class Painter : UserControl
    {

        public List<Molecule> Childrens;
        private int picPerSeconds = 120;
        private double ratio = 1, min = 0.125, max = 4;
        DispatcherTimer tick;
        private Point center;
        private Point clicked;
        private bool path;
        private Molecule selected;
        private bool clean = false;

        public Painter()
        {
            InitializeComponent();
            path = false;
            Width = 300;
            Height = 300;
            canvas.Width = this.Width - 10;
            canvas.Height = this.Height - 10;
            canvas.Background = new SolidColorBrush(Colors.AliceBlue);
            Childrens = new List<Molecule>();
            tick = new DispatcherTimer();
            tick.Interval = new TimeSpan(0,0,0,0,1000/picPerSeconds);
            tick.Tick += Tick_Tick; ;
            tick.Start();
            center.X = canvas.Width / 2;
            center.Y = canvas.Height / 2;
            SizeChanged += Painter_SizeChanged;
        }

        public List<Atom> FlatList()
        {
            List<Atom> temp = new List<Atom>();
            foreach (Molecule m in Childrens)
                foreach (Atom a in m.Atoms)
                    temp.Add(a);
            return temp;
        }

        private void Painter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.Width = ActualWidth - 10;
            canvas.Height = ActualHeight - 10;
            center.X = canvas.Width / 2;
            center.Y = canvas.Height / 2;
        }

        private void Tick_Tick(object sender, EventArgs e)
        {
            List<Atom> atoms = FlatList();
            tick.Stop();
            foreach (Molecule m in Childrens)
                    if(m.Start)
                        m.Move();
            for (int i = 0; i < atoms.Count();i++)
            {
                atoms[i].Reflection(atoms.FindAll(a => a.MolName != atoms[i].MolName));
                atoms[i].Reflection(atoms.FindAll(a => a.MolName == atoms[i].MolName));
            }
            DrawChildrens();
            tick.Start();
        }

        private void DrawChildrens()
        {
            Ellipse temp;
            canvas.Children.Clear();
            foreach (Atom c in FlatList())
            {
                if (!clean)
                {
                    foreach (Shell s in c.Shells)
                        foreach (Electron e in s.Electrons)
                        {
                            temp = new Ellipse()
                            {
                                Fill = new SolidColorBrush(e.ColorName),
                                Width = 6,
                                Height = 6
                            };
                            double angle = e.Angle * Math.PI / 180;
                            Canvas.SetLeft(temp, center.X + Math.Cos(angle) * (s.Radius) + s.Center.X - 3);
                            Canvas.SetTop(temp, center.Y - Math.Sin(angle) * (s.Radius) - s.Center.Y - 3);
                            canvas.Children.Add(temp);
                        }
                }
                else
                {
                    Shell s = c.Shells.Last();
                    temp = new Ellipse()
                    {
                        Stroke = new SolidColorBrush(Colors.Gray),
                        Width = s.Radius * 2,
                        Height = s.Radius * 2
                    };
                    Canvas.SetLeft(temp, center.X + s.Center.X - s.Radius);
                    Canvas.SetTop(temp, center.Y - s.Center.Y - s.Radius);
                    canvas.Children.Add(temp);
                }
            }
            foreach (Atom c in FlatList())
            {
                temp = new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Yellow),
                    Width = 30,
                    Height = 30
                };
                Canvas.SetLeft(temp, center.X + c.Center.X - 15);
                Canvas.SetTop(temp, center.Y - c.Center.Y - 15);
                canvas.Children.Add(temp);
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            clicked = e.GetPosition(canvas as IInputElement);
            clicked.X -= center.X;
            clicked.Y = center.Y-clicked.Y;
            foreach (Molecule a in Childrens)
                if (Math.Sqrt(Math.Pow(clicked.X - a.Center.X, 2) + Math.Pow(clicked.Y - a.Center.Y, 2)) < a.MyRadius)
                    StartPath(a);
            if (path)
            {
                selected.Path = clicked;
            }
        }

        private void StartPath(Molecule a)
        {
            path = true;
            selected = a;
        }

        public double IncreasSpeed()
        {
            tick.Stop();
            if (ratio < max)
                ratio *= 2;
            tick.Interval = new TimeSpan(0, 0, 0, 0, 1000 / (int)(ratio * picPerSeconds));
            tick.Start();
            return ratio;
        }

        public double IDecreasSpeed()
        {
            tick.Stop();
            if (ratio > min)
                ratio /= 2;
            tick.Interval = new TimeSpan(0, 0, 0, 0, 1000 / (int)(ratio * picPerSeconds));
            tick.Start();
            return ratio;
        }

        public void Add(DataAtom a)
        {
            Childrens.Add(new Molecule(this,a,clicked,Childrens.Count));
        }

        public void regView()
        {
            st.CenterX = center.X;
            st.CenterY = center.Y;
            st.ScaleX = 1;
            st.ScaleY = 1;
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            st.CenterX = e.GetPosition(canvas).X;
            st.CenterY = e.GetPosition(canvas).Y;
            if(e.Delta>0)
            {
                st.ScaleX *= 1.1;
                st.ScaleY *= 1.1;
            }
            else
            {
                st.ScaleX /= 1.1;
                st.ScaleY /= 1.1;
            }
        }

        public void ActivatePath()
        {
            foreach(Molecule m in Childrens)
                m.Start = true;
            path = false;
        }

        public void pause()
        {
            if(tick.IsEnabled)
            {
                tick.Stop();
            }
            else
            {
                tick.Start();
            }
        }

        public void Clean()
        {
            clean = !clean;
        } 
    }
}
