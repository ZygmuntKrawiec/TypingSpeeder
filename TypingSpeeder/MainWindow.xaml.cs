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
using TypingSpeeder.Controllers;
using TypingSpeeder.Model;

namespace TypingSpeeder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string textToValidate = string.Empty;        
        GameController tbManipulator;

        public MainWindow()
        {
            InitializeComponent();
            tbManipulator = new GameController(cvsGameBoard);
            tbManipulator.FontSize = 20;
            tbManipulator.GameEnded += () =>
            {   // TODO: Remove this ugly looking code from constructor.
                var imageSource = new BitmapImage(new Uri(@"../../Images/btnStart.png", UriKind.Relative));
                btnStartStop.Background = new ImageBrush(imageSource);
            };
        }

        private void txtEnterAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            // If user hit an enter key
            if (e.Key == Key.Enter && tbManipulator.IsWorking)
            {
                var answer = txtEnterAnswer.Text;
                txtEnterAnswer.Clear();
                // Check if answer given by user is correct.
                bool answerResult = tbManipulator.ValidateAnswer(answer);
                // If answer is correct then remove first element from data collection
                if (answerResult)
                {
                    tbManipulator.RemoveFirstTextBlock();
                    txtPoints.Text = ScoreController.Point().ToString();
                }
                else
                {
                    txtMisses.Text = ScoreController.Missed().ToString();
                }
            }
        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            string imagePath;

            if (!tbManipulator.IsWorking)
            {
                cvsGameBoard.Children.Clear();
                tbManipulator.Start();
                imagePath = @"../../Images/btnStop.png";
                txtEnterAnswer.Focus();
                txtPoints.Text = "0";
                txtMisses.Text = "0";
                ScoreController.Reset();
            }
            else
            {
                tbManipulator.Stop();
                imagePath = @"../../Images/btnStart.png";
            }

            var imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            btnStartStop.Background = new ImageBrush(imageSource);
        }

        private void mitExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void mitEasy_Click(object sender, RoutedEventArgs e)
        {
            tbManipulator.SelectMode(Level.Easy);
            tbManipulator.ChangeBackground(lblLevelOption, @"../../Images/btnEasy.png");
        }

        private void mitNormal_Click(object sender, RoutedEventArgs e)
        {
            tbManipulator.SelectMode(Level.Normal);
            tbManipulator.ChangeBackground(lblLevelOption, @"../../Images/btnNormal.png");
        }

        private void mitHard_Click(object sender, RoutedEventArgs e)
        {
            tbManipulator.SelectMode(Level.Hard);
            tbManipulator.ChangeBackground(lblLevelOption, @"../../Images/btnHard.png");
        }

        private void mitNumbers_Click(object sender, RoutedEventArgs e)
        {
            tbManipulator.SelectTextType(TextType.Numbers);
            tbManipulator.ChangeBackground(lblTextOption, @"../../Images/btnNumbers.png");
        }

        private void mitText_Click(object sender, RoutedEventArgs e)
        {
            tbManipulator.SelectTextType(TextType.Words);
            tbManipulator.ChangeBackground(lblTextOption, @"../../Images/btnWords.png");
        }
    }
   
   
}
