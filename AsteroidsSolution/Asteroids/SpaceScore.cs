using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Asteroids
{
    class SpaceScore
    {
        private int ScoreInternal;
        public int Score { get { return ScoreInternal;  } }
        private string Name;

        public SpaceScore(int score, string name)
        {
            Name = name + "    ";
            ScoreInternal = score;
            Name = Name.Substring(0, 3).ToUpper();
        }

        public SpaceScore(string LineGenerated)
        {
            int i;
            string s = "" + LineGenerated;
            string[] subs = s.Split(' ');
            Name = "" + subs[0];

            for (i = 0; i < subs.Length -1; i++) ;
            ScoreInternal = int.Parse(subs[i]);
            
        }

        public string GetName()
        {
            return Name;
        }

        public override string ToString()
        {
            string s = "";

            s += Name + "   " + Score;

            return s;
        }
    }
}
