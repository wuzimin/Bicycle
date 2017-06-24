using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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
    public sealed partial class Sign_up : Page
    {
        public Sign_up()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.DarkGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.DarkGreen;
        }

        public string imagename = @"dabai.jpg";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void SignUp_button(object sender, RoutedEventArgs e) //注册
        {
            string str = "";
            //图片转为byte[]
            byte[] photo = null;
            string strpath = "Assets/" + imagename;
            FileStream fs = new System.IO.FileStream(@strpath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            photo = br.ReadBytes((int)fs.Length);
            //判断一些条件
            //if (account.Text == "")
            //    str += "账号不得为空\n";
            //if (password.Password == "" || passwordConfirm.Password == "")
            //    str += "密码不得为空\n";
            //if (phoneNumber.Text == "")
            //    str += "电话号码不得为空\n";
            //if (school.Text == "")
            //    str += "学校不得为空\n";
            if (phoneNumber.Text != "") //判断电话号码是否合理
            {
                System.String ex = "^[1][358][0-9]{9}$";
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(ex);
                if (!(reg.IsMatch(phoneNumber.Text)))
                    str += "电话号码不存在";
            }
            if (str != "")
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            else if (passwordConfirm.Password != password.Password)
            {
                var i = new MessageDialog("密码不一致，请再次确认！").ShowAsync();
            }
            else
            {
                var db = App.conn_bicycle;
                bool flag = true;
                try
                { //判断账户是否已经存在
                    using (var statement = db.Prepare("SELECT password FROM user WHERE account = ?"))
                    {
                        statement.Bind(1, account.Text);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            if ((string)statement[0] != null)
                            {
                                flag = false;
                            }
                        }
                        if (!flag)
                        {
                            var i = new MessageDialog("账户已存在！").ShowAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString()).ShowAsync();
                }
                if (flag)
                {
                    try
                    { //插入user数据库
                        using (var user_insert = db.Prepare("INSERT INTO user(account, password, userphone, school, userpicture) VALUES(?, ?, ?, ?, ?)"))
                        {
                            user_insert.Bind(1, account.Text);
                            user_insert.Bind(2, password.Password);
                            user_insert.Bind(3, phoneNumber.Text);
                            user_insert.Bind(4, school.Text);
                            user_insert.Bind(5, photo);
                            user_insert.Step();
                        }
                    }
                    catch (Exception ex)
                    {
                        var i = new MessageDialog(ex.ToString(), "\n\n\n注册失败！").ShowAsync();
                    }
                    Frame.Navigate(typeof(Sign_in));
                }
            }
        }

        private void home_button(object sender, RoutedEventArgs e) // 主页面
        {
            Frame.Navigate(typeof(MainPage));
        }


        public FileOpenPickerContinuationEventArgs FilePickerEvent { get; internal set; }

        private async void picture_selection_button(object sender, RoutedEventArgs e) //选择个人图片
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.ContinuationData["Operation"] = "Image";
            StorageFile file = await openPicker.PickSingleFileAsync();
            try
            {
                imagename = file.Name;
            }
            catch (Exception ex)
            {
                //var i = new MessageDialog(ex.ToString()).ShowAsync();
                //do nothing
            }
            if (file != null)
            {
                // set the image source to the selected bitmap
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    userimage.Source = bitmapImage;
                }
            }
        }
    }
}
