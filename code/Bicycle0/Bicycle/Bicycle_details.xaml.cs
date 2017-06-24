using Bicycle.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class Bicycle_details : Page
    {
        private ViewModels.SellItemViewModel ViewModel;
        public Bicycle_details()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.DarkGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.DarkGreen;
        }
        public string username = "";

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            try // 得到用户的账号，以便得到用户信息
            {
                username = e.Parameter.ToString();
            }
            catch (Exception ex)
            {

            }

            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
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

            ViewModel = ((ViewModels.SellItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
            }
            else // 赋值
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
        private void share_button(object sender, RoutedEventArgs e) //share
        {
            DataTransferManager.ShowShareUI();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args) // share
        {
            try
            {
                var request = args.Request;
                request.Data.Properties.Title = ("we share!");
                request.Data.Properties.Description = ("my bicycle, my favorite!");
                string content = ("车辆类型：  " + title.Text
                                + "\n价格：      " + price.Text
                                + "\n使用年限：  " + age.Text
                                + "\n学校：      " + school.Text
                                + "\n备注：      " + description.Text
                                + "\n联系电话：  " + phonenumber.Text
                                + "\n");
                request.Data.SetText(content.Trim());
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.ToString()).ShowAsync();
            }
        }

        private void home_button(object sender, RoutedEventArgs e) //回到主页
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void account_button(object sender, RoutedEventArgs e)
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
    }
}
