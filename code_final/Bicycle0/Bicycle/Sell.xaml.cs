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
using Windows.Graphics.Imaging;
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
    public sealed partial class Sell : Page
    {


        public Sell()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.DarkGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.DarkGreen;
        }

        public string imagename = @"dabai.jpg";
        public string username = "";
        public string userphone = "";
        public string school = "";

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            try
            {
                username = e.Parameter.ToString();
                user.Text = username;
            }
            catch (Exception ex)
            {

            }
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

        private void submit_button(object sender, RoutedEventArgs e) //卖车
        { //判断一些条件
            string str = "";
            if (title.Text == "")
                str += "车辆类型不得为空\n";
            if (age.Text == "")
                str += "使用年限不得为空\n";
            if (description.Text == "")
                str += "备注不得为空\n";
            if (price.Text == "")
                str += "价格不得为空\n";
            if (str != "")
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            else
            {
                // 卖车成功，插入数据库
                var db = App.conn_bicycle;
                try
                { // 得到图片信息
                    using (var statement = db.Prepare("SELECT userphone, school FROM user WHERE account = ?"))
                    {
                        statement.Bind(1, username);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            userphone = (string)statement[0];
                            school = (string)statement[1];
                        }
                    }
                }
                catch (Exception ex)
                {
                    var j = new MessageDialog(ex.ToString()).ShowAsync();
                }


                try
                { // 插入数据库
                    using (var bicyclemarket_insert = db.Prepare("INSERT INTO bicyclemarket(title, age, price, phonenumber, school, description, imagesource, user) VALUES(?, ?, ?, ?, ?, ?, ?, ?)"))
                    {
                        bicyclemarket_insert.Bind(1, title.Text);
                        bicyclemarket_insert.Bind(2, age.Text);
                        bicyclemarket_insert.Bind(3, price.Text);
                        bicyclemarket_insert.Bind(4, userphone);
                        bicyclemarket_insert.Bind(5, school);
                        bicyclemarket_insert.Bind(6, description.Text);
                        bicyclemarket_insert.Bind(7, imagename);
                        bicyclemarket_insert.Bind(8, "dabai");
                        bicyclemarket_insert.Step();
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString()).ShowAsync();
                }
                try
                { //插入数据库
                    using (var sell_insert = db.Prepare("INSERT INTO sell(title, age, price, phonenumber, school, description, imagesource, username) VALUES(?, ?, ?, ?, ?, ?, ?, ?)"))
                    {
                        sell_insert.Bind(1, title.Text);
                        sell_insert.Bind(2, age.Text);
                        sell_insert.Bind(3, price.Text);
                        sell_insert.Bind(4, userphone);
                        sell_insert.Bind(5, school);
                        sell_insert.Bind(6, description.Text);
                        sell_insert.Bind(7, imagename);
                        sell_insert.Bind(8, username);
                        sell_insert.Step();
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString()).ShowAsync();
                }
                Frame.Navigate(typeof(MainPage), username);
            }
        }

        public FileOpenPickerContinuationEventArgs FilePickerEvent { get; internal set; }

        private async void picture_selection_button(object sender, RoutedEventArgs e) //选择自行车的图片
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
                    image.Source = bitmapImage;
                }
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
