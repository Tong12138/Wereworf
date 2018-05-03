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
using EFCore1.Library;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Wereworf
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class regPage : Page
    {
        public regPage()
        {
            this.InitializeComponent();
        }
        private async void checkIn_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Model())
            {
                string us = u.Text;
                string paa = pa.Password;
                string ema = em.Text;
                var student = db.User.Where(b => b.Email == ema);
                if (student.Count() > 0)
                {
                    //在组件上提示邮箱已经被注册
                    all.Text = "邮箱已经被注册";
                }
                else
                {
                    User n = new User
                    {
                        UserName = u.Text,
                        Password = pa.Password,
                        Email = em.Text,
                        CimbatGains = "0"
                    };
                    db.User.Add(n);
                    await db.SaveChangesAsync();
                    all.Text = "注册成功";
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    MyFrame.Navigate(typeof(logingPage));
                }
            }
        }
    }
}
