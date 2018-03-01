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
using System.Windows.Shapes;

namespace Asteroids
{
    /// <summary>
    /// Interaction logic for ScoreInput.xaml
    /// </summary>
    public partial class ScoreInput : Window
    {
        private string Name;
        public ScoreInput()
        {
            InitializeComponent();
            Name = "";
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ScoreInput1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Name = textBox.Text;
        }

        public string GetName()
        {
            return Name;
        }

        // Used a bit of code from Grant Edwards here.
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Name = textBox.Text;
            if (Name.Trim().Length > 3)
            {
                MessageBox.Show("Must enter a three-letter name");
                Name = "";
                DoneButton.IsEnabled = false;
            }
            else if (Name.Trim().Length == 3)
                DoneButton.IsEnabled = true;
            else
                DoneButton.IsEnabled = false;
        }
    }
}
