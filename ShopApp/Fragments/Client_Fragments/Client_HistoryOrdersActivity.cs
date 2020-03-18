﻿using System;
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
    [Activity(Label = "Client_HistoryOrdersActivity")]
    public class Client_HistoryOrdersActivity : Android.Support.V4.App.Fragment
    {
        ISharedPreferences sp;
        string userName;

        ListView lvOrdersHistory;
        Orders_History Orders_History;
        Adapter_OrdersHistory pa_OrdersHistory;
        List<Orders_History> allOrders;

        Button btn_backPage;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.layout_OrdersHistory, container, false);
        }
        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);



            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");
            User u = await User.GetUser(userName);

            this.allOrders = await Orders_History.GetAllOrders(u.Username);

            if (this.allOrders == null || this.allOrders.Count ==  0)//בודק האם יש ללקוח הזמנותקודמות ובמידה וכן עובר לאקטיביטי הזמנות קודמות
            {
                Toast.MakeText(Activity, "אין הזמנות ישנות", ToastLength.Long).Show();
            }

            this.lvOrdersHistory = view.FindViewById<ListView>(Resource.Id.listViewOrdersHistory);


            this.pa_OrdersHistory = new Adapter_OrdersHistory(Activity, this.allOrders);//מקבל אקטיביטי ואת רשימת ההזמנות שביצע המשתמש

            this.lvOrdersHistory.Adapter = this.pa_OrdersHistory;//אומר לליסט ויואו שהוא עובד עם המתאם הזה


            this.pa_OrdersHistory.NotifyDataSetChanged(); //הפעלת המתאם

            this.lvOrdersHistory.ItemClick += LvOrdersHistory_ItemClick;



        }


        private void LvOrdersHistory_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}