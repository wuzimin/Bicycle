using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Bicycle
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class recorrect : Page
    {
        public recorrect()
        {
            this.InitializeComponent();
        }

        public string username = "";
        public byte[] photo = null;
        public string imagename = "";
        public string password_before = "";

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //do nothing
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e) //展示个人信息，图片与账户
        {
            Frame rootFrame = Window.Current.Content as Frame;
            username = e.Parameter.ToString();
            var db = App.conn_bicycle;
            try
            {
                using (var statement = db.Prepare("SELECT password, userpicture FROM user WHERE account = ?"))
                {
                    statement.Bind(1, username);
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        password_before = (string)statement[0];
                        photo = (byte[])statement[1];
                    }
                }
            }
            catch (Exception ex)
            {
                var j = new MessageDialog(ex.ToString()).ShowAsync();
            }
            //将byte[]转为图片
            StorageFolder fold_write = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file_write = await fold_write.CreateFileAsync(@"test", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file_write, photo);
            StorageFolder fold_read = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file_read = await fold_read.GetFileAsync(@"test");
            using (IRandomAccessStream fileStream = await file_read.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);
                image.Source = bitmapImage;
            }

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

        private void home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), username);
        }

        private async void picture_selection_Click(object sender, RoutedEventArgs e) //选择个人图片并修改
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.ContinuationData["Operation"] = "Image";
            StorageFile file = await openPicker.PickSingleFileAsync();
            try // 如果没选择图片就关闭会报错
            {
                imagename = file.Name; //得到图片信息
            }
            catch (Exception ex)
            {
                //do nothing
            }
            if (file != null)
            {
                // set the image source to the selected bitmap
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    image.Source = bitmapImage;
                }
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e) //更新个人信息
        {
            if (imagename != "")
            {
                photo = null;
                string strpath = "Assets/" + imagename;
                FileStream fs = null;
                fs = new System.IO.FileStream(@strpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                photo = br.ReadBytes((int)fs.Length);
            }
            string str = ""; //判断一些条件
            if (before.Password != password_before)
                str += "密码错误！\n";
            if (password.Password == "" || passwordConfirm.Password == "")
                str += "密码不得为空！\n";
            if (phoneNumber.Text == "")
                str += "电话号码不得为空！\n";
            if (school.Text == "")
                str += "学校不得为空！\n";
            //if (phoneNumber.Text != "") //判断电话号码是否合理
            //{
            //    System.String ex = "^[1][358][0-9]{9}$";
            //    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(ex);
            //    if (!(reg.IsMatch(phoneNumber.Text)))
            //        str += "电话号码不存在";
            //}
            //if (str != "")
            //{
            //    var i = new MessageDialog(str).ShowAsync();
            //}
            //else if (password.Password != passwordConfirm.Password)
            //{
            //    var i = new MessageDialog("新密码不一致，请重试！").ShowAsync();
            //}
            else
            {
                //更新个人信息 
                var db = App.conn_bicycle;
                try
                {
                    using (var user_update = db.Prepare("UPDATE user SET password = ?, userphone = ?, school = ?, userpicture = ? WHERE account = ?"))
                    {
                        user_update.Bind(1, password.Password);
                        user_update.Bind(2, phoneNumber.Text);
                        user_update.Bind(3, school.Text);
                        user_update.Bind(4, photo);
                        user_update.Bind(5, username);
                        user_update.Step();
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString()).ShowAsync();
                }
                Frame.Navigate(typeof(Sign_in));
            }
        }

        private void account_button(object sender, RoutedEventArgs e) //个人中心
        {
            if (username == "")
            {
                Frame.Navigate(typeof(Sign_in));
            }
            else
            {
                Frame.Navigate(typeof(useraccount), username);
            }
        }

        private void home_button(object sender, RoutedEventArgs e) //主页面
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
