using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using EFCore1.Library;
using Microsoft.EntityFrameworkCore;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Wereworf
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class gamePage : Page
    {
        public gamePage()
        {
           this.InitializeComponent();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/gameBack.jpg", UriKind.Absolute));
            timer.Tick += handler;
            OnMyTagChanged += new MyTagChanged(AfterMyValueChanged);
            GameGrid.Background = imageBrush;
        }
        string[] identity ;
        List<string> worf;
        List<string> god;
        List<string> civilian;
        List<Button> dead = new List<Button>();

        //定义委托
        public delegate void MyTagChanged(object sender, EventArgs e);
        //与委托相关联的事件
        public event MyTagChanged OnMyTagChanged;

        DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

        private int timetag = 0;

        private int start = 0;   //开始按钮

        EventHandler<object> handler;

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
                //timetag = value;
            }
        }

       public void changehandler(int TimeCount)
        {
            int i = 0;
             handler= new EventHandler<object>(async (s, ee) =>
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




        private int deadpeople = 0;  //一次狼刀

        private int antidote;    //解药

        private int poison;      //毒药

        private int check=0;    //预言家验人     

        private int vote = 0;


        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(start==1)
            {
                var image = sender as Image;
                int i = int.Parse(image.Name.Substring(5)) - 1;
                if (TimeTag == 0)
                {
                    if (image.Tag.ToString() == "1")
                    // if(image1.Source== new BitmapImage(new Uri("ms-appx:///Assets/aaaa.jpg", UriKind.Absolute)))
                    { image.Source = new BitmapImage(new Uri(identity[i], UriKind.Absolute)); image.Tag = "2"; }
                    else
                    {
                        image.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute));
                        image.IsTapEnabled = false;
                        image.Tag = "1";
                    }
                }

            }

        }

        private void AfterMyValueChanged(object sender, EventArgs e)
        {

            switch(TimeTag)
            {
                case 0: break;
                case 1:
                    GameSession.Text = "天黑请闭眼";
                    explaination.Text = "狼人请杀人";
                    timer.Tick -= handler;
                    changehandler(20);
                    timer.Tick += handler;
                    break;
                case 2:
                    GameSession.Text = "女巫请睁眼";
                    explaination.Text = "女巫判断是否救人";
                    timer.Tick -= handler;
                    changehandler(30);
                    timer.Tick += handler;
                    break;
                  
                case 3:
                    GameSession.Text = "预言家请睁眼";
                    explaination.Text = "预言家验人";
                    timer.Tick -= handler;
                    changehandler(15);
                    timer.Tick += handler;
                    break;
                case 4:
                    GameSession.Text = "天亮了";
                    explaination.Text = "玩家请发言";
                    timer.Tick -= handler;
                    changehandler(22);
                    timer.Tick += handler;
                    break;
                case 5:
                    GameSession.Text = "下面开始放逐投票";
                    explaination.Text = "点击被放逐玩家的头像";
                    timer.Tick -= handler;
                    changehandler(10);
                    timer.Tick += handler;
                    break;

                default:
                        deadpeople = 0;
                        check = 0;
                        vote = 0;
                        TimeTag = 1;
                    
                    break;

            }
            //explaination.Text = TimeTag.ToString();

        }
     
        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if(TimeTag==2)
            {
                if (dead.Count != 0)
                {
                    ((dead.ElementAt(0) as Button).Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/areadydead.jpg", UriKind.Absolute));
                    (dead.ElementAt(0) as Button).IsEnabled = false;
                    string deadname = (int.Parse((dead.ElementAt(0) as Button).Tag.ToString()) - 1).ToString();
                    if (worf.Contains(deadname))
                        worf.Remove(deadname);
                    if (god.Contains(deadname))
                        god.Remove(deadname);
                    if (civilian.Contains(deadname))
                        civilian.Remove(deadname);
                    dead.RemoveAt(0);
                }
                if(JudgeGameOver())
                {
                    timer.Stop();
                    return;
                }
            }
            TimeTag++;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //可以点击图片，重置背景
            player1.IsEnabled = true;
            player2.IsEnabled = true;
            player3.IsEnabled = true;
            player4.IsEnabled = true;
            player5.IsEnabled = true;
            player6.IsEnabled = true;
            player7.IsEnabled = true;
            player8.IsEnabled = true;
            player9.IsEnabled = true;
            player10.IsEnabled = true;
            image1.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image1.Tag = "1"; image1.IsTapEnabled = true;
            image2.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image2.Tag = "1"; image2.IsTapEnabled = true;
            image3.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image3.Tag = "1"; image3.IsTapEnabled = true;
            image4.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image4.Tag = "1"; image4.IsTapEnabled = true;
            image5.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image5.Tag = "1"; image5.IsTapEnabled = true;
            image6.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image6.Tag = "1"; image6.IsTapEnabled = true;
            image7.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image7.Tag = "1"; image7.IsTapEnabled = true;
            image8.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image8.Tag = "1"; image8.IsTapEnabled = true;
            image9.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image9.Tag = "1"; image9.IsTapEnabled = true;
            image10.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute)); image10.Tag = "1"; image10.IsTapEnabled = true;
            //变量初始化
            antidote = 1;
            poison = 1;
            deadpeople = 0;
            vote = 0;
            check = 0;
            TimeTag = 0;

           worf=new List<string>();
             god=new List<string>();
           civilian=new List<string>();


            //随机分配身份
            string[] identity1 = { "ms-appx:///Assets/lr.jpg", "ms-appx:///Assets/lr.jpg", "ms-appx:///Assets/lr.jpg", "ms-appx:///Assets/nw.jpg", "ms-appx:///Assets/yyj.jpg", "ms-appx:///Assets/qiang.jpg", "ms-appx:///Assets/pc.jpg", "ms-appx:///Assets/pc.jpg", "ms-appx:///Assets/pc.jpg" , "ms-appx:///Assets/pc.jpg" };
            identity = new string[10];
            //initialize
            Hashtable hashtable = new Hashtable();
            Random rm = new Random();
            int RmNum = 10;
            for (int j = 0; hashtable.Count < RmNum;)
            {
                int nValue = rm.Next(0, 10);
                if (!hashtable.ContainsValue(nValue))
                {
                    hashtable.Add(nValue, nValue);
                    identity[nValue] = identity1[j];
                    if (j < 3)
                        worf.Add(nValue.ToString());
                    else if (j < 6)
                        god.Add(nValue.ToString());
                    else
                        civilian.Add(nValue.ToString());
                    j++;
                }
            }
         //   identity[0] = identity1[0];

            GameSession.Text = "游戏开始";
            explaination.Text = "请玩家点击头像查验身份";
            timer.Tick -= handler;
            changehandler(30);
            timer.Tick += handler;
            //while (timetag!=1) 
            start = 1;
            EndButton.IsEnabled = true;
        }

        private void player_Click(object sender, RoutedEventArgs e)
        {
            var deadbutton = sender as Button;
            if(TimeTag==5)
            {
                if(vote==0)
                {
                    deadbutton.IsEnabled = false;
                    (deadbutton.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/areadydead.jpg", UriKind.Absolute));
                    string deadname = (int.Parse(deadbutton.Tag.ToString())-1).ToString();
                    if (worf.Contains(deadname))
                        worf.Remove(deadname);
                    if (god.Contains(deadname))
                        god.Remove(deadname);
                    if (civilian.Contains(deadname))
                        civilian.Remove(deadname);
                    if (JudgeGameOver())
                        //  TimeTag = 0;
                        timer.Stop();
                }
                vote++;
               
            }
            //杀人
            if (TimeTag == 1)
            {
                if(deadpeople==0)
                {
                    (deadbutton.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/dead.jpg", UriKind.Absolute));
                    // string deadname = (int.Parse(deadbutton.Tag.ToString()) - 1).ToString();
                    dead.Add(deadbutton);
                    deadpeople++;
                }

            }
            //救人
            if (TimeTag == 2)
            {
                Button button;
               
                if (dead.Count != 0)
                {
                    button = (dead.ElementAt(0) as Button);
                }
                else
                    button = null;

                //救人成功
                if (button == deadbutton)

                {
                    if (antidote == 1)
                    {
                        (button.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute));
                        antidote--;
                        dead.RemoveAt(0);
                    }
                   
                }

                //杀人成功
                else if (poison==1)
                {
                    dead.Add(deadbutton);
                    poison--; 
                }

                int counttemp = dead.Count;
                 for(int i=0;i<counttemp;i++)
                {
                   ( (dead.ElementAt(0) as Button ).Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/areadydead.jpg", UriKind.Absolute));
                    (dead.ElementAt(0) as Button).IsEnabled = false;
       
                    string deadname = (int.Parse((dead.ElementAt(0) as Button).Tag.ToString()) - 1).ToString();
                    if (worf.Contains(deadname))
                        worf.Remove(deadname);
                    if (god.Contains(deadname))
                        god.Remove(deadname);
                    if (civilian.Contains(deadname))
                        civilian.Remove(deadname);
                    dead.RemoveAt(0);
                }
                if (JudgeGameOver())
                    //  TimeTag = 0;
                    timer.Stop();

            }
             if(TimeTag==3)
         {



                var image = (sender as Button).Content as Image;
                if (check==0)
                {
                    
                    int i = int.Parse(image.Name.Substring(5)) - 1;

                    if (image.Tag.ToString() == "1")
                    // if(image1.Source== new BitmapImage(new Uri("ms-appx:///Assets/aaaa.jpg", UriKind.Absolute)))
                    { image.Source = new BitmapImage(new Uri(identity[i], UriKind.Absolute)); image.Tag = "2"; }
                    //else
                    //{
                    //    image.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute));
                    //    image.Tag = "1";
                    //}
                    check++;
                }
                  
                    if (check==2&& image.Tag.ToString() == "2")
                    {

                        image.Source = new BitmapImage(new Uri("ms-appx:///Assets/bj.jpg", UriKind.Absolute));
                        image.Tag = "1";
                    }
                    check++;
             }

        }

        private bool JudgeGameOver()
        {
            if (worf.Count == 0)
            {
                GameSession.Text = "好人胜利";
                explaination.Text = "好人真棒";
                using (var db = new Model())
                {
                    var t = db.User.Where(b => (b.UserName == MainPage.firstpage.UserName.Text)).ToArray();
                    string texttemp = GameSession.Text + "\n" + t.ElementAt(0).CimbatGains;
                    t.ElementAt(0).CimbatGains = texttemp;
                    db.SaveChanges();
                }
                start = 0;
                EndButton.IsEnabled = false;
                return true;
            }
            else if (god.Count == 0 || civilian.Count == 0)
            {  
                GameSession.Text = "狼人胜利";
                explaination.Text = "狼人真厉害";
                using (var db = new Model())
                {
                    var t = db.User.Where(b => (b.UserName == MainPage.firstpage.UserName.Text)).ToArray();
                    string texttemp = GameSession.Text + "\n" + t.ElementAt(0).CimbatGains;
                    t.ElementAt(0).CimbatGains = texttemp;
                    db.SaveChanges();
                }
                    start = 0;
                EndButton.IsEnabled = false;
                return true;
            }
            return false;
        }

    }
}

