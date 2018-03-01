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
    class SpaceShip : SpaceThing
    {
        private int count;
        public bool Thrusting { get; set; }
        public SpaceShip(Point loc, Point ScreenDim )
            : base(ScreenDim)
        {
            count = 0;
            this.upperLeft = loc;

            this.AddVert(new Point(0, 25));
            this.AddVert(new Point(-10, -9));
            this.AddVert(new Point(10, -9));

            this.AddVert(new Point(-8, -2));
            this.AddVert(new Point(8, -2));

            this.AddVert(new Point(0, -22));
            this.AddVert(new Point(6, -11));
            this.AddVert(new Point(-6, -11));
        }

        public override void Blast(SpaceGame g)
        {
            g.LoseLife();
            g.TimeInPlayer(60);
            g.CreateDebris(upperLeft);
            g.CreateDebris(upperLeft);
            g.CreateDebris(upperLeft);
            g.CreateDebris(upperLeft);
        }

        public override bool Collide(SpaceThing thing)
        {
            bool t = Thrusting;
            Thrusting = false;
            List<Line> list1 = this.GetLines();
            List<Line> list2 = thing.GetLines();
            Thrusting = t;
            foreach (Line l1 in list1)
                foreach (Line l2 in list2)
                    if (SpaceThing.linesIntersect(l1, l2))
                    {
                        //MessageBox.Show("x " + l1.X1 + "   y " + l1.Y1 + Environment.NewLine + "x " + l1.X2 + "   y " + l1.Y2 + Environment.NewLine +
                        //Environment.NewLine + "x " + l2.X1 + "   y " + l2.Y1 + Environment.NewLine + "x " + l2.X2 + "   y " + l2.Y2);
                        return true;
                    }
            return false;
        }

        public override List<Line> GetLines(Point offset)
        {
            List<Line> l = new List<Line>();

            Point p = new Point(offset.X + upperLeft.X, offset.Y + upperLeft.Y);

            l.Add(SpaceThing.MakeLine(verts[0], verts[1], p, rotationMatrix));
            l.Add(SpaceThing.MakeLine(verts[0], verts[2], p, rotationMatrix));
            l.Add(SpaceThing.MakeLine(verts[3], verts[4], p, rotationMatrix));

            if (Thrusting)
            {
                if (count*5 % 2 == 0)
                {
                    l.Add(SpaceThing.MakeLine(verts[5], verts[6], p, rotationMatrix));
                    l.Add(SpaceThing.MakeLine(verts[5], verts[7], p, rotationMatrix));
                }
            }

            return l;
        }

        public override List<Line> GetLines()
        {
            /*List<Line> l = new List<Line>();

            l.Add(SpaceThing.MakeLine(verts[0], verts[1], upperLeft, rotationMatrix));
            l.Add(SpaceThing.MakeLine(verts[0], verts[2], upperLeft, rotationMatrix));
            return l;*/

            List<Line> l = new List<Line>();

            l.AddRange(GetLines(new Point(0, 0)));

            if ((upperLeft.Y + margin) > worldSize.Y)
                l.AddRange(GetLines(new Point(0, -worldSize.Y)));
            if ((upperLeft.Y - margin) < 0)
                l.AddRange(GetLines(new Point(0, worldSize.Y)));
            if ((upperLeft.X + margin) > worldSize.Y)
                l.AddRange(GetLines(new Point(-worldSize.X, 0)));
            if ((upperLeft.X - margin) > 0)
                l.AddRange(GetLines(new Point(worldSize.X, 0)));







            return l;
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);
        }

        public override void Simulate(SpaceGame g)
        {
            base.Simulate(g);
            count++;
            if (Thrusting)
                moving += .6*(new Vector(0, 1) * rotationMatrix);
            
        }

        public void Shoot(SpaceGame g)
        {
            SpaceBullet b = new SpaceBullet(upperLeft + 34 * ((new Vector(0, 1)) * rotationMatrix), worldSize);

            b.SetMoving(this.moving + 17 * ((new Vector(0, 1)) * rotationMatrix));
            //MessageBox.Show("" + upperLeft);

            g.AddBullet(b);
        }
    }
}
