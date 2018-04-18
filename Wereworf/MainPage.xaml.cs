using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Wereworf
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            
            this.InitializeComponent();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/aaaa.jpg", UriKind.Absolute));
            //center.Background = imageBrush;
        }

        string[] identity ;

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] identity1 = { "ms-appx:///Assets/pao.jpg", "2", "3", "4", "5","","","","","" };
            identity = new string[10];
            //initialize
            Hashtable hashtable = new Hashtable();
            Random rm = new Random();
            int RmNum = 10;
            for (int j = 0; hashtable.Count < RmNum; )
            {
                int nValue = rm.Next(0,10);
                if (!hashtable.ContainsValue(nValue))
                {
                    hashtable.Add(nValue, nValue);
                    identity[nValue] = identity1[j];
                    j++;
                }
            }
            identity[0] = identity1[0];
            int i = 0;
            int TimeCount = 90;//倒计时秒数
            DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += new EventHandler<object>(async (s, ee) =>
            {
                await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    i += 1;
                    clocktext.Text = ((TimeCount - i) / 60).ToString("00") + ":" + ((TimeCount - i) % 60).ToString("00");
                    if (i == TimeCount)
                        timer.Stop();
                }));
            });
            timer.Start();
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (image1.Tag.ToString() == "1")
            // if(image1.Source== new BitmapImage(new Uri("ms-appx:///Assets/aaaa.jpg", UriKind.Absolute)))
            { image1.Source = new BitmapImage(new Uri(identity[0], UriKind.Absolute)); image1.Tag = "2"; }
            else
            {
                image1.Source = new BitmapImage(new Uri("ms-appx:///Assets/aaaa.jpg", UriKind.Absolute));
                  image1.IsTapEnabled = false;
                image1.Tag = "1";
            }
        }
    }
}
