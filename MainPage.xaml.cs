using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp2.Resources;
using ImageTools;
using ImageTools.Controls;
using ImageTools.IO;
using ImageTools.IO.Gif;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PhoneApp2
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        Image mosq, obama, obama2, obama3, obama4;
        TimeSpan spawn_time, anim_time;
        int score, bites;
        int t=100;
        int state = 1;
        
        public MainPage()
        {
            if (state == 1)
            {
                InitializeComponent();
                spawn_time = TimeSpan.FromMilliseconds(1500);
                anim_time = TimeSpan.FromMilliseconds(2000);
                // Sample code to localize the ApplicationBar
                //BuildLocalizedApplicationBar();
                score = 0;
                bites = 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logo.Visibility = Visibility.Collapsed;
                tbl1.Visibility = Visibility.Visible;
                tbl2.Visibility = Visibility.Visible;
                insbtn.Visibility = Visibility.Collapsed;
                soundbtn.Visibility = Visibility.Collapsed;
                Canvas cv1 = new Canvas();
                obama = new Image();
                obama.Source = new BitmapImage(new Uri("/Assets/obama.png", UriKind.RelativeOrAbsolute));
                obama.Width = 100;
                obama.Height = 132;
                Canvas.SetLeft(obama, 320);
                Canvas.SetTop(obama, 350);
                cv1.Children.Add(obama);
                this.LayoutRoot.Children.Add(cv1);

                Canvas cv2 = new Canvas();
                obama2 = new Image();
                obama2.Source = new BitmapImage(new Uri("/Assets/obama2.png", UriKind.RelativeOrAbsolute));
                obama2.Width = 100;
                obama2.Height = 132;
                Canvas.SetLeft(obama2, 320);
                Canvas.SetTop(obama2, 350);
                cv2.Children.Add(obama2);
                this.LayoutRoot.Children.Add(cv2);
                obama2.Visibility = Visibility.Collapsed;

                Canvas cv3 = new Canvas();
                obama3 = new Image();
                obama3.Source = new BitmapImage(new Uri("/Assets/obama3.png", UriKind.RelativeOrAbsolute));
                obama3.Width = 100;
                obama3.Height = 132;
                Canvas.SetLeft(obama3, 320);
                Canvas.SetTop(obama3, 350);
                cv3.Children.Add(obama3);
                this.LayoutRoot.Children.Add(cv3);
                obama3.Visibility = Visibility.Collapsed;

                Canvas cv4 = new Canvas();
                obama4 = new Image();
                obama4.Source = new BitmapImage(new Uri("/Assets/obama4.png", UriKind.RelativeOrAbsolute));
                obama4.Width = 100;
                obama4.Height = 132;
                Canvas.SetLeft(obama4, 320);
                Canvas.SetTop(obama4, 350);
                cv4.Children.Add(obama4);
                this.LayoutRoot.Children.Add(cv4);
                obama4.Visibility = Visibility.Collapsed;

                Task refreshTask = PeriodicallyRefreshDataAsync(spawn_time);
                startbtn.Visibility = Visibility.Collapsed;
            
        }
        private async Task startGame()
        {
            if (state == 1)
            {
                if (bites == 10) GameOver();
                if (bites == 3)
                {
                    obama.Visibility = Visibility.Collapsed;
                    obama2.Visibility = Visibility.Visible;
                }
                if (bites == 6)
                {
                    obama2.Visibility = Visibility.Collapsed;
                    obama3.Visibility = Visibility.Visible;
                }
                if (bites == 8)
                {
                    obama3.Visibility = Visibility.Collapsed;
                    obama4.Visibility = Visibility.Visible;
                }
                if (score > t)
                {
                    t += 100;
                    anim_time = anim_time.Subtract(TimeSpan.FromMilliseconds(200));
                    spawn_time = spawn_time.Subtract(TimeSpan.FromMilliseconds(150));
                }
                int screen_width = (int)Application.Current.Host.Content.ActualWidth;
                int screen_height = (int)Application.Current.Host.Content.ActualHeight;
                Random rnd = new Random();
                int i = rnd.Next(-screen_width, screen_width);
                int j = rnd.Next(-screen_height, screen_height);
                mosq = spawnNMove(i, j, screen_width, screen_height);
                Task k = deleteMosq(mosq);
            }
        }

        private async Task deleteMosq(Image mosq)
        {
            
            
            await Task.Delay(anim_time);
            if (!mosq.Visibility.Equals(Visibility.Collapsed))
            {
                if(state==1)
                bites++;
                bits.Text = bites.ToString();
            }
            mosq.Visibility = Visibility.Collapsed;
        
        }
        private async Task PeriodicallyRefreshDataAsync(TimeSpan period)
        {
            if (state==1)
            {
                while (true)
                {
                    await startGame();
                    await Task.Delay(spawn_time);
                }
            }
        }

        public Image spawnNMove(int i, int j, int w, int h)
        {
            Canvas cv = new Canvas();
            cv.Width = 70;
            cv.Height = 87;
            Image mosquito = new Image();
            mosquito.Source = new BitmapImage(new Uri("/Assets/mosquito.gif", UriKind.RelativeOrAbsolute));
            mosquito.MaxWidth = 70;
            mosquito.MaxHeight = 87;
            //mosquito.Margin = new Thickness(i, 0, 0, 0);
            Canvas.SetLeft(mosquito, i);
            Canvas.SetTop(mosquito, -10);
            cv.Children.Add(mosquito);
            this.LayoutRoot.Children.Add(cv);
            moveTo(mosquito, i, 0, 0, 420);
            mosquito.Tap += mosquito_tap;
            mosquito.DoubleTap += mosquito_tap;
            return mosquito;
        }

        protected void mosquito_tap(object sender, EventArgs e)
        {
            if (sender is Image)
            {
                bleep.Stop();
                bleep.Play();
                Image im = (Image)sender;
                im.Visibility = Visibility.Collapsed;
                score += 10;
                scor.Text = score.ToString();
                bits.Text = bites.ToString();
            }
        }

        public void moveTo(Image target, double startX, double startY, double endX, double endY)
        {
            Duration duration = new Duration(anim_time);

            // Create two DoubleAnimations and set their properties.
            DoubleAnimation myDoubleAnimation1 = new DoubleAnimation();
            DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();

            myDoubleAnimation1.Duration = duration;
            myDoubleAnimation2.Duration = duration;

            Storyboard sb = new Storyboard();
            sb.Duration = duration;

            sb.Children.Add(myDoubleAnimation1);
            sb.Children.Add(myDoubleAnimation2);

            Storyboard.SetTarget(myDoubleAnimation1, target);
            Storyboard.SetTarget(myDoubleAnimation2, target);
            
            // Set the attached properties of Canvas.Left and Canvas.Top
            // to be the target properties of the two respective DoubleAnimations.
            Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath("(Canvas.Top)"));

            myDoubleAnimation1.To = endX;
            myDoubleAnimation2.To = endY;
            // Make the Storyboard a resource.
            //this.LayoutRoot.ContentPanel.Resources.Add("unique_id", sb);

            // Begin the animation.
            sb.Begin();
            if (target.Margin.Left == endX && target.Margin.Top == endY)
            {
                target.Visibility = Visibility.Collapsed;
            }
        }

        private void GameOver()
        {
            state = 0;
            bits.Text = "10";
            NavigationService.Navigate(new Uri("/GamePage.xaml",UriKind.RelativeOrAbsolute));
            
        }


        private void soundbtn_Click(object sender, RoutedEventArgs e)
        {
            if(soundbtn.Content.Equals("Sounds On"))
            {
                soundbtn.Content = "Sounds Off";
                bleep.Volume = 0;
            }
            else
            {
                soundbtn.Content = "Sounds On";
                bleep.Volume = 1;
            }
        }

        private void insbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page2.xaml",UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}