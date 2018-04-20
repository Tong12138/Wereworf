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

        //定义委托
        public delegate void MyTagChanged(object sender, EventArgs e);
        //与委托相关联的事件
        public event MyTagChanged OnMyTagChanged;

       

        private int timetag = 0;

        private void WhenMyTagChange()
        {
            if (OnMyTagChanged != null)

            {

                OnMyTagChanged(this, null);

            }

        }


        public int TimeTag
        {
            get { return timetag; }
            set
            {
                //如果赋的值与原值不同
                if (value != timetag)
                {
                    timetag = value;

                    //就触发该事件!
                    WhenMyTagChange();
                }

                //然后赋值!
                timetag = value;
            }
        }


        //游戏开始
        private  void Button_Click(object sender, RoutedEventArgs e)
        {

            OnMyTagChanged += new MyTagChanged(AfterMyValueChanged);

            //随机分配身份
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

            GameSession.Text = "游戏开始";
            explaination.Text = "请玩家点击头像查验身份";
            TimeCounter(3);
            //while (timetag!=1) ;

            
        }

        private void  TimeCounter(int TimeCount)
        {
            //开始倒计时
            int i = 0;
            DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += new EventHandler<object>(async (s, ee) =>
            {
                await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
                {
                    i += 1;
                    clocktext.Text = ((TimeCount - i) / 60).ToString("00") + ":" + ((TimeCount - i) % 60).ToString("00");
                    if (i == TimeCount)
                    {
                        TimeTag = timetag + 1;
                        timer.Stop();
                        
                    }
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



        private void AfterMyValueChanged(object sender, EventArgs e)
        {

            switch(TimeTag)
            {
                case 1:
                    GameSession.Text = "天黑请闭眼";
                    explaination.Text = "狼人请杀人";
                    TimeCounter(3);
                    break;
                case 2:
                    GameSession.Text = "女巫请睁眼";
                    explaination.Text = "女巫判断是否救人";
                    TimeCounter(3);
                    break;
                case 3:
                    GameSession.Text = "预言家请睁眼";
                    explaination.Text = "预言家验人";
                    TimeCounter(3);
                    break;
                case 4:
                    GameSession.Text = "天亮了";
                    explaination.Text = "玩家请发言";
                    TimeCounter(3);
                    break;
                case 5:
                    GameSession.Text = "下面开始放逐投票";
                    explaination.Text = "投票票拉";
                    TimeCounter(3);
                    break;
                default:
                    TimeTag = 0;
                    break;

            }


            //explaination.Text = TimeTag.ToString();

        }

    }



   


    
}
