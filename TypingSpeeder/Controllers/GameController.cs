using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TypingSpeeder.Engines;
using TypingSpeeder.Model;
using TypingSpeeder.Providers;

namespace TypingSpeeder.Controllers
{
    class GameController
    {
        // TODO: Class has too many responsibilities, need to be splitted

        GameEngine ge = new GameEngine();
        Canvas canvas;

        public event Action GameEnded;

        public double FontSize { get; set; }

        public bool IsWorking { get { return ge.IsRunning; } }

        public GameController(Canvas cnv)
        {
            canvas = cnv;
            ge.NextSampleAdded += addNextTextBlock;
            ge.NextStepMade += moveTextBlocks;
        }

        private void addNextTextBlock(string text)
        {
            canvas.Dispatcher.Invoke(() =>
            {
                TextBlock newTextBlock = new TextBlock();
                canvas.Children.Add(newTextBlock);
                newTextBlock.FontSize = FontSize;
                newTextBlock.Background = Brushes.LightBlue;
                newTextBlock.Text = text;
                newTextBlock.Loaded += (s, e) => NewTextBlock_Loaded(s, e);
            });
        }

        /// <summary>
        /// Generate a random left margin for a TextBlock 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var lenght = ((TextBlock)sender).ActualWidth;
            var left = DataProvider.InitializeDouble() * (canvas.ActualWidth - (lenght));
            ((TextBlock)sender).Margin = new Thickness(left, 0, 0, 0);
            ((TextBlock)sender).SetValue(Canvas.TopProperty, 0.0);
        }

        private void moveTextBlocks(double nextStep)
        {
            bool isGameEnded = false;
            canvas.Dispatcher.Invoke(() =>
            {
                foreach (UIElement tb in canvas.Children)
                {
                    var top = (double)tb.GetValue(Canvas.TopProperty);
                    var height = ((TextBlock)tb).ActualHeight;
                    isGameEnded = checkIfTextReachedEnd(top, height);

                    if (isGameEnded)
                    {
                        ge.Stop();
                        GameEnded();
                        break;
                    }
                    else
                    {
                        top += nextStep;
                        tb.SetValue(Canvas.TopProperty, top);
                    }
                }

                if (isGameEnded)
                {
                    canvas.Children.Clear();
                    TextBlock newLabel = new TextBlock();
                    canvas.Children.Add(newLabel);
                    newLabel.FontSize = 50;
                    newLabel.Text = "You lose!!!!!";
                    var left = canvas.ActualWidth / 3;
                    var top = canvas.ActualHeight / 3;
                    newLabel.Margin = new Thickness(left, top, 0, 0);
                    newLabel.SetValue(Canvas.TopProperty, 0.0);
                }
            });
        }

        private void CountingDown()
        {
            IEnumerator<string> counter = new List<string>() { "3", "2", "1", "GO!" }.GetEnumerator();
            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += (s, e) => Timer_Elapsed(s, e, counter);
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e, IEnumerator counter)
        {
            canvas.Dispatcher.Invoke(() =>
            {
                canvas.Children.Clear();
                TextBlock text = new TextBlock();
                text.FontSize = 50;
                var left = canvas.ActualWidth / 3;
                var top = canvas.ActualHeight / 3;
                text.Margin = new Thickness(left, top, 0, 0);
                if (counter.MoveNext())
                {
                    text.Text = counter.Current.ToString();
                    canvas.Children.Add(text);
                }
                else
                {
                    ((Timer)sender).Stop();
                    ge.Start();
                }
            });
        }

        private bool checkIfTextReachedEnd(double top, double tbHeight)
        {
            return canvas.ActualHeight <= (top + tbHeight);
        }

        public bool ValidateAnswer(string answer)
        {
            if (canvas.Children.Count > 0)
            {
                string value = ((TextBlock)canvas.Children[0]).Text;
                return ge.ValidateValue(answer, value);
            }
            return false;
        }

        public void RemoveFirstTextBlock()
        {
            canvas.Dispatcher.Invoke(() =>
            {
                canvas.Children.RemoveAt(0);
            });

            if (canvas.Children.Count == 0)
            {
                ge.InvokeNextTextSample();
            }
        }

        public void SelectMode(Level level)
        {
            if (!ge.IsRunning)
                ge.SetGameMode(level);
        }

        public void SelectTextType(TextType textType)
        {
            if (!ge.IsRunning)
                ge.SetGameTextType(textType);
        }

        public void ChangeBackground(Control control, string backgroudPath)
        {
            var imageSource = new BitmapImage(new Uri(backgroudPath, UriKind.Relative));
            control.Background = new ImageBrush(imageSource);
        }

        public void Start()
        {
            if (canvas.Children.Count == 0)
            {
                CountingDown();
            }
            DataProvider.Reset();
        }

        public void Stop()
        {
            ge.Stop();
        }
    }
}
