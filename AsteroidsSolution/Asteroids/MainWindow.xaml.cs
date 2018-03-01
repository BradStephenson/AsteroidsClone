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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpaceLine l;
        //Line line;
        SpaceGame g;
        public MainWindow()
        {
            InitializeComponent();
            g = new SpaceGameScoreScreen(SpaceCanvas, ScoreLabel, SpaceCanvas.Width, SpaceCanvas.Height, new SpaceScore(30, "ASS"));
            //g = new Asteroids.SpaceGame(SpaceCanvas, ScoreLabel, SpaceCanvas.Width, SpaceCanvas.Height);
            //SpaceLine l;
            //this.DataContext = g.Score;
            //ScoreLabel.DataContext = g.Score;

            g.GameOver += g_GameOver;

            //MessageBox.Show("Got here");
        }

        private void SpaceCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        

        private void SpaceCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("" + e.Key);
        }

        private void SpaceCanvas_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                g.PlayerTurningRight = true;
            if (e.Key == Key.Left)
                g.PlayerTurningLeft = true;
            if (e.Key == Key.Up)
                g.PlayerThrusting = true;
            if (e.Key == Key.Space)
                g.PlayerShoot();
            //MessageBox.Show("" + e.Key);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                g.PlayerTurningRight = false;
            if (e.Key == Key.Left)
                g.PlayerTurningLeft = false;
            if (e.Key == Key.Up)
                g.PlayerThrusting = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            g.PauseUnpause();
        }

        private void g_GameOver(object sender, EventArgs e)
        {
            int sc = g.Score;
            string name = g.NameGotten;
            MessageBox.Show("Your Score: " + sc);
            g.StopGame();
            g = null;
            g = new SpaceGameScoreScreen(SpaceCanvas, ScoreLabel, SpaceCanvas.Width, SpaceCanvas.Height, new SpaceScore(sc, name));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            g.StopGame();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is Asteroids as the coder, Brad Stephenson remembers it." + Environment.NewLine
                + "the differences are that the small ship doesn't aim at the player, the  " + Environment.NewLine
                + "asteroids are randomized, and they rotate." + Environment.NewLine 
                + ".NET framework 4.5 in c#");
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Use the arrow keys to move the ship. It has no reverse. Use space bar to shoot. There is no drag in space.");
        }

        private void NewGameItem_Click(object sender, RoutedEventArgs e)
        {
            g.StopGame();
            g = null;
            g = new Asteroids.SpaceGame(SpaceCanvas, ScoreLabel, SpaceCanvas.Width, SpaceCanvas.Height);
            //SpaceLine l;
            //this.DataContext = g.Score;
            //ScoreLabel.DataContext = g.Score;
            g.GameOver += g_GameOver;
        }
    }
}
