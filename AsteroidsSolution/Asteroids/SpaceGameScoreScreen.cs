using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Asteroids
{
    class SpaceGameScoreScreen : SpaceGame
    {
        SpaceScore[] Scores;
        FileStream fs;
        StreamReader Reader;
        StreamWriter Writer;

        public SpaceGameScoreScreen(Canvas c, Label l, double x, double y, SpaceScore score)
            : base(c, l, x, y)
        {
            int i;
            Scores = new SpaceScore[11];
            for (i = 0; i < 11; i++)
                Scores[i] = new SpaceScore(50, "ERR");
            Scores[10] = new SpaceScore(score.Score, score.GetName() );
            //Scores[0] = new SpaceScore(score.Score, score.GetName());
            fs = new FileStream("highscores.ast", FileMode.OpenOrCreate);
            Reader = new StreamReader(fs);

            i = 0;
            while(Reader.Peek() != -1)
            {
                Scores[i] = new SpaceScore(Reader.ReadLine());
                i++;
            }
            for (; i<10;i++)
                Scores[i] = new SpaceScore(42, "BPS");
            Reader.Close();
            fs.Close();

            //SpceScores are really C#y. I like them. 
            Scores = Scores.OrderBy(s => -s.Score).ToArray();  // Trying lambdas they're awesome.


            this.ScreenDim = new Point(x, y);
            //MessageBox.Show(ScreenDim + "");
            Counter = 0;
            //this.ScreenDim = new Point(400, 400);
            this.ScoreLabel.Content = "";
            gameLoopRunning = true;
            Score = 0;
            dt = 10000;
            Enemies = new List<Blastable>();
            ToBeBlasted = new List<Blastable>();
            Debris = new List<Blastable>();
            Bullets = new List<Blastable>();
            factory = new SpaceFactory(this);
            //Lives = 3;
            //TimeInPlayer(30);
            //level = 6;
            StartLevel(8);
            GameLoop();
        }

        public async override void GameLoop()
        {
            int i;
            Label l;


            long time = DateTime.Now.Ticks;
            int dt;
            Counter++;

            if (gameLoopRunning)
            {
                foreach (SpaceThing thing in Enemies)
                {
                    thing.Simulate(this);

                }
                foreach (Blastable debris in Debris)
                    debris.Simulate(this);
                foreach (Blastable bullet in Bullets)
                    bullet.Simulate(this);
                foreach (SpaceAlien a in Aliens)
                    a.Simulate(this);



                if (((Counter * 50000000 + 345) * (Counter + 55)) % 125 == 0)
                    Aliens.Add(factory.BigAlien());
                foreach (SpaceAlien a in Aliens)
                {
                    a.Shoot(this, Player.GetLocation());


                }

                CollideAsteroidsAliensBullets();





                await Task.Delay(1);
                //if (Counter == 3)
                //MessageBox.Show("hullo");

                PlayerTimer--;
                view.Children.Clear();
                

                foreach (Blastable thing in Enemies)
                {
                    thing.Draw(view);

                }
                foreach (Blastable debris in Debris)
                    debris.Draw(view);
                foreach (Blastable bullet in Bullets)
                    bullet.Draw(view);

                foreach (SpaceAlien a in Aliens)
                    a.Draw(view);

                l = new Label();


                l.Margin = new Thickness(420, 280, 0, 0);
                l.Content = "ASTEROIDS";
                //l.FontStretch = FontStretches.ExtraCondensed;
                l.Foreground = Brushes.White;
                l.FontSize = 100;
                //l.FontFamily = new FontFamily("");
                l.FontFamily = new FontFamily("Lucida Console" );
                l.FontStretch = FontStretches.ExtraCondensed;
                //l.FontWeight = FontWeights.Bold;


                view.Children.Add(l);




                l = new Label();


                l.Margin = new Thickness(180, 80 , 0, 0);
                l.Content = "HIGH SCORES";
                l.Foreground = Brushes.White;
                l.FontSize = 25;
                l.FontFamily = new FontFamily("Courier");

                //l.FontWeight = FontWeights.Bold;


                view.Children.Add(l);



                for (i = 0; i < 10; i++)
                {
                    l = new Label();


                    l.Margin = new Thickness(190, 150 + i * 60, 0, 0);
                    l.Content = Scores[i].ToString();
                    l.Foreground = Brushes.White;
                    l.FontSize = 15;
                    l.FontFamily = new FontFamily("Courier");

                    l.FontWeight = FontWeights.Bold;


                    view.Children.Add(l);



                }





                //Player.Draw(view);

                HandleBlasts();
            }


            //if (Counter > 50)
            //MessageBox.Show(time + "");
            await Task.Delay(1);
            time = DateTime.Now.Ticks - time;
            dt = 60 - (int)(time / 10000);
            if (dt > 0)
                await Task.Delay(60 - (int)(time / 10000));
            if (Enemies.Count < 1)
                StartLevel(7);





            

            if (!stopped)
                GameLoop();
        }

        public override void StopGame()
        {
            int i;
            fs = new FileStream("highscores.ast", FileMode.Create);
            Writer = new StreamWriter(fs);
            for (i = 0; i < 10; i++)
                Writer.Write(Scores[i].ToString() + Environment.NewLine);
            Writer.Close();
            fs.Close();
            stopped = true;
        }
    }
}
