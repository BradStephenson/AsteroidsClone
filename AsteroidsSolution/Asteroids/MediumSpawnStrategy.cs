using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Asteroids
{
    class MediumSpawnStrategy : SpaceSpawnStrategy
    {
        public void SpawnChildren(SpaceGame g, Point loc)
        {
            g.SpawnSmalls(loc);
        }
    }
}
