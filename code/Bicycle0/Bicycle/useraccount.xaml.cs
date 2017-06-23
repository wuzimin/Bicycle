using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class useraccount : Page
    {
        public useraccount()
        {
            this.InitializeComponent();
        }

        public byte[] photo = null;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            { //得到用户信息
                userAccount.Text = e.Parameter.ToString();
            }
            catch (Exception ex)
            {

            }
            var db = App.conn_bicycle;
            try
            { //展示用户信息
                using (var statement = db.Prepare("SELECT userphone, school, userpicture FROM user WHERE account = ?"))
                {
                    statement.Bind(1, userAccount.Text);
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        account.Text = "账户：" + userAccount.Text;
                        userphone.Text = "联系方式：" + (string)statement[0];
                        school.Text = "学校：" + (string)statement[1];
                        photo = (byte[])statement[2];
                    }

                }
            }
            catch (Exception ex)
            {
                var j = new MessageDialog(ex.ToString()).ShowAsync();
            }
            //byte[]转为图片
            StorageFolder fold_write = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file_write = await fold_write.CreateFileAsync(@"test", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file_write, photo);
            var fold_read = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file_read = await fold_read.GetFileAsync(@"test");
            using (IRandomAccessStream fileStream = await file_read.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);
                image.Source = bitmapImage;
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)//主页面
        {
            Frame.Navigate(typeof(MainPage), userAccount.Text);
        }

        private void setting_Click(object sender, RoutedEventArgs e)//修改个人信息
        {
            Frame.Navigate(typeof(recorrect), userAccount.Text);
        }

        private void favorite_Click(object sender, RoutedEventArgs e)//个人收藏
        {
            Frame.Navigate(typeof(collection), userAccount.Text);
        }

        private void selling_bike_Click(object sender, RoutedEventArgs e)//个人卖车
        {
            Frame.Navigate(typeof(mysell), userAccount.Text);
        }

        private void log_out(object sender, RoutedEventArgs e) //退出登录，切换用户
        {
            userAccount.Text = "";
            var i = new MessageDialog("退出成功！").ShowAsync();
            Frame.Navigate(typeof(MainPage), userAccount.Text);
        }
    }
}
