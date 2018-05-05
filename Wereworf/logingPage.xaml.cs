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
using Microsoft.EntityFrameworkCore;
using EFCore1.Library;
using System.Threading.Tasks;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Wereworf
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class logingPage : Page
    {
        public logingPage()
        {
            this.InitializeComponent();
        }
        private async void login_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Model())
            {
                string us = u.Text;
                string paa = pa.Password;
                if (us.Length == 0 | paa.Length == 0)
                {
                    all.Text = "请输入";
                    return;
                }
                var theuser = db.User.Where(b => (b.Password == paa & b.UserName == us));
                if (theuser.Count() == 0)
                {
                    //提示用户未注册或输入错误，请注册后登陆
                    all.Text = "您未注册或输入错误，请注册后登陆";
                }
                else
                {
                    //theuser.ElementAt(0).UserName   用户名
                    //theuser.ElementAt(0).Password   密码
                    //theuser.ElementAt(0).Email    邮箱
                    //theuser.ElementAt(0).CimbatGains  战绩
                    //否则已经注册，到游戏页面
                    all.Text = "登陆成功";
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    MyFrame.Navigate(typeof(gamePage));
                }
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(regPage));
        }
        private void MyFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
