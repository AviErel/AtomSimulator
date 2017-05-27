using System;
using System.Collections.Generic;
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
using AtomSimulator.Classes;
using System.Xml.Linq;
using System.Xml;

namespace AtomSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Painter drawing;
        private string xmlDocument="DataAtom.xml";

        public MainWindow()
        {
            InitializeComponent();
            var xmlDoc = XDocument.Load(xmlDocument);
            atomSelect.ItemsSource = (from c in xmlDoc.Root.Descendants("element") select c.Element("name").Value);
            atomSelect.SelectedIndex = 0;
            drawing = new Painter()
            {
                Width = Width,
                Height = Height
            };
            main.Children.Add(drawing);
            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            drawing.Width = this.ActualWidth;
            drawing.Height = this.ActualHeight;
        }

        protected void Button_Clicked(object sender, EventArgs e)
        {
            string name = (sender as Button).Name;
            switch (name)
            {
                case "plus":
                    ratio.Text="X"+drawing.IncreasSpeed();
                    break;
                case "minus":
                    ratio.Text="X"+drawing.IDecreasSpeed();
                    break;
                case "add":
                    DataAtom temp = new DataAtom();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlDocument);
                    XmlElement eml = doc.DocumentElement;//gets root element of xmldocument
                    XmlNode data= eml.SelectSingleNode(string.Format("element[name='{0}']", atomSelect.SelectedItem as string));
                    temp.Name = data["name"].InnerText;
                    temp.Shells = (int.Parse(data["shells"].InnerText));
                    temp.Weight = int.Parse(data["weight"].InnerText);
                    temp.OuterWeight = int.Parse(data["outerweight"].InnerText);
                    temp.Mol = int.Parse(data["molecule"].InnerText);
                    drawing.Add(temp);
                    break;
                case "clear":
                    for(int i=0;i< drawing.Childrens.Count();i++)
                    {
                        drawing.Childrens[i] = null;
                    }
                    drawing.Childrens.Clear();
                    break;
                case "path":
                    drawing.ActivatePath();
                    break;
                case "pause":
                    drawing.pause();
                    break;
                case "clean":
                    drawing.Clean();
                    break;
                case "regular":
                    drawing.regView();
                    break;
            }

        }

    }
}
