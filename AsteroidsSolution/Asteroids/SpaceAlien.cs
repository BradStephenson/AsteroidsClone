using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Asteroids
{



    // getting close to time to tun in, using a bit of bad oo here
    class SpaceAlien : SpaceThing
    {
        Random rand;
        public int ShootMod { get; set; }
        public SpaceAlien(Point worldSize, int x, int y)
                : base(x, y, worldSize)
        {
            rotationMatrix = new Matrix();
            rand = new Random();
            moving = RandomVector(2.0);
            ShootMod = 60;
        }


        public override void Simulate(SpaceGame g)
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

            if (rand.Next() % 40 == 0)
                moving = RandomVector(2.0);

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

        public void Shoot(SpaceGame g, Point PlayerLocation)
        {
            if (rand.Next() % ShootMod == 0)
            {
                Vector v = RandomVector(16.0);
                SpaceBullet b = new SpaceBullet(upperLeft + (10 * v), g.ScreenDim);
                b.SetMoving(v);
                g.AddBullet(b);
            }
        }

        public override void Blast(SpaceGame g)
        {
            //base.Blast(g);
            g.RemoveFromAliens(this);

            g.CreateDebris(upperLeft);
            g.CreateDebris(upperLeft + RandomVector(5.0));
            g.CreateDebris(upperLeft + RandomVector(5.0));
            g.CreateDebris(upperLeft + RandomVector(5.0));
            //Spawner.SpawnChildren(g, upperLeft);
            g.AddScore(ScorePayout);
        }
    }
}
