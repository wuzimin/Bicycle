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
    public sealed partial class mysell : Page
    {
        public FileOpenPickerContinuationEventArgs FilePickerEvent { get; internal set; }

        public mysell()
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
            try //得到用户信息
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
            { //展示某个用户所卖的车
                using (var statement = db.Prepare("SELECT title, age, price, phonenumber, school, description, imagesource FROM sell WHERE username = ?"))
                {
                    statement.Bind(1, username);
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

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args) //share
        {
            try
            {
                var request = args.Request;
                request.Data.Properties.Title = ("we share!");
                request.Data.Properties.Description = ("my bicycle, my favorite!");
                string content = ("车辆类型：  " + title.Text //车辆信息
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

        private void delete_Click(object sender, RoutedEventArgs e) //删除所卖的车，bicyclemarket也要删除
        {
            if (title.Text == "")
            {
                var i = new MessageDialog("请选择自行车！").ShowAsync();
            }
            else
            {
                var db = App.conn_bicycle;
                try
                {
                    using (var statement = db.Prepare("DELETE FROM bicyclemarket WHERE title = ?"))
                    {
                        statement.Bind(1, title.Text);
                        statement.Step();
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString(), "删除失败！\n").ShowAsync();
                }
                try
                {
                    using (var statement = db.Prepare("DELETE FROM sell WHERE title = ?"))
                    {
                        statement.Bind(1, title.Text);
                        statement.Step();
                        Frame.Navigate(typeof(mysell), username);
                    }
                }
                catch (Exception ex)
                {
                    var i = new MessageDialog(ex.ToString(), "删除失败！\n").ShowAsync();
                }

                var info = new MessageDialog("删除成功！\n").ShowAsync();
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

        private void home_button(object sender, RoutedEventArgs e) //主页面
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
