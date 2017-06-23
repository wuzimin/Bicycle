using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Bicycle.Models
{
    class SellItem
    {
        private string id;

        public string title { get; set; }         //类型
         
        public string description { get; set; }   //备注

        public string age { get; set; }           //车龄

        public string price { get; set; }         //价格

        public string phonenumber { get; set; }   //联系人信息

        public string school { get; set; }        //学校

        public ImageSource imagesource { get; set; }   //车辆图片


        public SellItem(string title,
            string description,
            string age,
            string price,
            string phonenumber,
            string school,
            ImageSource src)
        {
            this.id = Guid.NewGuid().ToString(); //生成id
            this.title = title;
            this.age = age;
            this.price = price;
            this.phonenumber = phonenumber;
            this.school = school;
            this.description = description;
            this.imagesource = src;
        }
    }
}
