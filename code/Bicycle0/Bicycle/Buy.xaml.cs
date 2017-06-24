using Bicycle.Models;
using Microsoft.VisualBasic.CompilerServices;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class Buy : Page
    {
        public FileOpenPickerContinuationEventArgs FilePickerEvent { get; internal set; }

        public Buy()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.DarkGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.DarkGreen;
            this.ViewModel = new ViewModels.SellItemViewModel(image);
        }


        public string username = "";

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
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;

            var db = App.conn_bicycle;
            try
            { // 展示所有车辆
                using (var statement = db.Prepare("SELECT title, age, price, phonenumber, school, description, imagesource FROM bicyclemarket WHERE user = ?"))
                {
                    statement.Bind(1, "dabai");
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        string title_ = (string)statement[0];
                        string age_ = (string)statement[1];
                        string price_ = (string)statement[2];
                        string phonenumber_ = (string)statement[3];
                        string school_ = (string)statement[4];
                        string description_ = (string)statement[5];
                        string imagesource_ = (string)statement[6];
                        ViewModel.AddSellItem(title_, description_, age_, price_, phonenumber_, school_, new BitmapImage(new Uri(image.BaseUri, ("Assets/" + imagesource_))));
                    }

                }
            }
            catch (Exception ex)
            {
                var j = new MessageDialog(ex.ToString()).ShowAsync();
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



        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        ViewModels.SellItemViewModel ViewModel { get; set; }

        private void Sell_Clicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (SellItem)(e.ClickedItem);
            if (mainpage2.Visibility == Visibility.Collapsed)
                Frame.Navigate(typeof(Bicycle_details), ViewModel);
            else
            {
                title.Text = ViewModel.SelectedItem.title;
                description.Text = ViewModel.SelectedItem.description;
                age.Text = ViewModel.SelectedItem.age;
                price.Text = ViewModel.SelectedItem.price;
                phonenumber.Text = ViewModel.SelectedItem.phonenumber;
                school.Text = ViewModel.SelectedItem.school;
                image.Source = ViewModel.SelectedItem.imagesource;
            }
        }

        private void share_button(object sender, RoutedEventArgs e)   // 分享
        {
            DataTransferManager.ShowShareUI();
        }

        private void Favorite_button(object sender, RoutedEventArgs e) // 收藏
        {
            if (username == "")
            {
                var i = new MessageDialog("请先登录！").ShowAsync(); //收藏需要先登录
            }
            else if (title.Text == "")
            {
                var i = new MessageDialog("请选择自行车！").ShowAsync(); //选择自行车
            }
            else
            {
                string imagename = "";
                var db = App.conn_bicycle;
                try
                { //得到图片信息
                    using (var statement = db.Prepare("SELECT imagesource FROM bicyclemarket WHERE title = ?"))
                    {
                        statement.Bind(1, title.Text);
                        while (statement.Step() == SQLiteResult.ROW)
                        {
                            imagename = (string)statement[0];
                        }
                    }
                }
                catch (Exception ex)
                {
                    var j = new MessageDialog(ex.ToString()).ShowAsync();
                }
                try
                { // 插入favorite数据库，保存
                    using (var sell_insert = db.Prepare("INSERT INTO favorite(title, age, price, phonenumber, school, description, imagesource, username) VALUES(?, ?, ?, ?, ?, ?, ?, ?)"))
                    {
                        sell_insert.Bind(1, title.Text);
                        sell_insert.Bind(2, age.Text);
                        sell_insert.Bind(3, price.Text);
                        sell_insert.Bind(4, phonenumber.Text);
                        sell_insert.Bind(5, school.Text);
                        sell_insert.Bind(6, description.Text);
                        sell_insert.Bind(7, imagename);
                        sell_insert.Bind(8, username);
                        sell_insert.Step();
                        var j = new MessageDialog("收藏成功！").ShowAsync();
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString()).ShowAsync();
                }
            }
        }
        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args) //share
        {
            try
            {
                var request = args.Request;
                request.Data.Properties.Title = ("we share!");
                request.Data.Properties.Description = ("my bicycle, my favorite!");
                string content = ("车辆类型：  " + title.Text
                                + "\n价格：  " + price.Text
                                + "\n使用年限：  " + age.Text
                                + "\n学校：  " + school.Text
                                + "\n备注：  " + description.Text
                                + "\n联系电话：  " + phonenumber.Text
                                + "\n");
                request.Data.SetText(content.Trim());
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.ToString()).ShowAsync();
            }
        }

        private void account_button(object sender, RoutedEventArgs e) //跳转到个人中心
        {
            if (username == "") //需要先登录
            {
                Frame.Navigate(typeof(Sign_in));
            }
            else
            {
                Frame.Navigate(typeof(useraccount), username);
            }
        }

        private void home_button(object sender, RoutedEventArgs e) //回到主页面
        {
            Frame.Navigate(typeof(MainPage));

        }
    }
}
