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
    class Adapter_OrdersHistory : BaseAdapter<Orders_History>
    {

        Activity activity;
        ISharedPreferences sp;
        public  string userName;

        public List<Orders_History> AllOrders { get; set; }
       
        public Adapter_OrdersHistory()
        {

        }
        public Adapter_OrdersHistory(Activity activity, List<Orders_History> orders_history_list)//מקבלת אקטיביטי ומקבלת  רשימה של הזמנות שבוצעו
        {
            this.activity = activity;
       

            this.sp = activity.GetSharedPreferences("details", FileCreationMode.Private);
            userName = this.sp.GetString("Username", "");

        }




        public List<Orders_History> GetList()
        {
            return this.AllOrders;

        }

        public override long GetItemId(int position)  //  שהוא אותו מיקום שהכניס לפעולה ID מקבלת מיקום ומחזירה 
        {

            return position;
        }

        public override int Count    // מחזיר את כמות האיברים שיש 
        {
            get { return this.AllOrders.Count; }
        }



        public override Orders_History this[int position]
        {
            get
            {
                return this.AllOrders[position]; //מחזיר את האיבר במקום מסוים

            }

        }


        public override View GetView(int position, View convertView, ViewGroup parent)//הפעולה מקבלת שלוש דברים -מיקום שאליו יכנס הויאו בלולאת הפור ,
        {

            if (convertView == null)

            {
                convertView = this.activity.LayoutInflater.Inflate(Resource.Layout.layout_customOrderHistory, parent, false);/*מגדיר לו איזה לייאוט להפוך לוויאו*/
            }



            TextView tvDate = convertView.FindViewById<TextView>(Resource.Id.tvOrderHistoryRawDate);
          
            TextView tvPrice = convertView.FindViewById<TextView>(Resource.Id.tvOrderHistoryRawPrice);
            

            Orders_History temp_order = AllOrders[position];
          

            tvDate.Text = temp_order.Date.ToString();//מציג את   התאריך של אותה הזמנה
            tvPrice.Text = " סכום ההזמנה:  " + temp_order.Total_Price;//מציג כמה המשתמש שילם באותה הזמנה
         

            return convertView;
        }

    }



}
           