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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.layout_fragment_home, container, false);

        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {            
            base.OnViewCreated(view, savedInstanceState);

            this.btnStartOrder = view.FindViewById<Button>(Resource.Id.btnStartShop);
            this.btnSetting = view.FindViewById<Button>(Resource.Id.btnHomeSetting);
            this.btnPruchesHistory = view.FindViewById<Button>(Resource.Id.btnPruchesHistory);
            this.btnContectUs = view.FindViewById<Button>(Resource.Id.btnContectUs);


            this.btnSetting.Click += BtnSetting_Click;
            this.btnStartOrder.Click += BtnStartOrder_Click;
            this.btnPruchesHistory.Click += BtnPruchesHistory_Click;
            this.btnContectUs.Click += BtnContectUs_Click;

        }




        private void BtnContectUs_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent();

                intent.SetAction(Intent.ActionDial);

                Android.Net.Uri data = Android.Net.Uri.Parse("tel:" + "053-8285819"); //חייוג

                intent.SetData(data);

                StartActivity(intent);

            }

            catch (Exception)
            {
                Toast.MakeText(Activity, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();

            }
        }

        private void BtnPruchesHistory_Click(object sender, EventArgs e)
        {
            FragmentHelper.LoadFragment(, new Client_HistoryOrdersActivity());

        }

        private void BtnStartOrder_Click(object sender, EventArgs e)
        {

            FragmentHelper.LoadFragment( , new ClientOrder_Activity());

        }




        private void BtnSetting_Click(object sender, EventArgs e)
        {
            FragmentHelper.LoadFragment(, new HomeSetting_Activityt());


        }

    }
}