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
    class SpaceThing : Blastable
    {

        protected Point upperLeft;
        protected Vector moving;
        protected List<Point> verts;
        protected Matrix rotationMatrix;
        protected double rotationSpeed;
        protected int ScorePayout;
        protected SpaceSpawnStrategy Spawner;
        protected Point worldSize;
        protected double margin;

        //SpaceGame MyGame;

        public SpaceThing(Point worldSize)
        {
            margin = 50;
            verts = new List<Point>();
            verts = new List<Point>();
            moving = new Vector(1.1, 0.0);
            upperLeft = new Point(0, 0);
            rotationMatrix = new Matrix();
            rotationSpeed = 0;
            ScorePayout = 0;
            Spawner = new SmallSpawnStrategy();
            this.worldSize = worldSize;
            //sl = new Asteroids.SpaceLine();
            //this.MakeBigAsteroid();
        }

        public SpaceThing(double x, double y, Point worldSize)
            : this(worldSize)
        {
            upperLeft.X += x;
            upperLeft.Y += y;
        }

        public SpaceThing(Point p, Point worldSize)
            : this(worldSize)
        {
            upperLeft.X = p.X;
            upperLeft.Y = p.Y;
        }

        public virtual bool Collide(SpaceThing thing)
        {
            List<Line> list1 = this.GetLines();
            List<Line> list2 = thing.GetLines();
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

        public void SetSpawner(SpaceSpawnStrategy s)
        {
            Spawner = s;
        }

        public void SetPayout(int score)
        {
            ScorePayout = score;
        }

        public void SetRotationSpeed(double Speed)
        {
            this.rotationSpeed = Speed;
        }

        public void AddVert(Point p)
        {
            double num;
            verts.Add(p);
            foreach (Point point in verts)
            {
                num = Math.Sqrt(point.X * point.X * 4 + point.Y * point.Y * 4);
                if (num > margin)
                    margin = num;
            }
        }

        public void SetMoving(Vector v)
        {
            moving = v;
        }


        

        public virtual void Draw(Canvas c)
        {
            foreach (Line l in this.GetLines())
                c.Children.Add(l);

            c.InvalidateVisual();
        }

        public virtual void Simulate(SpaceGame g)
        {
            this.upperLeft += this.moving;
            if (upperLeft.X < 0)
                upperLeft.X = g.ScreenDim.X;
            if (upperLeft.X > g.ScreenDim.X)
                upperLeft.X = 0;
            if (upperLeft.Y < 0)
                upperLeft.Y = g.ScreenDim.Y;
            if (upperLeft.Y > g.ScreenDim.Y)
                upperLeft.Y = 0;

            rotationMatrix.Rotate(rotationSpeed);
        }

        public virtual void Blast(SpaceGame g)
        {
            g.RemoveFromLists(this);
            g.CreateDebris(upperLeft);
            Spawner.SpawnChildren(g, upperLeft);
            g.AddScore(ScorePayout);
        }

        public virtual List<Line> GetLines(Point offset)
        {
            List<Line> l = new List<Line>();

            int i;
            Point p = new Point(offset.X + upperLeft.X, offset.Y + upperLeft.Y);

            for (i = 0; i < verts.Count - 1; i++)
            {
                l.Add(MakeLine(verts[i], verts[i + 1], p, rotationMatrix));
            }
            if (verts.Count > 0)
            {
                l.Add(MakeLine(verts[verts.Count - 1], verts[0], p, rotationMatrix));
            }

            return l;
        }

        public virtual List<Line> GetLines()
        {
            List<Line> l = new List<Line>();

            l.AddRange(GetLines(new Point(0,0)));

            if ((upperLeft.Y + margin) > worldSize.Y)
                l.AddRange(GetLines(new Point(0, -worldSize.Y)));
            if ((upperLeft.Y - margin) < 0)
                l.AddRange(GetLines(new Point(0, worldSize.Y)));
            if ((upperLeft.X + margin) > worldSize.X)
                l.AddRange(GetLines(new Point(-worldSize.X, 0)));
            if ((upperLeft.X - margin) < 0)
                l.AddRange(GetLines(new Point(worldSize.X, 0)));







            return l;
        }

        public Point GetLocation()
        {
            return upperLeft;
        }

        // Static helper methods for linear collisions

        public static Line MakeDot(Point loc)
        {
            Line mk = MakeLine(new Point(0, 1), new Point(0, 0), loc, new Matrix());
            mk.StrokeThickness = 3;
            return mk;
        }

        public static Line MakeLine(Point first, Point second, Point loc, Matrix rotator)
        {
            Point vert1 = Point.Multiply(first, rotator);
            Point vert2 = Point.Multiply(second, rotator);
            Line line = new Line();
            line.X1 = vert1.X + loc.X;
            line.Y1 = vert1.Y + loc.Y;
            line.X2 = vert2.X + loc.X;
            line.Y2 = vert2.Y + loc.Y;

            line.Stroke = new SolidColorBrush(Colors.White);
            return line;
        }

        public static bool IsIntersecting(Point a, Point b, Point c, Point d)
        {
            double denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            double numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            double numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            double r = numerator1 / denominator;
            double s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

        //Found this algorithm online a long time ago. The link to where I got it seems to be broken.
        public static bool linesIntersect(Line line1, Line line2)
        {
            double d = ((line1.X2 - line1.X1) * (line2.Y2 - line2.Y1)) - ((line1.Y2 - line1.Y1) * (line2.X2 - line2.X1));
            double r = ((line1.Y1 - line2.Y1) * (line2.X2 - line2.X1)) - ((line1.X1 - line2.X1) * (line2.Y2 - line2.Y1));
            double s = ((line1.Y1 - line2.Y1) * (line1.X2 - line1.X1)) - ((line1.X1 - line2.X1) * (line1.Y2 - line1.Y1));
            
            if (d == 0) return r == 0 && s == 0;

            r = r / d;
            s = s / d;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

        public static bool onLine()
        {
            return false;
        }

        public static double min(double first, double second)
        {
            double res = first;
            if (first > second)
                res = second;
            return res;
        }

        public static double max(double first, double second)
        {
            double res = first;
            if (first < second)
                res = second;
            return res;
        }

        


        /*public static bool linesIntersect(Line line1, Line line2)
        {
            double run1 = line1.X2 - line1.X1;
            double run2 = line2.X2 - line2.X1;


            if (run1 == 0.0 || run2 == 0.0)
            {
                if (run1 == 0.0 && run2 == 0.0)
                    return line1.X1 == line2.X1;
                return false;  // one or both is vertical
            }

            double m1 = (line1.Y2 - line1.Y1) / run1;
            double m2 = (line2.Y2 - line2.Y1) / run2;

            

            double b1 = line1.Y1 - m1 * line1.X1;
            double b2 = line2.Y1 - m1 * line2.X1;

            if (m1 == m2)
                return (b1 == b2) && !(      (min(line1.X1, line1.X2) > max(line2.X1, line2.X2)) || (min(line2.X1, line2.X2) > max(line1.X1, line1.X2)));// lines are parallel

            double x = (b1 - b2) / (m2 - m1);
            double y = (m2 * b1 - m1 * b2) / (m2 - m1);

            return (x > min(line1.X1, line1.X2) && x < max(line1.X1, line1.X2)) && (x > min(line2.X1, line2.X2) && x < max(line2.X1, line2.X2));
        }*/
    }
}
