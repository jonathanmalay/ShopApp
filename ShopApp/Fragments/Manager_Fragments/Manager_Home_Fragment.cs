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
    public class Manager_Home_Fragment : Android.Support.V4.App.Fragment
    {
        Button btn_EditProducts, btnOrders, btnSetting;
        TextView tv_toolbar_title;




        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            return LayoutInflater.Inflate(Resource.Layout.layout_Manager_Home_Fragment, container, false);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (hidden == false)
            {

                this.tv_toolbar_title.Text = "מסך הבית";

            }
        }



        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "דף הבית"; //set the activity title
            this.btn_EditProducts = view.FindViewById<Button>(Resource.Id.btnManagerHomeEditProducts);
            this.btnOrders = view.FindViewById<Button>(Resource.Id.btnManagerHomeOrders);
            this.btnSetting = view.FindViewById<Button>(Resource.Id.btnManagerHomeSetting);


            this.btn_EditProducts.Click += Btn_EditProducts_Click;
            this.btnOrders.Click += BtnOrders_Click;
            this.btnSetting.Click += BtnSetting_Click;

        }



        private void Btn_EditProducts_Click(object sender, EventArgs e)
        {
            try
            {
                FragmentHelper.LoadFragment(Activity, new Fragment_ManagerProducts());
            }


            catch (Exception)
            {

            }

        }


        private void BtnSetting_Click(object sender, EventArgs e)
        {
            try
            {
                FragmentHelper.LoadFragment(Activity, new Fragment_ManagerHomeSetting());

            }


            catch (Exception)
            {

            }
        }


        private void BtnOrders_Click(object sender, EventArgs e)
        {
            try
            {
                FragmentHelper.LoadFragment(Activity, new Fragment_ManagerOrders());

            }

            catch (Exception)
            {

            }
        }
    }


  }