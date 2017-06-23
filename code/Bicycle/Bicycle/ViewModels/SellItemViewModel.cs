using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Bicycle.ViewModels
{
    class SellItemViewModel
    {
        private ObservableCollection<Models.SellItem> allItems = new ObservableCollection<Models.SellItem>();
        public ObservableCollection<Models.SellItem> AllItems { get { return this.allItems; } }

        private Models.SellItem selectedItem = default(Models.SellItem);
        public Models.SellItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public SellItemViewModel(object sender) //用来测试的样例，已删除
        {
            Image image = sender as Image;
            
        }

        public void AddSellItem(string title, //添加车辆
            string description,
            string age,
            string price,
            string phonenumber,
            string school,
            ImageSource src)
        {
            this.allItems.Add(new Models.SellItem(title, description, age, price, phonenumber, school, src));
        }

        public void RemoveSellItem()              //用于个人中心用户删除已卖出的车
        {
            // DIY
            this.allItems.Remove(selectedItem);
            // set selectedItem to null after remove
            this.selectedItem = null;
        }

    }
}
