using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Bicycle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sign_in : Page
    {
        public Sign_in()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.DarkGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.DarkGreen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void SignIn_button(object sender, RoutedEventArgs e)
        {
            // 判断账户密码是否为空
            string str = "";
            if (_account.Text == "")
                str += "账号不得为空\n";
            if (password.Password == "")
                str += "密码不得为空\n";
            if (str != "")
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            //判断是否可以登录
            else
            {
                var db = App.conn_bicycle;
                var sql = _account.Text;
                try
                {
                    using (var statement = db.Prepare("SELECT password FROM user WHERE account = ?"))
                    {
                        statement.Bind(1, _account.Text);
                        bool flag = true;
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            if (password.Password == (string)statement[0]) // 登陆成功
                            {
                                flag = false;
                                var i = new MessageDialog("登陆成功！").ShowAsync();
                                Frame.Navigate(typeof(MainPage), _account.Text);
                            }
                            else
                            { // 密码错误
                                flag = false;
                                var i = new MessageDialog("密码错误，请重试！").ShowAsync();
                            }

                        }
                        if (flag) // 账户不存在
                        {
                            var j = new MessageDialog("账户不存在").ShowAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString()).ShowAsync();
                }
            }
        }

        private void SignUp_button(object sender, RoutedEventArgs e) //注册
        {
            Frame.Navigate(typeof(Sign_up));
        }

        private void home_button(object sender, RoutedEventArgs e) // 回到主页
        {
            Frame.Navigate(typeof(MainPage));
        }

    }
}
