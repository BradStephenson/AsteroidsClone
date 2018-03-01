using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Asteroids
{
    class SmallSpawnStrategy : SpaceSpawnStrategy
    {
        public void SpawnChildren(SpaceGame g, Point loc)
        {
            g.CreateDebris(loc);
            g.CreateDebris(loc);
        }
    }
}
