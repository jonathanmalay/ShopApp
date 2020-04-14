using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    public class Client_HomeFragment : Android.Support.V4.App.Fragment  
    {
        Button btnStartOrder, btnPruchesHistory, btnSetting, btnContectUs;
        TextView tv_toolbar_title;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)//inflate the view of the fragment 
        {
            return LayoutInflater.Inflate(Resource.Layout.layout_fragment_home, container, false);

        }

        public override void OnHiddenChanged(bool hidden)//whats happens when the fragment is hidden 
        {
            base.OnHiddenChanged(hidden);//if the fragment on the screen now
            if(hidden == false)
            {
                this.tv_toolbar_title.Text = "מסך ראשי";
            }
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)//
        {            
            base.OnViewCreated(view, savedInstanceState);
            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title); //change the toolbar title to the name of the fragment 
            this.tv_toolbar_title.Text = "מסך ראשי";


            this.btnStartOrder = view.FindViewById<Button>(Resource.Id.btnStartShop);
            this.btnSetting = view.FindViewById<Button>(Resource.Id.btnHomeSetting);
            this.btnPruchesHistory = view.FindViewById<Button>(Resource.Id.btnPruchesHistory);
            this.btnContectUs = view.FindViewById<Button>(Resource.Id.btnContectUs);


            this.btnSetting.Click += BtnSetting_Click;
            this.btnStartOrder.Click += BtnStartOrder_Click;
            this.btnPruchesHistory.Click += BtnPruchesHistory_Click;
            this.btnContectUs.Click += BtnContectUs_Click;

        }




        private async void BtnContectUs_Click(object sender, EventArgs e) //call to the shop  
        {
            try
            {


                string shop_phone = await Manager.GetShopPhone();
                
                Intent intent = new Intent();

                intent.SetAction(Intent.ActionDial);


                Android.Net.Uri data = Android.Net.Uri.Parse("tel:" + shop_phone); 

                intent.SetData(data);

                StartActivity(intent);

            }

            catch (Exception)
            {
                Toast.MakeText(Activity, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();

            }
        }



        private void BtnPruchesHistory_Click(object sender, EventArgs e) // move to the client orders history screen 
        {
            FragmentHelper.LoadFragment(Activity, new Client_HistoryOrders_Fragment(), false);
        }

        private void BtnStartOrder_Click(object sender, EventArgs e)// move to start order fragment 
        {
            FragmentHelper.LoadFragment( Activity, new ClientOrder_Fragment(), false);
        }


        private void BtnSetting_Click(object sender, EventArgs e)//move to  the client setting fragment
        {
            FragmentHelper.LoadFragment(Activity, new HomeSetting_Fragment(), false);

        }

    }
}