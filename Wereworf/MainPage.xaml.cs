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
        public static MainPage firstpage;

        public MainPage()
        {

            this.InitializeComponent();
            firstpage = this;
        }
        private void NavView_SelectionChanged(
            Windows.UI.Xaml.Controls.NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                MyFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                NavigationViewItem item =
                    args.SelectedItem as NavigationViewItem;

                switch (item.Tag)
                {
                    case "loging":
                        MyFrame.Navigate(typeof(logingPage));
                        break;

                    case "games":
                        MyFrame.Navigate(typeof(gamePage));
                        break;

                 
                }
            }
        }

       
    }
    
}
