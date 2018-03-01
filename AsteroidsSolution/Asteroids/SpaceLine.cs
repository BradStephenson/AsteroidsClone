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

namespace Asteroids
{
    class SpaceLine
    {
        public Point start { get; set; }
        public Point end   { get; set; }
        public double slope { get; }
        private Line l;
        

        public SpaceLine(Point start, Point end)
        {
            l = new Line();
            l.Stroke = new SolidColorBrush(Colors.White);
            l.X1 = start.X;
            l.Y1 = start.Y;
            l.X2 = end.X;
            l.Y2 = end.Y;
            this.slope = (l.Y2 - l.Y1)/(l.X2 - l.X1);
        }

        public void draw(Canvas c)
        {
            c.Children.Add(l);
        }

        public bool intersect(SpaceLine l)
        {
            

            return false;
        }
    }
}
