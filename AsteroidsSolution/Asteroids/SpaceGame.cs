using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    class SpaceGame
    {
        public bool PlayerTurningLeft      { set{
                Player.SetRotationSpeed(0.0);
                if (value)
                    Player.SetRotationSpeed(-12.0);
                //PlayerTurningLeft = value;
            }
        }
        public bool PlayerTurningRight
        {
            set
            {
                Player.SetRotationSpeed(0.0);
                if (value)
                    Player.SetRotationSpeed(12.0);
                //PlayerTurningLeft = value;
            }
        }
        private int scoreInternal;
        public int Score
        {
            get { return scoreInternal; }
            set
            {
                scoreInternal = value;
                this.ScoreLabel.Content = "" + value;

                if (Score > LivesScoreThreshold)
                {
                    Lives++;
                    LivesScoreThreshold -= LivesScoreThreshold % 10000;
                    LivesScoreThreshold += 10000;
                }

                NotifyScoreChanged("score changed");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyScoreChanged(string name)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        public bool PlayerThrusting { set { Player.Thrusting = value; } }

        public string NameGotten;
        protected bool gameLoopRunning;
        protected long dt;
        protected Canvas view;
        protected SpaceFactory factory;

        private int level;

        public Point ScreenDim;

        private int Lives;

        private int LivesScoreThreshold;
        

        protected int Counter;
        protected int PlayerTimer;

        protected bool stopped;
        public event EventHandler GameOver;
        protected Label ScoreLabel;

        protected SpaceShip Player;
        public List<Blastable> Enemies;

        public List<Blastable> ToBeBlasted;

        public List<Blastable> Debris;

        public List<Blastable> Bullets;

        public List<SpaceAlien> Aliens;

        public SpaceGame(Canvas canvas, Label scoreLabel, double x, double y)
        {
            this.ScreenDim = new Point(x, y);
            //MessageBox.Show(ScreenDim + "");
            Counter = 0;
            //this.ScreenDim = new Point(400, 400);
            this.ScoreLabel = scoreLabel;
            gameLoopRunning = true;
            view = canvas;
            Score = 0;
            dt = 10000;
            Enemies = new List<Blastable>();
            ToBeBlasted = new List<Blastable>();
            Debris = new List<Blastable>();
            Bullets = new List<Blastable>();
            Aliens = new List<SpaceAlien>();
            factory = new SpaceFactory(this);
            Lives = 3;
            TimeInPlayer(30);
            level = 6;
            StartLevel(8);
            LivesScoreThreshold = 5000;
            GameLoop();
        }

        public void StartLevel(int num)
        {
            int i;
            Enemies.Clear();
            Bullets.Clear();
            Debris.Clear();

            for (i = 0; i < num/2; i++)
                Enemies.Add(factory.BigAsteroid(i * 150 + 100, 90));
            for (; i < num; i++)
                Enemies.Add(factory.BigAsteroid(50,i * 120 + 90));
        }

        public virtual async void GameLoop()
        {
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
                {
                    a.Simulate(this);
                    a.Shoot(this, Player.GetLocation());
                }

                if ((((Counter * 50000000 + 345) * (Counter + 55)) % 125 == 0)&&Aliens.Count < 2)
                    Aliens.Add(factory.GiveMeAnAlien(Score));
                


                CollideAsteroidsAliensBullets();


                if (PlayerTimer < 0)
                    CollidePlayerAndAsteroidsAndBullets();



                await Task.Delay(1);
                //if (Counter == 3)
                    //MessageBox.Show("hullo");
                
                PlayerTimer--;
                view.Children.Clear();

                // Player Draw
                if (PlayerTimer < 0)
                    Player.Draw(view);
                else
                    if (((Counter / 5) % 2 == 0)/*&&(Counter < 50)*/)
                        Player.Draw(view);
                
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

                // LIVES
                Point p;
                int i;

                for (i = 0; i < Lives; i++)
                {
                    p = new Point(60 + i*30, 60);
                    view.Children.Add(SpaceThing.MakeLine((new Point(0, -25)), (new Point(10, 9)), p, new Matrix()));
                    view.Children.Add(SpaceThing.MakeLine((new Point(0, -25)), (new Point(-10, 9)), p, new Matrix()));
                    view.Children.Add(SpaceThing.MakeLine((new Point(-8, 2)), (new Point(8, 2)), p, new Matrix()));
                }
                


                // END LIVES

                Player.Simulate(this);
                //Player.Draw(view);

                HandleBlasts();
            }
            

            //if (Counter > 50)
            //MessageBox.Show(time + "");
            await Task.Delay(1);
            time = DateTime.Now.Ticks - time;
            dt = 60 - (int)(time / 10000);
            if (dt > 0)
                await Task.Delay(60 - (int)(time/10000));
            if (Enemies.Count < 1)
                StartLevel((int)Math.Sqrt(Score/600) + 6);
            if (!stopped)
                GameLoop();
        }

        protected void CollidePlayerAndAsteroidsAndBullets()
        {
            foreach (Blastable b in Bullets)
                if ((Player).Collide((SpaceThing)b))
                {
                    Blast(Player);
                    Blast(b);
                    break;
                }

            foreach (Blastable e in Enemies)
                if ((Player).Collide((SpaceThing)e))
                {
                    Blast(Player);
                    Blast(e);
                    break;
                }
        }

        protected void CollideAsteroidsAliensBullets()
        {
            foreach (Blastable e in Enemies)
            {
                foreach (SpaceAlien sa in Aliens)
                    if (((SpaceThing)e).Collide(sa))
                    {
                        Blast(sa);
                        Blast(e);
                    }
            }

            foreach (Blastable b in Bullets)
            {
                foreach (SpaceAlien sa in Aliens)
                    if (sa.Collide((SpaceThing)b))
                    {
                        Blast(sa);
                        Blast(b);
                        break;
                    }
                foreach (Blastable e in Enemies)
                    if (((SpaceThing)e).Collide((SpaceThing)b))
                    {
                        Blast(e);
                        Blast(b);
                        break;
                    }
            }
        }

        public void PauseUnpause()
        {
            gameLoopRunning = !gameLoopRunning;
        }

        public void SpawnMediums(Point loc)
        {
            Enemies.Add(factory.MediumAsteroid((int)loc.X, (int)loc.Y));
            Enemies.Add(factory.MediumAsteroid((int)loc.X, (int)loc.Y));
        }

        public void SpawnSmalls(Point loc)
        {
            Enemies.Add(factory.SmallAsteroid((int)loc.X, (int)loc.Y));
            Enemies.Add(factory.SmallAsteroid((int)loc.X, (int)loc.Y));
        }

        public void PlayerShoot()
        {
            Player.Shoot(this);
        }

        public void GainLife()
        {
            Lives++;
        }

        public void LoseLife()
        {
            Lives--;
            if (Lives < 1)
                OnGameOver(EventArgs.Empty);
        }

        public void TimeInPlayer(int time)
        {
            PlayerTimer = time;
            Player = new SpaceShip(new Point(500, 500), ScreenDim);

        }

        public void CreateDebris(Point loc)
        {
            foreach (SpaceDebris d in factory.DebrisCloud(loc))
            {
                Debris.Add(d);
            }
        }

        public virtual void StopGame()
        {
            stopped = true;
        }

        public void AddBullet(SpaceBullet b)
        {
            Bullets.Add(b);
            //MessageBox.Show("" + Bullets.Count);
        }

        public void Blast(Blastable b)
        {
            ToBeBlasted.Add(b);
        }

        public void RemoveFromLists(Blastable b)
        {
            /*if (Bullets.Remove(b))
                return;
            if (Enemies.Remove(b))
                return;
            if (Debris.Remove(b))
                return;
            //Aliens.Remove((SpaceAlien)b);*/

            Bullets.Remove(b);
            Enemies.Remove(b);
            Debris.Remove(b);
        }

        public void RemoveFromAliens(SpaceAlien a)
        {
            Aliens.Remove(a);
        }

        public void RemoveFromEnemies(Blastable enemy)
        {
            Enemies.Remove(enemy);
        }

        public void RemoveFromDebris(Blastable debris)
        {
            Debris.Remove(debris);
        }

        protected void HandleBlasts()
        {
            foreach (Blastable st in ToBeBlasted)
                st.Blast(this);
            ToBeBlasted.Clear();
        }

        public void AddScore(int scoreToAdd)
        {
            if (scoreToAdd > 0)
                Score += scoreToAdd;
        }

        protected virtual void OnGameOver(EventArgs e)
        {
            //Window ScoreBox = new Window();
            //TextBox t = new TextBox();
            ScoreInput wind = new ScoreInput();
            wind.ShowDialog();
            NameGotten = wind.GetName();

            //MessageBox.Show("Game over");

           // ScoreBox.ShowDialog();


            EventHandler handler = GameOver;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        
    }
}
