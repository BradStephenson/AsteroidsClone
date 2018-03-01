using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Asteroids
{
    class SpaceFactory
    {
        private Random rand;
        private SpaceGame sg;

        public SpaceFactory(SpaceGame sg)
        {
            rand = new Random();
            this.sg = sg;
            
        }

        public List<SpaceDebris> DebrisCloud(Point loc)
        {
            List<SpaceDebris> cloud = new List<SpaceDebris>();
            int num = rand.Next() % 6 + rand.Next() % 6 + 4; // 2d6 + 2
            int i;
            for (i = 0; i < num; i++)
            {
                cloud.Add(new SpaceDebris(loc, RandomVector(3.0), rand.Next() % 6 + rand.Next() % 6 + 25));
            }


            return cloud;
        }

        public Vector RandomVector(double magFactor)
        {
            Vector uv = new Vector(1, 0);
            Matrix m = (new Matrix());
            m.Rotate(rand.NextDouble() * 360.0);

            uv *= m;
            double scalar = (rand.NextDouble() * magFactor + .5 * magFactor);
            uv *= scalar;

            return uv;
        }

        public SpaceAlien GiveMeAnAlien(int score)
        {
            SpaceAlien alien = BigAlien();

            double first = rand.NextDouble();
            double second = rand.NextDouble();

            first *= 20000;
            second *= score;

            if (second > first)
                alien = LittleAlien();

            if (score > 40000)
                alien = LittleAlien();
            return alien;
        }

        private SpaceAlien AlienHelper(int scale)
        {
            int x = (rand.Next() % 2) * ((int)sg.ScreenDim.X);
            int y = rand.Next() % (int)sg.ScreenDim.Y;
            SpaceAlien alien = new SpaceAlien(sg.ScreenDim, x, y);

            alien.AddVert(new Point(3 * scale, -2 * scale));
            alien.AddVert(new Point(0 * scale, -2 * scale));
            alien.AddVert(new Point(.5 * scale, -3 * scale));
            alien.AddVert(new Point(2.5 * scale, -3 * scale));
            alien.AddVert(new Point(3 * scale, -2 * scale));
            alien.AddVert(new Point(5 * scale, -1 * scale));
            alien.AddVert(new Point(-2 * scale, -1 * scale));
            alien.AddVert(new Point(0 * scale, -0 * scale));
            alien.AddVert(new Point(3 * scale, -0 * scale));
            alien.AddVert(new Point(5 * scale, -1 * scale));
            alien.AddVert(new Point(-2 * scale, -1 * scale));
            alien.AddVert(new Point(0 * scale, -2 * scale));

            return alien;
        }

        public SpaceAlien BigAlien()
        {
            SpaceAlien alien = AlienHelper(8);
            alien.SetPayout(200);
            alien.ShootMod = 40;
            return alien;
        }

        public SpaceAlien LittleAlien()
        {
            SpaceAlien alien = AlienHelper(3);
            alien.SetPayout(1000);
            alien.ShootMod = 30;
            return alien;
        }

        public SpaceThing BigAsteroid(int x, int y)
        {

            SpaceThing st = new SpaceThing(x, y, new Point(sg.ScreenDim.X, sg.ScreenDim.Y));
            st.SetSpawner(new LargeSpawnStrategy()); // THIS LINE DIFFERENT
            st.SetRotationSpeed(rand.NextDouble() * 6 -3);

            int numPoints = 9 + rand.Next() % 6;
            int i;
            Point UnitZero = new Point(1.0, 0.0); // a unit vector at 0 degrees
            Matrix m = (new Matrix());
            Point p;
            double scalar;

            st.SetMoving(RandomVector(2.1));//THIS LINE IS DIFFERENT
            st.SetPayout(20);//THIS LINE IS DIFFERENT

            for (i = 0; i < numPoints; i++)
            {
                scalar = (rand.NextDouble() * 30 + 12);
                p = Point.Multiply(UnitZero, m);
                p.X *= scalar;
                p.Y *= scalar;
                st.AddVert(p);
                m.Rotate(360.0 / numPoints);
            }
            
            return st;
        }

        public SpaceThing MediumAsteroid(int x, int y)
        {

            SpaceThing st = new SpaceThing(x, y, new Point(sg.ScreenDim.X, sg.ScreenDim.Y));
            st.SetSpawner(new MediumSpawnStrategy()); // THIS LINE DIFFERENT
            st.SetRotationSpeed(rand.NextDouble() * 6 - 3);

            int numPoints = 9 + rand.Next() % 6;
            int i;
            Point UnitZero = new Point(1.0, 0.0); // a unit vector at 0 degrees
            Matrix m = (new Matrix());
            Point p;
            double scalar;

            st.SetMoving(RandomVector(2.7));//THIS LINE IS DIFFERENT
            st.SetPayout(50);//THIS LINE IS DIFFERENT

            for (i = 0; i < numPoints; i++)
            {
                scalar = (rand.NextDouble() * 20 + 9);   // THIS LINE IS THE ONLY ONE DIFFERENT
                p = Point.Multiply(UnitZero, m);
                p.X *= scalar;
                p.Y *= scalar;
                st.AddVert(p);
                m.Rotate(360.0 / numPoints);
            }

            return st;
        }

        public SpaceThing SmallAsteroid(int x, int y)
        {

            SpaceThing st = new SpaceThing(x, y, new Point(sg.ScreenDim.X, sg.ScreenDim.Y));
            st.SetRotationSpeed(rand.NextDouble() * 6 - 3);

            int numPoints = 9 + rand.Next() % 6;
            int i;
            Point UnitZero = new Point(1.0, 0.0); // a unit vector at 0 degrees
            Matrix m = (new Matrix());
            Point p;
            double scalar;

            st.SetMoving(RandomVector(3.4));//THIS LINE IS DIFFERENT
            st.SetPayout(100);//THIS LINE IS DIFFERENT

            for (i = 0; i < numPoints; i++)
            {
                scalar = (rand.NextDouble() * 10 + 6);  // THIS LINE IS THE ONLY ONE DIFFERENT
                p = Point.Multiply(UnitZero, m);
                p.X *= scalar;
                p.Y *= scalar;
                st.AddVert(p);
                m.Rotate(360.0 / numPoints);
            }

            return st;
        }
    }
}
