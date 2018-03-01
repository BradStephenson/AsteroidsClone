using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Asteroids
{
    class SpaceDebris : Blastable
    {
        private int timer;

        public Point loc;
        public Vector moving;

        public SpaceDebris(Point loc, Vector moving, int time)
        {
            this.loc = loc;
            this.moving = moving;
            timer = time;
        }


        public void Blast(SpaceGame g)
        {
            g.RemoveFromDebris(this);
        }

        public void Draw(Canvas c)
        {
            c.Children.Add(SpaceThing.MakeLine(new Point(0, 1), new Point(0, 0), loc, new System.Windows.Media.Matrix()));
        }

        public void Simulate(SpaceGame g)
        {
            timer--;
            loc += moving;
            if (timer <= 0)
                g.Blast(this);
        }
    }
}
