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
   
    public class Client_HistoryOrders_Fragment : Android.Support.V4.App.Fragment
    {
        ISharedPreferences sp;
        string userName;

        ListView lvOrdersHistory;
        Orders_History Orders_History;
        Adapter_OrdersHistory pa_OrdersHistory;
        List<Orders_History> allOrders;
        TextView tv_toolbar_title, tv_orders_Status; 
    

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.Fragment_ClientOrdersHistory, container, false);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if(hidden == false)
            {
                this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title); //change the toolbar title to the name of the fragment 
                this.tv_toolbar_title.Text = "היסטוריית הזמנות";
            }
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title); //change the toolbar title to the name of the fragment 
            this.tv_toolbar_title.Text = "היסטוריית הזמנות";

            this.tv_orders_Status = view.FindViewById<TextView>(Resource.Id.tv_Fragment_OrdersHistory_Status);
            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");
            User u = await User.GetUser(userName);

            this.allOrders = await Orders_History.GetAllOrders(u.Username);

            if (this.allOrders == null || this.allOrders.Count ==  0)//בודק האם יש ללקוח הזמנותקודמות ובמידה וכן עובר לאקטיביטי הזמנות קודמות
            {
                this.tv_orders_Status.Text = "  ...טרם ביצעת הזמנות  "; //change the text on the page
                Toast.MakeText(Activity, "אין הזמנות ישנות", ToastLength.Long).Show();
            }

            this.lvOrdersHistory = view.FindViewById<ListView>(Resource.Id.listViewOrdersHistory);


            this.pa_OrdersHistory = new Adapter_OrdersHistory(Activity, this.allOrders);//מקבל אקטיביטי ואת רשימת ההזמנות שביצע המשתמש

            this.lvOrdersHistory.Adapter = this.pa_OrdersHistory;


            this.pa_OrdersHistory.NotifyDataSetChanged(); //הפעלת המתאם

            this.lvOrdersHistory.ItemClick += LvOrdersHistory_ItemClick;



        }


        private void LvOrdersHistory_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
        }
    }
}