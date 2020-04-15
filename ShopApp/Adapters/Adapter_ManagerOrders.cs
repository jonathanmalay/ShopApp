using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    class Adapter_ManagerOrders : BaseAdapter<Manager_Order>
    {
        Activity activity;
        ISharedPreferences sp;


        public List<Manager_Order> Manager_orders_list { get; set; }

        public Adapter_ManagerOrders(Activity activity , List<Manager_Order> orders_list )
        {
            this.activity = activity;
            this.Manager_orders_list = orders_list;

        }

        public override int Count    // מחזיר את כמות האיברים שיש 
        {
            get { return this.Manager_orders_list.Count; }
        }

        public override Manager_Order this[int position]
        {
            get
            {
                return this.Manager_orders_list[position];
            }
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)//return the view of the cell in the list view 
        {
            if (convertView == null)
            {
                convertView = this.activity.LayoutInflater.Inflate(Resource.Layout.layout_costum_ManagerOrdersAdapter, parent, false);
            }

            TextView tv_ClientName = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerOrdersAdapterUsername);
            TextView tv_IsDeliverd = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerOrdersAdapterIsDeliverd);
            TextView tv_OrderDate = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerOrdersAdapterDate);
            TextView tv_OrderPrice = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerOrdersAdapterPrice);
            TextView tv_OrderCity = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerOrdersAdapterCity);
            TextView tv_OrderAddress = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerOrdersAdapterAddress);
            TextView tv_OrderPhoneNum = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerOrdersAdapterPhoneNum);


            Manager_Order temp_order = Manager_orders_list[position];

            tv_ClientName.Text = "שם הלקוח: " + temp_order.ClientUsername ; 
            tv_OrderDate.Text = "תאריך הזמנה: " + temp_order.Date.ToString() ;
            tv_OrderPrice.Text = "סכום הזמנה: " + temp_order.Price.ToString() ;
            tv_OrderCity.Text = "עיר מגורים: " + temp_order.City;
            tv_OrderAddress.Text =  "כתובת מגורים: "  + temp_order.Address ;
            tv_OrderPhoneNum.Text = "טלפון לקוח: " + temp_order.ClientPhone ;
           if(temp_order.IsDelivered)
            {
                tv_IsDeliverd.Text = "נשלח ללקוח בהצלחה";

            }

            else
            {
                tv_IsDeliverd.Text = "לא בוצע משלוח עדיין" ;
            }

            return convertView;
        }

       

    }

  
}