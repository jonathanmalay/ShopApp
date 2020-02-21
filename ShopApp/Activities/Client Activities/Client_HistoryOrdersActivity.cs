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
    public class Client_HistoryOrdersActivity : Activity
    {
        ISharedPreferences sp;
        string userName;

        ListView lvOrdersHistory;
        Orders_History Orders_History;
        Adapter_OrdersHistory pa_OrdersHistory;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.layout_OrdersHistory);

                this.sp = GetSharedPreferences("details", FileCreationMode.Private);
                this.userName = this.sp.GetString("Username", "");

                this.lvOrdersHistory = FindViewById<ListView>(Resource.Id.listViewOrdersHistory);


                List<Orders_History> orders = new List<Orders_History>();
                orders = await Orders_History.GetAllOrders(userName);//מביא  רשימה של כל ההזמנות שביצע המשתמש 

                this.pa_OrdersHistory = new Adapter_OrdersHistory(this, orders);//מקבל אקטיביטי ואת רשימת ההזמנות שביצע המשתמש

                this.lvOrdersHistory.Adapter = this.pa_OrdersHistory;//אומר לליסט ויואו שהוא עובד עם המתאם הזה


                this.pa_OrdersHistory.NotifyDataSetChanged(); //הפעלת המתאם

                this.lvOrdersHistory.ItemClick += LvOrdersHistory_ItemClick;


            }

            catch(Exception)
            {

            }
        }

        private void LvOrdersHistory_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}