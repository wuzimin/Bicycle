using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Bicycle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.DarkGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.DarkGreen;
        }
        public string username = "";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try //得到用户信息
            {
                username = e.Parameter.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void buy_button(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Buy), username);
        }

        private void sell_button(object sender, RoutedEventArgs e) //跳转到卖车界面，卖车需要先注册登录
        {
            if (username == "")
            {
                var i = new MessageDialog("请先登陆！").ShowAsync();
                Frame.Navigate(typeof(Sign_in));
            }
            else
            {
                Frame.Navigate(typeof(Sell), username);
            }
        }

        private void account_button(object sender, RoutedEventArgs e) //个人中心
        {
            //if (username == "")
            //{
            //    Frame.Navigate(typeof(Sign_in));
            //}
            //else
            //{
            //    Frame.Navigate(typeof(useraccount), username);
            //}
        }

    }
}
