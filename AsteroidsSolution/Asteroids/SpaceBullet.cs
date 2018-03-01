using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Asteroids
{
    class SpaceBullet : SpaceThing
    {
        private Point prev;
        private int timer;

        public SpaceBullet(Point loc, Point screenDim)
            : base(loc, screenDim)
        {
            timer = 30;
        }

        public override void Draw(Canvas c)
        {
            c.Children.Add(SpaceThing.MakeDot(upperLeft));
            //MessageBox.Show("hi");
        }

        public override void Simulate(SpaceGame g)
        {
            prev.X = upperLeft.X;
            prev.Y = upperLeft.Y;
            // Had to grab this from the base class 
            // because I want to do a check and cancel the way
            // I get the line from it for the current frame.
            // This prevents teleporting bullets from killing everything between teleportation spots.

            this.upperLeft += this.moving;
            if (upperLeft.X < 0)
            {
                upperLeft.X = g.ScreenDim.X;
                prev.X = upperLeft.X;
                prev.Y = upperLeft.Y;
            }
            if (upperLeft.X > g.ScreenDim.X)
            {
                upperLeft.X = 0;
                prev.X = upperLeft.X;
                prev.Y = upperLeft.Y;
            }
            if (upperLeft.Y < 0)
            {
                upperLeft.Y = g.ScreenDim.Y;
                prev.X = upperLeft.X;
                prev.Y = upperLeft.Y;
            }
            if (upperLeft.Y > g.ScreenDim.Y)
            {
                upperLeft.Y = 0;
                prev.X = upperLeft.X;
                prev.Y = upperLeft.Y;
            }
            timer--;
            if (timer <= 0)
                g.Blast(this);
            //MessageBox.Show("sim");
        }

        public override List<Line> GetLines()
        {
            List<Line> l = new List<Line>();
            Line line = SpaceThing.MakeLine(prev, upperLeft, new Point(0, 0), new System.Windows.Media.Matrix());
            //MessageBox.Show("x " + line.X1 + "   y " + line.Y1 + Environment.NewLine + "x " + line.X2 + "   y " + line.Y2);
            l.Add(line);
            return l;
        }

        public override void Blast(SpaceGame g)
        {
            g.RemoveFromLists(this);
        }
    }
}
