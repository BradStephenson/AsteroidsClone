using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Asteroids
{
    interface Blastable
    {
        void Blast(SpaceGame g);

        void Draw(Canvas c);

        void Simulate(SpaceGame g);
        
    }
}
