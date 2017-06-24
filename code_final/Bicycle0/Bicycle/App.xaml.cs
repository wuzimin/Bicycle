using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Bicycle
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.LoadDatabase_bicycle(); // 创建数据库
            this.Suspending += OnSuspending;
        }

        public static SQLiteConnection conn_bicycle = new SQLiteConnection("bicycle.db");

        private void LoadDatabase_bicycle()
        { // bicyclemarket数据库，用于所有的自行车
            string sql_bicyclemarket = @"CREATE TABLE IF NOT EXISTS
                            bicyclemarket(Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    title    VARCHAR( 140 ),
                                    age    VARCHAR( 140 ),
                                    price VARCHAR( 140 ),
                                    phonenumber VARCHAR( 140 ),
                                    school VARCHAR( 140 ),
                                    description VARCHAR( 200 ),
                                    imagesource VARCHAR ( 200 ),
                                    user VARCHAR( 100 )
                                    );";
            using (var statement = conn_bicycle.Prepare(sql_bicyclemarket))
            {
                statement.Step();
            }
            // user数据库，用于存储用户信息
            string sql_user = @"CREATE TABLE IF NOT EXISTS
                            user(Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    account    VARCHAR( 140 ),
                                    password    VARCHAR( 140 ),
                                    userphone VARCHAR( 140 ),
                                    school     VARCHAR( 140 ),
                                    userpicture BLOB( 2000 )
                                    );";
            using (var statement = conn_bicycle.Prepare(sql_user))
            {
                statement.Step();
            }
            // favorite数据库，用于保存收藏的信息
            string sql_favorite = @"CREATE TABLE IF NOT EXISTS
                            favorite(Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    title    VARCHAR( 140 ),
                                    age    VARCHAR( 140 ),
                                    price VARCHAR( 140 ),
                                    phonenumber VARCHAR( 140 ),
                                    school VARCHAR( 140 ),
                                    description VARCHAR( 200 ),
                                    imagesource VARCHAR ( 200 ),
                                    username VARCHAR( 100 )
                                    );";
            using (var statement = conn_bicycle.Prepare(sql_favorite))
            {
                statement.Step();
            }
            // sell数据库，用于保存用户所卖出车辆的信息
            string sql_sell = @"CREATE TABLE IF NOT EXISTS
                            sell(Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    title    VARCHAR( 140 ),
                                    age    VARCHAR( 140 ),
                                    price VARCHAR( 140 ),
                                    phonenumber VARCHAR( 140 ),
                                    school VARCHAR( 140 ),
                                    description VARCHAR( 200 ),
                                    imagesource VARCHAR ( 200 ),
                                    username VARCHAR( 100 )
                                    );";
            using (var statement = conn_bicycle.Prepare(sql_sell))
            {
                statement.Step();
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }
        
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
    }
}

