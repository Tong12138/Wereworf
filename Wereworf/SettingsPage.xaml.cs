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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Wereworf
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public static SettingsPage firspage;

        public SettingsPage()
        {
            this.InitializeComponent();
            using (var db = new Model())
            {
                var theuser = db.User.Where(b => (b.UserName == MainPage.firstpage.UserName.Text)).ToArray();
                if(theuser.Length > 0)
                {
                    SettingPageUserName.Text = theuser.ElementAt(0).UserName;
                    SettingPageUserMail.Text = theuser.ElementAt(0).Email;
                    HistoryText.Text = theuser.ElementAt(0).CimbatGains;
                }


            }
        }
    }
}
